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

    public static partial class FlangeOrWebWithConcentratedForces
    {
        /// <summary>
        /// Web local crippling strength
        /// </summary>
        /// <param name="t_w">Web thickness</param>
        /// <param name="t_f">Flange thickness</param>
        /// <param name="d">Full nominal depth of the section</param>
        /// <param name="l_b">Bearing length</param>
        /// <param name="l_edge">Edge distance</param>
        /// <param name="F_yw">Web yield strength</param>
        /// <returns></returns>
        public static double GetWebLocalCripplingStrength(double t_w, double t_f, double d, double l_b, double l_edge,
            double F_yw)
        {
            double R_n = 0.0;
            double E = 29000;
            if (l_edge >= d/2)
            {
                //(J10-4)
                R_n = 0.8 * Math.Pow(t_w, 2) * (1 + 3 * (((l_b) / (d))) * Math.Pow((((t_w) / (t_f))), 1.5)) * Math.Sqrt(((E * F_yw * t_f) / (t_w)));
            }
            else
            {
                if (((l_b) / (d))<=0.2)
                {
                    //(J10-5a)
                    R_n = 0.4 * Math.Pow(t_w, 2) * (1 + 3 * (((l_b) / (d))) * Math.Pow((((t_w) / (t_f))), 1.5)) * Math.Sqrt(((E * F_yw * t_f) / (t_w)));
                }
                else
                {
                    //(J10-5b)
                    R_n=0.4*Math.Pow(t_w, 2)*(1+(((4*l_b) / (d))-0.2)*Math.Pow((((t_w) / (t_f))), 15))*Math.Sqrt(((E*F_yw*t_f) / (t_w)));
                }
            }
            return 0.75 * R_n;
        }
    }
}
