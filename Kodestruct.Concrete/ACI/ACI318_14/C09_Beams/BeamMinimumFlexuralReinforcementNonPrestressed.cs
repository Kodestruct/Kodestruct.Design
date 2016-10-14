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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI;


namespace Kodestruct.Concrete.ACI318_14
{
    public partial class ReinforcedConcreteBeamNonprestressed
    {


        
        public double GetMinimumFlexuralReinforcement(double d,double f_y,  double RatioA_sProvidedToRequired=1.0 )
        {
            //ConcreteSectionFlexure FlexuralSection


            //Section 9.6.1.2 
            double b_w = ConcreteSection.b_w;
            double f_cPrime = ConcreteSection.Material.SpecifiedCompressiveStrength;
            double A_sMin1 = ((3*Math.Sqrt(f_cPrime)) / (f_y))*b_w*d;
            double A_sMin2 =((200*b_w*d) / (f_y));

            double A_sMin = Math.Max(A_sMin1, A_sMin2);
            if (RatioA_sProvidedToRequired>=1.333)
            {
                //9.6.1.3
                A_sMin = 0; 
            }
            return A_sMin;
        }

        //private double FindLowest_f_y(FlexuralCompressionFiberPosition CompressionFiber)
        //{
        //    double YCutoff  = (FlexuralSection.Section.SliceableShape.YMax + FlexuralSection.Section.SliceableShape.YMin)/2.0;
        //    List<RebarPoint> tensionBars;
        //            if (CompressionFiber == FlexuralCompressionFiberPosition.Top)
        //            {
        //            tensionBars = FlexuralSection.GetFilteredBars(YCutoff, 
        //            Kodestruct.Concrete.ACI.ConcreteSectionLongitudinalReinforcedBase.BarCoordinateFilter.Y,
        //            ACI.ConcreteSectionLongitudinalReinforcedBase.BarCoordinateLimitFilterType.Maximum);
        //            }
        //            else
        //            {
        //            tensionBars = FlexuralSection.GetFilteredBars(YCutoff,
        //            Kodestruct.Concrete.ACI.ConcreteSectionLongitudinalReinforcedBase.BarCoordinateFilter.Y,
        //            ACI.ConcreteSectionLongitudinalReinforcedBase.BarCoordinateLimitFilterType.Minimum);
        //            }

        //            double f_yMin = tensionBars.Min(b => b.Rebar.Material.YieldStress);

        //     return f_yMin;
        //}
    }
}
