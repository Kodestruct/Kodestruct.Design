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
    /// <summary>
    ///  W, M, S or HP  Shapes or Tees cut  from these shapes. (If U is calculated per Case 2, the larger value is permitted to be used.)
    /// </summary>
    public class ShearLagCase7 : ShearLagFactorBase
    {

        int N;
        double bf;
        double d;
        double x_ob;
        double l;
    
        public ShearLagCase7(int NumberOfBoltLines, double Depth, double FlangeWidth,
            double EccentricityOfConnection, double LengthOfConnection )
            
        {
            base.Log = new CalcLog();
            this.N = NumberOfBoltLines;
            this.bf = FlangeWidth;
            this.d = Depth;
            x_ob = EccentricityOfConnection;
            l = LengthOfConnection;
        }


        /// <summary>
        /// Calculates shear lag factor per AISC Table D3.1 "Shear Lag Factors for Connections  to Tension Members".
        /// </summary>
        public override double GetShearLagFactor()
        {
            double U, U1, U2;
            if (N<3)
            {
                ShearLagCase2 Case2 = new ShearLagCase2(x_ob, l);
                U1 = Case2.GetShearLagFactor();
            }
            else if (N==3)
            {
                if (bf > 2 / 3 * d)
                {
                    U1 = 0.9;
                }
                else
                {
                    U1 = 0.85;
                } 
            }
            else
            {
                U1 = 0.7;
            }
            // If Case 2 information is applicable
            if (x_ob>0 && l > 0)
            {
                ShearLagCase2 Case2 = new ShearLagCase2(x_ob, l);
                U2 = Case2.GetShearLagFactor();
                U = Math.Max(U1, U2);
            }
            else
            {
                U = U1;
            }

            return U;
        }
    }
}
