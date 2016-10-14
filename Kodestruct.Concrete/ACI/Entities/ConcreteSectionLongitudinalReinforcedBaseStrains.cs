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
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.Section.Interfaces;



namespace Kodestruct.Concrete.ACI
{
    public abstract partial class ConcreteSectionLongitudinalReinforcedBase : ConcreteSectionBase, IConcreteSectionWithLongitudinalRebar
    {


        public virtual double MaxConcreteStrain
        {
            get
            {
                return
                    StrainUltimateConcrete.Value;
            }

        }
        protected double GetStrainDistributionHeight(FlexuralCompressionFiberPosition CompressionFiberPosition)
        {
            double StrainDistributionHeight = 0.0;
            double YMax = Section.SliceableShape.YMax;
            double YMin = Section.SliceableShape.YMin;
            double XMax = Section.SliceableShape.XMax;
            double XMin = Section.SliceableShape.XMin;

            if (CompressionFiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                var LowestPointY = LongitudinalBars.Min(yVal => yVal.Coordinate.Y);
                var PointsAtLowestY = LongitudinalBars.Where(point => point.Coordinate.Y == LowestPointY).Select(point => point);
                StrainDistributionHeight = Math.Abs(YMax - LowestPointY);
            }
            else
            {
                var HighestPointY = LongitudinalBars.Max(yVal => yVal.Coordinate.Y);
                var PointsAtHighestY = LongitudinalBars.Where(point => point.Coordinate.Y == HighestPointY).Select(point => point);
                StrainDistributionHeight = Math.Abs(YMin - HighestPointY);
            }
            return StrainDistributionHeight;
        }

        protected virtual LinearStrainDistribution GetStrainDistributionBasedOn_a(double a, FlexuralCompressionFiberPosition CompressionFiberPosition)
        {

            double c = a / Section.Material.beta1;
            double StrainDistributionHeight = GetStrainDistributionHeight(CompressionFiberPosition);

            double epsilon_c = this.MaxConcreteStrain;
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

    }


}
