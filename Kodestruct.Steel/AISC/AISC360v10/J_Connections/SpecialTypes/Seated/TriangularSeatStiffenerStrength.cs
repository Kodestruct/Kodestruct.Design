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
    

        public double GetTriangularSeatStiffenerAxialStrength(double t_p)
        {
            double b_prime = Get_b_prime();
            double F_cr = GetF_cr(t_p);
            double phiN_n = 0.9 * F_cr * t_p * b_prime; //(15-11)
            return phiN_n;
        }

        private double GetF_cr(double t_p)
        {
            double lambda = Get_lamda(t_p);
            double Q = Get_Q(lambda);
            double F_cr = Q * F_y; //(15-14)
            return F_cr;
        }

        private double Get_Q(double lamda)
        {
            double Q;
            if (lamda<=0.7)
            {
                Q = 1;
            }
            else
            {
                if (lamda<=1.41)
                {
                    Q = 1.34 - 0.486 * lamda; //(15-15)
                }
                else
                {
                    Q = ((1.3) / (Math.Pow(lamda, 2))); //(15-16)
                }
            }
            return Q;
        }

        private double Get_lamda(double t_p)
        {
            double a_prime = Get_a_prime();
            double b_prime = Get_b_prime();
            double lamda = ((((b_prime) / (t_p))) * Math.Sqrt(F_y)) / (5 * Math.Sqrt(475 + 1120 * Math.Pow((((b_prime) / (a_prime))), 2))); //(15-17)
            return lamda;
        }

        public double GetTriangularSeatStiffenerShearStrength(double t_p)
        {
            double b_prime = Get_b_prime();
            double phiV_n = 1.0 * 0.6 * F_y * t_p * b_prime; //(15-7)
            return phiV_n;
        }

        public double GetTriangularSeatStiffenerMomentStrength(double t_p)
        {
            double b_prime = Get_b_prime();
            double F_cr = GetF_cr(t_p);
            double phiM_n = 0.9 * ((F_cr * t_p * Math.Pow(b_prime, 2)) / (4)); //(15-12)
            return phiM_n;
        }
    }
}
