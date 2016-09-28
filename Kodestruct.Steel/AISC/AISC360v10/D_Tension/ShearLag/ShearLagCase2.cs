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

// All tension members, except plates 
// and HSS, where the tension load is trans-mitted to some but not all of the cross-sectional elements by fasteners or longitu-dinal welds or by longitudinal welds in 
// combination with transverse welds. (Alternatively, for W, M, S and HP, Case 7 may
// be used. For angles, Case 8 may be used.)
    public class ShearLagCase2 : ShearLagFactorBase
    {
        double x_ob;
        double l;
        public ShearLagCase2(double EccentricityOfConnection, double LengthOfConnection)
            
        {
               base.Log = new CalcLog();
               x_ob= EccentricityOfConnection;
               l = LengthOfConnection;
        }
        /// <summary>
        ///Calculates shear lag factor per AISC Table D3.1 "Shear Lag Factors for Connections  to Tension Members".
        /// </summary>
        public override double GetShearLagFactor()
        {
            return 1.0-x_ob/l;
        }
    }
}
