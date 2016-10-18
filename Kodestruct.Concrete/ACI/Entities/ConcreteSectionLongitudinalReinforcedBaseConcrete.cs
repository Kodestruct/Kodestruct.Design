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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Interfaces;


namespace Kodestruct.Concrete.ACI
{
    public abstract partial class ConcreteSectionLongitudinalReinforcedBase : ConcreteSectionBase, IConcreteSectionWithLongitudinalRebar
    {

        protected double GetCompressionBlockDepth(LinearStrainDistribution StrainDistribution, FlexuralCompressionFiberPosition CompressionFiberPosition)
        {
            double c = GetDistanceToNeutralAxis(StrainDistribution, CompressionFiberPosition);
            return this.GetCompressionBlockDepth(c);
        }

        protected double GetCompressionBlockDepth(double DistanceToNeutralAxis)
        {
            double c = DistanceToNeutralAxis;
            double beta1 = Section.Material.beta1;
            double a = c * beta1;
            return a;
        }

        protected double GetCompressionBlockDepth(double RebarResultant, FlexuralCompressionFiberPosition CompressionFiberPosition)
        {

            //double fc = Section.Material.SpecifiedCompressiveStrength;
            double RequiredConcreteArea = 0;
            IMoveableSection compressedSection = null;

            double WhitneyBlockStress = GetWhitneyBlockStress();
            RequiredConcreteArea = RebarResultant / (WhitneyBlockStress);
            if (RequiredConcreteArea > this.Section.SliceableShape.A)
            {
                //TODO: go directly to strain compatibility solution
            }
            switch (CompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    
                    compressedSection = this.Section.SliceableShape.GetTopSliceOfArea(RequiredConcreteArea);
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    compressedSection = this.Section.SliceableShape.GetBottomSliceOfArea(RequiredConcreteArea);
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            //Get corresponding strain
            double a = compressedSection.YMax - compressedSection.YMin;


            return a;
        }

        protected virtual double GetWhitneyBlockStress()
        {
            double fc = Section.Material.SpecifiedCompressiveStrength;
            return 0.85 * fc;

        }

        protected double GetDistanceToNeutralAxis(LinearStrainDistribution StrainDistribution, FlexuralCompressionFiberPosition CompressionFiberPosition)
        {
            double c = StrainDistribution.NeutralAxisTopDistance;
             switch (CompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    return c;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    return StrainDistribution.Height-c;
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }
            return c;
        }

        protected ForceMomentContribution GetConcreteWhitneyForceResultant(LinearStrainDistribution StrainDistribution, FlexuralCompressionFiberPosition compFiberPosition)
        {
            ForceMomentContribution ConcreteCompressionContribution = new ForceMomentContribution();

            // CalculateBeta and compression block height
            double c = GetDistanceToNeutralAxis(StrainDistribution, compFiberPosition);
            double h = Section.SliceableShape.YMax - Section.SliceableShape.YMin;
            double a;
            
            if (c == double.PositiveInfinity)
            {
                a = h;
            }
            else
            {
                a = GetCompressionBlockDepth(c);
                if (a>h)
                {
                    a = h;
                }
            }


            double CentroidYToTopEdge = (Section.SliceableShape.YMax-Section.SliceableShape.YMin)-Section.SliceableShape.y_Bar;
            double neutralAxisToBottomOfCompressedShapeOffset = CentroidYToTopEdge - a;
            IMoveableSection compressedPortion = null;
            ISliceableSection sec = this.Section.SliceableShape as ISliceableSection;
            if (sec != null)
            {
                compressedPortion = GetCompressedConcreteSection(StrainDistribution,compFiberPosition, a);
            }
            double A = compressedPortion.A;
            double fc = Section.Material.SpecifiedCompressiveStrength;

            double WhitneyBlockStress = GetWhitneyBlockStress();
            double ConcreteResultantForce = A * WhitneyBlockStress;
            ConcreteCompressionContribution.Force = ConcreteResultantForce;


            double concreteForceCentroidDistance = compressedPortion.GetElasticCentroidCoordinate().Y;
            ConcreteCompressionContribution.Moment = concreteForceCentroidDistance * ConcreteResultantForce;

            return ConcreteCompressionContribution;
        }

        ForceMomentContribution GetConcreteParabolicStressForceResultant(LinearStrainDistribution StrainDistribution)
        {
            throw new NotImplementedException();
        }   
    }
}
