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

namespace Kodestruct.Analysis.Section
{
    public partial class SectionStressAnalysis
    {
        /// <summary>
        /// Pure torsion stress in open cross section
        /// </summary>
        /// <param name="G">Shear modulus of elasticity</param>
        /// <param name="t_el">Thickness of element</param>
        /// <param name="theta_1der">First derivative of angle of rotation with respect to z</param>
        /// <returns></returns>
        public double GetPureTorsionStressOpenSection(double G, double t_el, double theta_1der)
        {
            double tau_t = G * t_el * theta_1der; //AISC Design Guide 9 Equation(4.1)
            return tau_t;
        }

        public double GetPureTorsionStressForClosedSection(ISection section, double T_u)
        {
            double tau;
            double b, h, t,J,R;
            if (section is ISolidShape)
            {
                if (section is ISectionRectangular)
                {
                    ISectionRectangular sr = section as ISectionRectangular;
                    J = sr.J;
                    R = Math.Min(sr.B,sr.H);
                }
                else if (section is ISectionRound)
                {
                    ISectionRound srou = section as ISectionRound;
                    J = srou.J;
                    R = srou.D / 2.0;
                }
                else
                {
                    throw new Exception("Section type is not applicable for closed section analysis");
                }
                tau = T_u * R / J;
            }
            else if(section is ISectionHollow)
            {
                if (section is ISectionTube || section is ISectionBox)
                {


                    if (section is ISectionBox)
                    {
                        ISectionBox sb = section as ISectionBox;
                        b = sb.B;
                        h = sb.H;
                        t = Math.Min(sb.t_f, sb.t_w);
                    }
                    else if (section is ISectionTube)
                    {
                        ISectionTube sT = section as ISectionTube;
                        b = sT.B;
                        h = sT.H;
                        t = sT.t_des;
                    }

                    else
                    {
                        throw new Exception("Section type is not applicable for closed section analysis");
                    }
                   tau =  GetBoxShearStress(T_u, b, h,t);
                }
                else if (section is ISectionPipe)
                {
                    ISectionPipe pipe = section as ISectionPipe;
                    R = pipe.D / 2.0;
                    J = pipe.J;
                    tau= GetPipeStress(T_u, R, J);
                }
                else
                {
                    throw new Exception("Section type is not applicable for closed section analysis");
                }
            }
            else
            {
                throw new Exception("Section type is not applicable for closed section analysis");
            }
            return tau;
        }

        private double GetPipeStress(double T_u, double R, double J)
        {
            return T_u * R * J;
        }

        private double GetBoxShearStress(double T_u, double b, double h, double t)
        {

            if( Math.Min(b,h)/t < 10.0)
            {
                throw new Exception("Closed-Section analysis is only applicable to sctions with b/t ratios of greater than  10");
            }
            return T_u / (2.0 * b * h * t); //DG Table 4.1
        }
 
    }
}
