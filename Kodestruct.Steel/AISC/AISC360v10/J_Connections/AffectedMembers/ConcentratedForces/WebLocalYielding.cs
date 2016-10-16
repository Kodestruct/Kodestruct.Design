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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers.ConcentratedForces
{

    public static partial class FlangeOrWebWithConcentratedForces
    {
        /// <summary>
        /// Web Local Yielding strength
        /// </summary>
        /// <param name="d"> Depth of member</param>
        /// <param name="l_edge"> Edge distance</param>
        /// <param name="F_yw">Web yield strength</param>
        /// <param name="k">Distance from outer face of flange to the web toe of fillet </param>
        /// <param name="l_b">Length of bearing </param>
        /// <returns></returns>
        public static double GetWebLocalYieldingStrength(double d, double t_w, double l_edge, double F_yw, double k, double l_b )
        {
            double R_n=0.0;
            if (l_edge>=d)
            {
                R_n = F_yw * t_w * (5 * k + l_b); //(J10-2)
            }
            else
	        {
                R_n = F_yw * t_w * (2.5 * k + l_b); //(J10-3)
	        }
            double phiR_n = 1.0 * R_n;
            return phiR_n;
        }
    }
}
