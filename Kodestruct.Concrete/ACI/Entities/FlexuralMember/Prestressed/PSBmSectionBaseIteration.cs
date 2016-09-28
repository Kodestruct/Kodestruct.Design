#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;

 

namespace Kodestruct.Concrete.ACI
{
    public abstract partial class PrestressedBeamSectionBase : ConcreteFlexuralSectionBase, IConcreteFlexuralMember
    {
        protected override LinearStrainDistribution GetInitialStrainEstimate(FlexuralCompressionFiberPosition CompressionFiberPosition)
        {
            currentCompressionFiberPosition = CompressionFiberPosition; //store this off for getting iteration limits
            double StrainDistributionHeight = GetStrainDistributionHeight(CompressionFiberPosition);
            double c = StrainDistributionHeight * 0.6; 
            //this is AASHTO criteria for when it's OK to use assumption that
            //mild rebar yields, beyond that need to use strain compatibility
            double epsilon_c = StrainUltimateConcrete.Value;
            double epsilon_s = epsilon_c - (epsilon_c / c) * StrainDistributionHeight;

            LinearStrainDistribution strainDistribution = null;
            switch (CompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    strainDistribution = new LinearStrainDistribution(StrainDistributionHeight, epsilon_c, epsilon_s);
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    strainDistribution = new LinearStrainDistribution(StrainDistributionHeight, epsilon_s, epsilon_c);
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            return strainDistribution;

        }

        FlexuralCompressionFiberPosition currentCompressionFiberPosition = FlexuralCompressionFiberPosition.Top;

        protected override TCIterationBound GetSolutionBoundaries(SectionAnalysisResult result, 
            LinearStrainDistribution ApproximationStrainDistribution)
            {
                double prestressForce = this.GetPrestressForceEffective();
                if (prestressForce == 0)
                {
                    //TODO: add check for prstressing if needed
                    //throw new NotPrestressedBeamException();
                }
            //prestressed beam strain iteration is different (from regular RC) from regular concrete because we are assuming that
            //the maximum depth of compression zone is 0.6d. Therefore this determines the start of iterations

                TCIterationBound bound = new TCIterationBound();
                switch (currentCompressionFiberPosition)
                {
                    case FlexuralCompressionFiberPosition.Top:
                        bound.MinStrain = ApproximationStrainDistribution.BottomFiberStrain;
                        break;
                    case FlexuralCompressionFiberPosition.Bottom:
                        bound.MinStrain = ApproximationStrainDistribution.TopFiberStrain;
                        break;
                    default:
                        throw new CompressionFiberPositionException();
                    
                }
                bound.MaxStrain = CalculateMaximumSteelStrain(currentCompressionFiberPosition);
                return bound;
            }

    }
}
