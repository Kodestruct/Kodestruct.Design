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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.BasePlate
{
    public class BasePlateIShape : BasePlateTypeBase
    {

        public BasePlateIShape(double B_bp, double N_bp, double d_c, double b_f,
             double f_c, double F_y, double A_2)
            :base(B_bp,N_bp, f_c, F_y, A_2)

        {
            this.d_c = d_c;
            this.b_f = b_f;
            //this.P_u = P_u;
        }

        double d_c;
        double b_f;
        //double P_u;
       

        public override double GetLength(double P_u)
        {
            double m = Get_m();
            double n = Get_n();
           double  lambda_n_prime = Get_lambda_n_prime(P_u);

           List<double> ls = new List<double>
           {
               m,n,lambda_n_prime
           };
           var l_max = ls.Max();

           return l_max;
        }

        private double Get_lambda_n_prime(double P_u)
        {
            double phiP_p = GetphiP_p();
            double X=(((4.0*d_c*b_f) / (Math.Pow((d_c+b_f), 2))))*((P_u) / (phiP_p));
            double lambda1 = ((2.0 * Math.Sqrt(X)) / (1 + Math.Sqrt(1 - X)));
            double lambda2 = 1.0;
            double lambda = Math.Min(lambda1, lambda2);
            double lambda_n_prime = lambda * ((Math.Sqrt(d_c * b_f)) / (4.0));
            return lambda_n_prime;
        }

        public override double Get_m()
        {
            double m = ((N_bp - 0.95 * d_c) / (2.0));
            return m;
        }

        public override  double Get_n()
        {
            double n =((B_bp - 0.8 * b_f) / (2.0));
            return n;
        }
    }
}
