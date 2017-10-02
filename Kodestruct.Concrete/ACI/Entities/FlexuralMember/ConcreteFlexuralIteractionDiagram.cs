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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14;
using System.IO;
using System.Text;

namespace Kodestruct.Concrete.ACI
{
    public abstract partial class ConcreteFlexuralSectionBase : ConcreteSectionLongitudinalReinforcedBase, IConcreteFlexuralMember
    {

        protected List<SectionAnalysisResult> GetInteractionResults(FlexuralCompressionFiberPosition CompressionFiberPosition, int NumberOfSteps=50)
        {

            List<SectionAnalysisResult> SectionResults = new List<SectionAnalysisResult>();

            currentCompressionFiberPosition = CompressionFiberPosition; //store this off because it will be necessary during iteration
            StrainHeight = GetStrainDistributionHeight(CompressionFiberPosition);//store this off because it will be necessary during iteration

            double StrainMin = CalculateMaximumSteelStrain(CompressionFiberPosition);
            double StrainMax = MaxConcreteStrain;

            double TotalStrainVariation = StrainMax - StrainMin;
            double strainStep = TotalStrainVariation / NumberOfSteps;

            int NumberOfStartEndPoints = 10;
            int TotalSteps = NumberOfSteps + 2 * NumberOfStartEndPoints;
            for (int i = 0; i < TotalSteps; i++)
            {

                //double SteelStrain = StrainMax -i * strainStep;
                double SteelStrain =GetSteelStrain(NumberOfSteps, i, StrainMax, StrainMin, NumberOfStartEndPoints);
                LinearStrainDistribution currentStrainDistribution = null;
                switch (CompressionFiberPosition)
                {
                    case FlexuralCompressionFiberPosition.Top:

                        currentStrainDistribution = new LinearStrainDistribution(StrainHeight, this.MaxConcreteStrain, SteelStrain);

                        break;
                    case FlexuralCompressionFiberPosition.Bottom:

                        currentStrainDistribution = new LinearStrainDistribution(StrainHeight, SteelStrain, this.MaxConcreteStrain);

                        break;
                    default:
                        throw new CompressionFiberPositionException();
                }

                if (i==66)
                {
                    
                }
                SectionAnalysisResult thisDistibutionResult = GetSectionResult(currentStrainDistribution, CompressionFiberPosition);

                
                #region Dubugging output
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("AxialForce = {0}", Math.Round(thisDistibutionResult.AxialForce / 1000.0, 0)));
                sb.AppendLine(string.Format("Moment = {0}", Math.Round(thisDistibutionResult.Moment / 12.0 / 1000.0, 0)));
                sb.AppendLine(string.Format("CForce = {0}", Math.Round(thisDistibutionResult.CForce / 1000.0, 0)));
                sb.AppendLine(string.Format("TForce = {0}", Math.Round(thisDistibutionResult.TForce / 1000.0, 0)));
                sb.AppendLine(string.Format("SteelStrain = {0}", thisDistibutionResult.StrainDistribution.BottomFiberStrain, 0));
                sb.AppendLine(string.Format("YNeutral = {0}", Math.Round(thisDistibutionResult.StrainDistribution.NeutralAxisTopDistance, 3)));

                #endregion

                SectionResults.Add(thisDistibutionResult);
            }


            return SectionResults;
               
        }



        private double GetSteelStrain(int numberOfSteps, int i, double strainMax, double strainMin, int numberOfStartEndPoints)
        {
            double StepCompression = strainMax / numberOfStartEndPoints;
            double StepTransition =  3.0* strainMax / numberOfSteps;
            double StepTension =( Math.Abs(strainMin)-4.0* strainMax) / numberOfStartEndPoints;
            double Strain = 0;

            if (i< numberOfStartEndPoints)
            {
                Strain = strainMax- i * StepCompression;
            }
            else if (i>= numberOfStartEndPoints && i<(numberOfSteps+ numberOfStartEndPoints))
            {
                double TotalStrainSeg1 = (numberOfStartEndPoints * StepCompression);
                Strain = strainMax-TotalStrainSeg1 - (i - numberOfStartEndPoints) * StepTransition;
            }
            else
            {
                double TotalStrainSeg1 = (numberOfStartEndPoints * StepCompression);
                double TotalStrainSeg2 = numberOfSteps*StepTransition;
                Strain = strainMax- TotalStrainSeg1 - TotalStrainSeg2 -(i- numberOfSteps-numberOfStartEndPoints)* StepTension;
            }
            return Strain;
        }
    }
}
