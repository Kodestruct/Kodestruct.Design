using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI;

namespace Kodestruct.Concrete.ACI318_14
{
    public class ConcreteSectionCompression : ConcreteCompressionSectionBase
    {
        public ConcreteSectionCompression(IConcreteSection Section,
            List<RebarPoint> LongitudinalBars, CompressionMemberType CompressionMemberType, ICalcLog log)
            : base(Section, LongitudinalBars, log)
        {
            this.CompressionMemberType = CompressionMemberType;
        }

        public ConcreteSectionCompression(ConcreteSectionFlexure FlexuralSection, CompressionMemberType CompressionMemberType, ICalcLog log)
            : base(FlexuralSection.Section, FlexuralSection.LongitudinalBars, log)
        {
            this.CompressionMemberType = CompressionMemberType;
        }


        private CompressionMemberType compressionMemberType;

        public CompressionMemberType CompressionMemberType
        {
            get { return compressionMemberType; }
            set { compressionMemberType = value; }
        }

        public  ConcreteCompressionStrengthResult GetDesignMomentWithCompressionStrength( double P_u,
            FlexuralCompressionFiberPosition FlexuralCompressionFiberPosition,
            ConfinementReinforcementType ConfinementReinforcementType, bool CapAxialForceAtMaximum = false)
        {
            double P_o = GetMaximumForce();
            StrengthReductionFactorFactory ff = new StrengthReductionFactorFactory();
            double phiAxial = ff.Get_phiFlexureAndAxial(FlexuralFailureModeClassification.CompressionControlled, ConfinementReinforcementType, 0, 0);
            double phiP_n = phiAxial * P_o;

            if (P_u > phiP_n)
            {
                if (CapAxialForceAtMaximum == false)
                {
                    throw new Exception("Axial forces exceeds maximum axial force.");
                }
                else
                {
                    P_u = phiP_n;
                }
                
            }
            IStrainCompatibilityAnalysisResult nominalResult = this.GetNominalMomentResult(P_u,FlexuralCompressionFiberPosition);
            ConcreteCompressionStrengthResult result = new ConcreteCompressionStrengthResult(nominalResult, FlexuralCompressionFiberPosition, this.Section.Material.beta1);
            StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            FlexuralFailureModeClassification failureMode = f.GetFlexuralFailureMode(result.epsilon_t, result.epsilon_ty);
            double phi = f.Get_phiFlexureAndAxial(failureMode, ConfinementReinforcementType, result.epsilon_t, result.epsilon_ty);
            double phiM_n = phi * nominalResult.Moment;
            result.phiM_n = phiM_n; result.FlexuralFailureModeClassification = failureMode;
            return result;
        }



        //Table 22.4.2.1
        protected double GetMaximumForce()
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
                case CompressionMemberType.Composite:
                    C = 0.85;
                    break;
                default:
                    C = 0.8;
                    break;
            }

            double P_o = GetP_o();
            return C * P_o;

        }




    }
}
