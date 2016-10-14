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
    public class DoubleAngleConnection : AnalyticalElement
    {
        /// <summary>
        /// Weld strength for outstanding leg of back to back angles. Both legs included.
        /// </summary>
        /// <param name="phiR_nWeld">Total weld strength for a single vertical leg  </param>
        /// <param name="L_angle"> Vertical length (height) of angle </param>
        /// <param name="e">Width of leg, welded to support</param>
        /// <returns></returns>
        public double GetWeldStrengthForOutstandingLegsOfBacktoBackAngles(double phiR_nWeld, double L_angle, double e)
        {
            double phiR_n = 2 * (((phiR_nWeld) / (Math.Sqrt(1 + ((12.96 * e) / (Math.Pow(L_angle, 2))))))); //(AISCM p.10-11)
            return phiR_n;
        }
    }
}
