#region Copyright
   /*Copyright (C) 2015 Konstantin Udilovich

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Concrete.ACI318_14
{
    public class ConcreteSectionCompression : ConcreteCompressionSectionBase
    {
        public ConcreteSectionCompression(IConcreteSectionWithLongitudinalRebar Section, 
        ConfinementReinforcementType ConfinementReinforcementType,
         ICalcLog log, bool IsPrestressed =false )
            : base(Section.Section, Section.LongitudinalBars, log)
        {
            this.ConfinementReinforcementType = ConfinementReinforcementType;
            switch (ConfinementReinforcementType)
            {
                case ConfinementReinforcementType.Spiral:
                    this.CompressionMemberType = IsPrestressed == false ? CompressionMemberType.NonPrestressedWithSpirals : CompressionMemberType.PrestressedWithSpirals;

                    break;
                case ConfinementReinforcementType.Ties:
                    this.CompressionMemberType = IsPrestressed == false ? CompressionMemberType.NonPrestressedWithTies : CompressionMemberType.PrestressedWithTies;
                    break;
                case ConfinementReinforcementType.NoReinforcement:
                    throw new Exception("Invalid type of ConfinementReinforcementType variable. Specify either ties or spirals. Alternatively use nodes for plain concrete design");
                    break;
                this.CompressionMemberType = IsPrestressed == false ? CompressionMemberType.NonPrestressedWithTies : CompressionMemberType.PrestressedWithTies;
                    break;
            }
        }

        public ConcreteSectionCompression(IConcreteSection Section,
            List<RebarPoint> LongitudinalBars, CompressionMemberType CompressionMemberType, ICalcLog log)
            : base(Section, LongitudinalBars, log)
        {
            this.CompressionMemberType = CompressionMemberType;
            switch (CompressionMemberType)
            {
                case CompressionMemberType.NonPrestressedWithTies:
                    ConfinementReinforcementType = ACI.ConfinementReinforcementType.Ties;
                    break;
                case CompressionMemberType.NonPrestressedWithSpirals:
                    ConfinementReinforcementType = ConfinementReinforcementType.Spiral;
                    break;
                case CompressionMemberType.PrestressedWithTies:
                    ConfinementReinforcementType = ConfinementReinforcementType.Ties;
                    break;
                case CompressionMemberType.PrestressedWithSpirals:
                    ConfinementReinforcementType = ConfinementReinforcementType.Spiral;
                    break;
                default:
                    ConfinementReinforcementType = ACI.ConfinementReinforcementType.Ties;
                    break;
            }
        }

        public ConcreteSectionCompression(ConcreteSectionFlexure FlexuralSection, CompressionMemberType CompressionMemberType, ICalcLog log)
            : base(FlexuralSection.Section, FlexuralSection.LongitudinalBars, log)
        {
            this.CompressionMemberType = CompressionMemberType;

            switch (CompressionMemberType)
            {
                case CompressionMemberType.NonPrestressedWithTies:
                    ConfinementReinforcementType = ACI.ConfinementReinforcementType.Ties;
                    break;
                case CompressionMemberType.NonPrestressedWithSpirals:
                    ConfinementReinforcementType = ConfinementReinforcementType.Spiral;
                    break;
                case CompressionMemberType.PrestressedWithTies:
                    ConfinementReinforcementType = ConfinementReinforcementType.Ties;
                    break;
                case CompressionMemberType.PrestressedWithSpirals:
                    ConfinementReinforcementType = ConfinementReinforcementType.Spiral;
                    break;
                default:
                    ConfinementReinforcementType = ACI.ConfinementReinforcementType.Ties;
                    break;
            }
        }


        private ConfinementReinforcementType _ConfinementReinforcementType;

        public ConfinementReinforcementType ConfinementReinforcementType
        {
        get { return _ConfinementReinforcementType;}
        set { _ConfinementReinforcementType = value;}
        }
	
        private CompressionMemberType compressionMemberType;

        public CompressionMemberType CompressionMemberType
        {
            get { return compressionMemberType; }
            set { compressionMemberType = value; }
        }

        public  ConcreteCompressionStrengthResult GetDesignMomentWithCompressionStrength( double phiP_n,
            FlexuralCompressionFiberPosition FlexuralCompressionFiberPosition,
             bool CapAxialForceAtMaximum = true)
        {

            this.phiP_n = phiP_n; //store value for iteration
            this.FlexuralCompressionFiberPosition = FlexuralCompressionFiberPosition;
            double P_o = GetMaximumForce();
            StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            double phiAxial = f.Get_phiFlexureAndAxial(FlexuralFailureModeClassification.CompressionControlled, ConfinementReinforcementType, 0, 0);
            double phiP_nMax = phiAxial * P_o;

            if (phiP_n > phiP_nMax)
            {
                if (CapAxialForceAtMaximum == false)
                {
                    throw new Exception("Axial forces exceeds maximum axial force.");
                }
                else
                {
                    phiP_n = phiP_n;
                }
                
            }

            //Estimate resistance factor to adjust from phiP_n to P_n
            double phiMin =  0.65;
            double phiMax =  0.9;

            double ConvergenceTolerance = 0.0001;
            double targetPhiFactorDifference = 0.0;
            //Find P_n by guessing a phi-factor and calculating the result
            double phiIterated = RootFinding.Brent(new FunctionOfOneVariable(phiFactorDifferenceCalculation), phiMax, phiMin, ConvergenceTolerance, targetPhiFactorDifference);
            double P_nActual = phiP_n / phiIterated;

            //Calculate final results using the estimated value of phi
            IStrainCompatibilityAnalysisResult nominalResult = this.GetNominalMomentResult(P_nActual, FlexuralCompressionFiberPosition);
            ConcreteCompressionStrengthResult result = new ConcreteCompressionStrengthResult(nominalResult, FlexuralCompressionFiberPosition, this.Section.Material.beta1);
            FlexuralFailureModeClassification failureMode = f.GetFlexuralFailureMode(result.epsilon_t, result.epsilon_ty);
            double phiFinal = f.Get_phiFlexureAndAxial(failureMode, ConfinementReinforcementType, result.epsilon_t, result.epsilon_ty);
            double phiM_n = phiFinal * nominalResult.Moment;
            result.phiM_n = phiM_n; 
            result.FlexuralFailureModeClassification = failureMode;
            return result;
        }

        double phiP_n; //stored value for iteration
        FlexuralCompressionFiberPosition FlexuralCompressionFiberPosition; //stored value for iteration

        private double phiFactorDifferenceCalculation(double phi)
        {
            double P_nIter = phiP_n / phi;

            IStrainCompatibilityAnalysisResult nominalResult = this.GetNominalMomentResult(P_nIter, FlexuralCompressionFiberPosition);
            ConcreteCompressionStrengthResult result = new ConcreteCompressionStrengthResult(nominalResult, FlexuralCompressionFiberPosition, this.Section.Material.beta1);
            StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            FlexuralFailureModeClassification failureMode = f.GetFlexuralFailureMode(result.epsilon_t, result.epsilon_ty);
            double phiActual = f.Get_phiFlexureAndAxial(failureMode, ConfinementReinforcementType, result.epsilon_t, result.epsilon_ty);

            return phi - phiActual;
        }



        //Table 22.4.2.1
        public double GetMaximumForce()
        {
            double C;
            switch (CompressionMemberType)
            {
                case CompressionMemberType.NonPrestressedWithTies:
                    C = 0.8;
                    break;
                case CompressionMemberType.NonPrestressedWithSpirals:
                    C = 0.85;
                    break;
                case CompressionMemberType.PrestressedWithTies:
                    C = 0.8;
                    break;
                case CompressionMemberType.PrestressedWithSpirals:
                    C = 0.85;
                    break;
                //case CompressionMemberType.Composite:
                //    C = 0.85;
                //    break;
                default:
                    C = 0.8;
                    break;
            }

            double P_o = GetP_o();
            return C * P_o;

        }




    }
}
