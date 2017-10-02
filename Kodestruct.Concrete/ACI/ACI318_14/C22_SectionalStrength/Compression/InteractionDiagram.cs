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
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class ConcreteSectionCompression : ConcreteCompressionSectionBase
    {
        public List<PMPair> GetPMPairs(FlexuralCompressionFiberPosition CompressionFiberPosition, int NumberOfSteps = 50, bool IncludeResistanceFactor = true)
        {
            List<PMPair> Pairs = new List<PMPair>();
            List<SectionAnalysisResult> SectionResults = GetInteractionResults(CompressionFiberPosition, NumberOfSteps);

            foreach (var thisDistibutionResult in SectionResults)
            {
                IStrainCompatibilityAnalysisResult nominalResult = GetResult(thisDistibutionResult);
                ConcreteCompressionStrengthResult result = new ConcreteCompressionStrengthResult(nominalResult, CompressionFiberPosition, this.Section.Material.beta1);

                StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
                FlexuralFailureModeClassification failureMode = f.GetFlexuralFailureMode(result.epsilon_t, result.epsilon_ty);
                double phi;
                if (IncludeResistanceFactor == true)
                {
                    phi = f.Get_phiFlexureAndAxial(failureMode, ConfinementReinforcementType, result.epsilon_t, result.epsilon_ty);
                }
                else
                {
                    phi = 1.0;
                }

                double SignFactor = 1.0;
                if (CompressionFiberPosition == FlexuralCompressionFiberPosition.Bottom)
                {
                    SignFactor = -1.0;
                }
                double P = thisDistibutionResult.AxialForce * phi;
                double M = thisDistibutionResult.Moment * phi * SignFactor;
                PMPair thisPair = new PMPair(P, M);
                Pairs.Add(thisPair);
            }

            if (IncludeResistanceFactor == true)
            {
                List<PMPair> TruncatedPairs = TruncateInteractionDiagram(Pairs, CompressionFiberPosition);
                return TruncatedPairs;
            }
            else
            {
                return Pairs;
            }
        }

        private List<PMPair> TruncateInteractionDiagram(List<PMPair> Pairs, FlexuralCompressionFiberPosition CompressionFiberPosition)
        {
           
            double P_oUnreduced = GetMaximumForce();
            double P_o = P_oUnreduced * 0.65;
            double M_cutoff=0;
            var SortedPairs = Pairs.OrderBy(p => p.P).ToList();

            for (int i = 0; i < SortedPairs.Count()-1; i++)
            {
                if (SortedPairs[i].P<=P_o && SortedPairs[i+1].P>=P_o)
                {
                    double deltaP = SortedPairs[i + 1].P - SortedPairs[i].P;
                    double deltaM = SortedPairs[i + 1].M - SortedPairs[i].M;
                    double slope = deltaM / deltaP;
                    double incrementP = P_o- SortedPairs[i].P;
                    M_cutoff = SortedPairs[i].M + incrementP * slope;
                }
            }

            //FINAL DIAGRAM
            List<PMPair> TruncatedPairs = new List<PMPair>();
            TruncatedPairs.Add(GetPureTensionPoint(CompressionFiberPosition));

            var RemainingPairs = SortedPairs.Where(p => p.P < P_o);
            TruncatedPairs.AddRange(RemainingPairs);
            TruncatedPairs.Add(new PMPair(P_o, M_cutoff));
            TruncatedPairs.Add(new PMPair(P_o, 0));
            return TruncatedPairs;

        }

        private PMPair GetPureTensionPoint(FlexuralCompressionFiberPosition CompressionFiberPosition, bool IncludePhiFactor=true)
        {
            double StrainMin = CalculateMaximumSteelStrain(CompressionFiberPosition);
            double P_t = LongitudinalBars.Sum(b => b.Rebar.Area * b.Rebar.Material.GetStress(StrainMin));
            if (IncludePhiFactor == true)
            {
                return new PMPair( P_t * 0.9,0);
            }
            else
            {
                return new  PMPair( P_t, 0);
            }
        }
    }
}
