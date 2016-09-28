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
using Kodestruct.Common.Entities;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI318_14
{
    /// <summary>
    ///  This class encpsulates sectional shear provisions per ACI.
    /// </summary>
    public partial class ConcreteSectionOneWayShearNonPrestressed : AnalyticalElement
    {

        public double GetConcreteShearStrength(double N_u, double rho_w, double M_u, double V_u)
        {

            double h = Section.SliceableShape.YMax - Section.SliceableShape.YMin;
            this.A_g = Section.SliceableShape.A;
            this.N_u = N_u;
            this.rho_w = rho_w;
            double V_c;
            double f_c = Section.Material.SpecifiedCompressiveStrength;

            double lambda = Section.Material.lambda;

            if (N_u == 0)
            {
                if (rho_w == 0  || M_u == 0 || V_u == 0)
                {
                    V_c = 2 * lambda * Section.Material.Sqrt_f_c_prime * b_w * d; // (22.5.5.1)
                }
                else
                {
                    //use detailed formula
                    //Table 22.5.5.1
                    double V_c_a = (1.9 * lambda * Section.Material.Sqrt_f_c_prime + 2500.0 * rho_w * ((V_u * d) / (M_u))) * b_w * d;
                    double V_c_b = (1.9 * lambda * Section.Material.Sqrt_f_c_prime + 2500.0 * rho_w) * b_w * d;
                    double V_c_c = 3.5 * lambda * Section.Material.Sqrt_f_c_prime * b_w * d;
                    List<double> V_cList = new List<double>() { V_c_a, V_c_b, V_c_c };
                    V_c = V_cList.Min();
                }

            }
            else
            {
                if (N_u > 0) //compression
                {
                    if (rho_w == 0 || h == 0)
                    {
                        //Use simplified formula
                        V_c = 2.0 * (1 + ((N_u) / (2000.0 * A_g))) * lambda * Section.Material.Sqrt_f_c_prime * b_w * d;
                    }
                    else
                    {
                        //Table 22.5.6.1

                        double V_c_b = 3.5 * lambda * Section.Material.Sqrt_f_c_prime * b_w * d * Math.Sqrt(1 + ((N_u) / (500.0 * A_g)));

                        if (M_u - N_u * (((4.0 * h - d) / (8.0))) <= 0)
                        {
                            V_c = V_c_b;
                        }
                        else
                        {
                            double V_c_a = (1.9 * lambda * Section.Material.Sqrt_f_c_prime + 2500.0 * rho_w * ((V_u * d) / (M_u - N_u * (((4.0 * h - d) / (8.0)))))) * b_w * d;
                            V_c = Math.Min(V_c_a, V_c_b);
                        }
                    }
                }
                else //tension 
                {
                    V_c = 2.0 * (1.0 + ((N_u) / (500.0 * A_g))) * lambda * Section.Material.Sqrt_f_c_prime * b_w * d;  //(22.5.7.1)
                }
            }

            V_c = V_c < 0 ? 0 : V_c;
            StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            double phi = f.Get_phi_ShearReinforced();
            return phi * V_c;
        }


    }
}
