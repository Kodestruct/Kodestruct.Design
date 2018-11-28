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
using Kodestruct.Common.Section.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Steel.AISC.AISC360v10.AffectedMembers.Splices
{
    public class ColumnFlangeSplice
    {
        /// <summary>
        /// Column section
        /// </summary>
        public ISectionI Section { get; set; }
        /// <summary>
        /// Bolt gage
        /// </summary>
        public double g { get; set; }
        public double F_y { get; set; }
        public ColumnFlangeSplice(ISectionI Section,double F_y, double g)
        {
            this.Section = Section;
            this.g = g;
            this.F_y = F_y;
        }
        public double GetEffectiveFlangeForce(double P, double Mx, double My)
        {
            P = Math.Abs(P);
            Mx = Math.Abs(Mx);
            My = Math.Abs(My);

            double F_t = P / 2;
            double F_fx = Mx / Section.d;
            double phiM_py = 0.9 * F_y * Section.Z_y;
            double M_fy = My / 2.0;

            bool compressionBlockCalculationFeasible = true;
            double epsilon = 0;
            double T = 0;
            double b = Math.Min(Section.b_fBot, Section.b_fTop);

            if (M_fy<=3.0/8.0* phiM_py)
            {
                if (8.0*M_fy<3.0* phiM_py)
                {
                    epsilon = (1.0 / 4.0) * b * (1 - Math.Sqrt(1 - ((8.0 * M_fy) / (3 * phiM_py))));
                }
                else
                {
                    compressionBlockCalculationFeasible = false;
                }
            }
            else
            {
                
                double gamma = 1.0 + g / b;
                if (8.0 * M_fy /( 3.0 * phiM_py)*Math.Pow(1.0/gamma,2)<1.0)
                {
                   
                    epsilon = (1.0 / 4.0) * b * gamma * (1 - Math.Sqrt(1 - ((8.0*M_fy) / (3.0 * phiM_py)) * (1.0 / gamma) * (1.0 / gamma)));
                }
                else
                {
                    compressionBlockCalculationFeasible = false;
                }
            }
            if (compressionBlockCalculationFeasible == true)
            {
                T = 0.75 * 1.8 * F_y * Section.t_f * epsilon;
                T = T > M_fy / g ? M_fy / g : T;
            }
            else
            {
                T = M_fy / g;
            }
            double F_fy = 2.0 * T;
            double F = F_t + F_fx + F_fy;
            return F;
        }
    }
}
