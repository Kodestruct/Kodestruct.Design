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
        /// Web panel zone shear
        /// </summary>
        /// <param name="t_w"></param>
        /// <param name="t_cf">Column </param>
        /// <param name="b_cf">Thickness of column flange, in.</param>
        /// <param name="d_b">Depth of beam, in.</param>
        /// <param name="d_c">Depth of column, in.</param>
        /// <param name="F_y">Specified minimum yield stress of the column web</param>
        /// <param name="P_u">Ultimate compression load</param>
        /// <param name="A_g">Gross area of column</param>
        /// <param name="PanelDeformationConsideredInAnalysis"> Defines if frame stability, including plastic panel-zone deformation, is considered in the analysis. </param>
        /// <returns></returns>
        public static double WebPanelZoneShear(double t_w, double t_cf,
            double b_cf, double d_b, double d_c, double F_y,
            double P_u, double A_g, bool PanelDeformationConsideredInAnalysis)
        {
            double R_n = 0.0;
            double P_y = A_g * F_y;

            if (PanelDeformationConsideredInAnalysis == false)
            {
                if (P_u<=0.4*P_y)
                {
                    //(J10-9)
                    R_n = 0.6 * F_y * d_c * t_w;
                }
                else
                {
                    //(J10-10)
                    R_n = 0.6 * F_y * d_c * t_w * (1.4 - ((P_u) / (P_y)));
                }
            }
            else
            {
                if (P_u <= 0.4 * P_y)
                {
                    //(J10-11)
                    R_n = 0.6 * F_y * d_c * t_w * (1 + ((3 * b_cf * Math.Pow(t_cf, 2)) / (d_b * d_c * t_w)));
                }
                else
                {
                    //(J10-12)
                    R_n = 0.6 * F_y * d_c * t_w * (1 + ((3 * b_cf * Math.Pow(t_cf, 2)) / (d_b * d_c * t_w))) * (1.9 - ((1.2 * P_u) / (P_y)));
                }
            }
            return 0.9 * R_n;
        }
    }
}
