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

namespace Kodestruct.Concrete.ACI
{
    public class LinearStrainDistribution //: ConcreteStrainDistribution
    {
        public double Height { get; set; }
        public double TopFiberStrain { get; set; }
        public double BottomFiberStrain { get; set; }

        public double NeutralAxisTopDistance
        {
            get { return GetNeutralAxisDistance(); }
        }


        public double StrainSlope
        {
            get { return (TopFiberStrain) / NeutralAxisTopDistance; ; }

        }

        public LinearStrainDistribution(double StrainHeight, double TopFiberStrain, double BottomFiberStrain)
        {
            this.Height = StrainHeight;
            this.TopFiberStrain = TopFiberStrain;
            this.BottomFiberStrain = BottomFiberStrain;
        }

        public double GetStrainAtPointOffsetFromTop(double PointTopDistance)
        {
            return TopFiberStrain - StrainSlope * PointTopDistance;
        }

        public double GetStrainAtPointOffsetFromTBottom(double PointBottomDistance)
        {
            double PointTopDistance = this.Height - PointBottomDistance;
            return TopFiberStrain - StrainSlope * PointTopDistance;
        }

        private double GetNeutralAxisDistance()
        {
            double neutralAxisTopDistance = 0;

            //if (TopFiberStrain * BottomFiberStrain >0)
            //{
            //    throw new Exception("Failed to locate neutral axis top and bottom strains must have different signs");
            //}
            //else
            //{
                
                if (TopFiberStrain != BottomFiberStrain)
                {
                    neutralAxisTopDistance = (TopFiberStrain * this.Height) / (Math.Abs(TopFiberStrain) + Math.Abs(BottomFiberStrain));
                }
                else
                {
                    neutralAxisTopDistance = double.PositiveInfinity;
                } 
            //}


            return Math.Abs(neutralAxisTopDistance);
        }
        public LinearStrainDistribution Clone()
        {
            return new LinearStrainDistribution(this.Height, this.TopFiberStrain, this.BottomFiberStrain);
        }
    }
}
