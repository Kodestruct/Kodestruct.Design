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

    // Round HSS with a single concentric 
    // gusset plate


    public class ShearLagCase5 : ShearLagFactorBase
    {
        //diameter  HSS member
        double D;
        double l;

        bool IsSingleConcentricGussetPlate;
        public ShearLagCase5(double Diameter, 
            double LengthOfConnection)
        {
            base.Log = new CalcLog();
            D=Diameter;
            l = LengthOfConnection;
        }

        /// <summary>
        ///Calculates shear lag factor per AISC Table D3.1 "Shear Lag Factors for Connections  to Tension Members".
        /// </summary>
        public override double GetShearLagFactor()
        {
            double x_ob = D/Math.PI;

            if (l>=1.3*D)
            {
                return 1.0;
            }

            return 1 - x_ob / l;
        }
    }
}
