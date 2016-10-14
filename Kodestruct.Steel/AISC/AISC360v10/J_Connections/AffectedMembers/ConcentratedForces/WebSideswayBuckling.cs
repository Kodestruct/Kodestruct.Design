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
    // WebSideswayBuckling
    public static partial class FlangeOrWebWithConcentratedForces
    {
        /// <summary>
        /// Web sidesway buckling strength
        /// </summary>
        /// <param name="t_w">Web thickness</param>
        /// <param name="t_f">Flange thickness</param>
        /// <param name="h">Clear distance between flanges less the fillet or corner radius for rolled shapes; distance between adjacent lines of fasteners or the clear distance between flanges when welds are used for built-up shapes,</param>
        /// <param name="L_bFlange">Largest laterally unbraced length along either flange at the point of load, in.</param>
        /// <param name="b_f">Flange width</param>
        /// <param name="CompressionFlangeRestrained"></param>
        /// <param name="M_u">Design moment</param>
        /// <param name="M_y">Yield moment</param>
        /// <returns></returns>
        public static double GetWebSideswayBucklingStrength(double t_w, double t_f, double h,
            double L_bFlange, double b_f, bool CompressionFlangeRestrained, double M_u, double M_y)
        {
            double C_r = GetC_r(M_u, M_y);
            double R_n = 0.0;
            if (CompressionFlangeRestrained ==true)
            {
                if((h/t_w)/(L_bFlange/b_f)<=2.3)
                {
                    //(J10-6)
                    R_n = ((C_r * Math.Pow(t_w, 3) * t_f) / (Math.Pow(h, 2))) * (1 + 0.4 * Math.Pow((((((h) / (t_w))) / (((L_bFlange) / (b_f))))), 3));
                }
                else
	            {
                    R_n = double.PositiveInfinity;
	            }
            }
            else
            {
                if ((h / t_w) / (L_bFlange / b_f) <= 1.7)
                {
                    //(J10-7)
                    R_n = ((C_r * Math.Pow(t_w, 3) * t_f) / (Math.Pow(h, 2))) * (0.4 * Math.Pow((((((h) / (t_w))) / (((L_bFlange) / (b_f))))), 3));
                }
                else
                {
                    R_n = double.PositiveInfinity;
                }
                 
            }
            return 0.85 * R_n;
        }

        private static double GetC_r(double M_u, double M_y)
        {
            if (M_u<M_y)
            {
                return 960000.0;
            }
            else
            {
                return 480000.0;
            }

        }
    }
}
