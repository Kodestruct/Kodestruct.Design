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
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10 
{
    //Rectangular HSS
    public class ShearLagCase6 : ShearLagFactorBase
    {
        //overall width of rectangular HSS member
        double B;
        //overall height of rectangular HSS member
        double H;
        double l;

        bool IsSingleConcentricGussetPlate;
        public ShearLagCase6(bool IsSingleConcentricGussetPlate,
            double OverallWidthOfRectangularHSSMember,
            double OverallHeightOfRectangularHSSMember,
            double LengthOfConnection)
           
        {
            base.Log = new CalcLog();
            B = OverallWidthOfRectangularHSSMember;
            H = OverallHeightOfRectangularHSSMember;
            l = LengthOfConnection;
        }

        /// <summary>
        ///Calculates shear lag factor per AISC Table D3.1 "Shear Lag Factors for Connections  to Tension Members".
        /// </summary>
        public override double GetShearLagFactor()
        {
            double x_ob;
            if (IsSingleConcentricGussetPlate == true)
            {
                x_ob = (B * B + 2.0 * B * H) / (4 * (B + H));

            }
            else
            {
                x_ob = (B * B) / (4 * (B + H));
            }

            return 1 - x_ob / l;
        }
    }
}
