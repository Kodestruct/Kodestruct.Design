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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers.ConcentratedForces
{
    

    public static  partial class FlangeOrWebWithConcentratedForces
    {
        /// <summary>
        /// Flange Local Bending strength
        /// </summary>
        /// <param name="F_yf">Specified minimum yield stress of the flange (ksi)</param>
        /// <param name="t_f">thickness of the loaded flange (in.)</param>
        /// /// <param name="l_edge">Edge distance</param>
        /// <returns></returns>
        public static double GetFlangeLocalBendingStrength(double F_yf, double t_f, double l_edge)
        {
            double R_n = 6.25 * F_yf * Math.Pow(t_f, 2); //(J10-1)

            //When the concentrated force to be resisted is applied at a distance from the member end 
            //that is less than 10t, 
            //Rn shall be reduced by 50%

            if (l_edge < 10.0 * t_f)
            {
                R_n = 0.5 * R_n;
            }
            double phiR_n = 0.9 * R_n;
            return phiR_n;
        }
    }
}
