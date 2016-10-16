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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10 
{
    //Plates where the tension load is 
    //transmitted by longitudinal welds only
    public class ShearLagCase4 : ShearLagFactorBase
    {
        double l;
        double w;

        public ShearLagCase4(double PlateWidth, double LengthOfConnection)
            
        {
            base.Log = new CalcLog();
            l = LengthOfConnection;
            w = PlateWidth;
        }

       /// <summary>
        ///Calculates shear lag factor per AISC Table D3.1 "Shear Lag Factors for Connections  to Tension Members".
       /// </summary>
        public override double GetShearLagFactor()
        {
            if (l>=2*w)
            {
                return 1.0; 
            }
            else if (l<2*w && l>=1.5*w)
            {
                return 0.87;
            }
            else
            {
                return 0.75;
            }
        }
    }
}
