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
    /// <summary>
    /// Single and double angles (If Uis calculated per Case 2, the larger value is permitted to be used.)
    /// </summary>
    public class ShearLagCase8 : ShearLagFactorBase
    {
        int N;
        double x_ob;
        double l;

        public ShearLagCase8(int NumberOfBoltLines, double EccentricityOfConnection, double LengthOfConnection)
         
        {
            base.Log = new CalcLog();
            this.N = NumberOfBoltLines;
            x_ob = EccentricityOfConnection;
            l = LengthOfConnection;
        }

        /// <summary>
        /// Calculates shear lag factor per AISC Table D3.1 "Shear Lag Factors for Connections  to Tension Members".
        /// </summary>
        public override double GetShearLagFactor()
        {
            double U;
            if (N < 3)
            {
                ShearLagCase2 Case2 = new ShearLagCase2(x_ob, l);
                U = Case2.GetShearLagFactor();
            }
            else if (N == 3)
            {
                U = 0.6;
            }
            else
            {
                U = 0.8;
            }
            // If Case 2 information is applicable
            if (x_ob > 0 && l > 0)
            {
                ShearLagCase2 Case2 = new ShearLagCase2(x_ob, l);
                double U_case2 = Case2.GetShearLagFactor();
                U = Math.Min(U, U_case2);
            }

            return U;
        }
    }
}
