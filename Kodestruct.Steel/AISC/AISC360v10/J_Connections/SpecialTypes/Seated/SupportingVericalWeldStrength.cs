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
using Kodestruct.Common.Entities;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public partial class SeatedConnection : AnalyticalElement
    {
        /// <summary>
        /// Weld strength for outstanding leg of back to back angles. Both legs included.
        /// </summary>
        /// <param name="phiR_nWeld">Total weld strength for a single vertical leg  </param>
        /// <param name="L_vSeat"> Vertical length (height) of seat </param>
        /// <param name="e1">Eccentricity from load to face of support</param>
        /// <returns>phiR_n - weld strength</returns>
        public double GetWeldStrengthForVerticalLegsOfSeat(double phiR_nWeld, double L_vSeat, double e_1)
        {

            double phiR_n = 2 * ((phiR_nWeld) / (Math.Sqrt(1 + ((20.25 * e_1) / (Math.Pow(L_vSeat, 2))))));
            return phiR_n;
        }
    }
}
