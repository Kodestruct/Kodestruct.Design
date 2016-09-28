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

namespace Kodestruct.Analysis.Torsion
{
    public class TorsionalFunctionCase10: TorsionalFunctionBase
    {
        public TorsionalFunctionCase10(double G, double J, double L, double z, double a,
            double t, double alpha)
            :base( G,  J,  L,  z,  a)
        {
            this.t = t;
            this.alpha = alpha;
        }

        protected override double Get_c_1()
        {
            double c_1 = ((t) / (G * J));
            return c_1;
        }

        protected override double Get_c_2()
        {
            double c_2 = Math.Tanh(((L) / (a))) * (((alpha * L) / (a)) - Math.Sinh(((alpha * L) / (a)))) + Math.Cosh(((alpha * L) / (a)));
            return c_2;
        }

        protected override double Get_c_3()
        {
            double c_3 = Math.Tanh(((L) / (a))) * Math.Sinh(((alpha * L) / (a))) - Math.Cosh(((alpha * L) / (a))) - (((alpha * L) / (a))) * Math.Tanh(((L) / (a))) + 1 + ((Math.Pow(alpha, 2) * Math.Pow(L, 2)) / (2 * Math.Pow(a, 2)));

            return c_3;
        }

        protected override double Get_c_4()
        {
            double c_4 = (Math.Sinh(((alpha * L) / (a))) - ((alpha * L) / (a))) * Math.Tanh(((L) / (a)));
            return c_4;
        }
        protected override double Get_c_5()
        {
            double c_5 = Math.Sinh (((alpha * L) / (a))) - ((alpha * L) / (a));
            return c_5;
        }

        public override double Get_theta()
        {
            double theta;
            if (z<=alpha*L)
            {
                theta = c_1 * a * (c_2 * a * (Math.Cosh(((z) / (a))) - 1) - alpha * L * Math.Sinh(((z) / (a))) + z * (((alpha * L) / (a)) - ((z) / (2 * a))));
            }
            else
            {
                theta = c_1 * Math.Pow(a, 2) * (c_3 - c_4 * Math.Cosh(((z) / (a))) + c_5 * Math.Sinh(((z) / (a))));
            }
            return theta;
            
        }

        public override double Get_theta_1()
        {
            double theta_1;
            if (z <= alpha * L)
            {
                theta_1 = c_1 * (c_2 * Math.Sinh(((z) / (a))) - alpha * L * Math.Cosh(((z) / (a))) + alpha * L - z);
            }
            else
            {
                theta_1 = c_1 * a * (-c_4 * Math.Sinh(((z) / (a))) + c_5 * Math.Cosh(((z) / (a))));
            }
            return theta_1;
        }

        public override double Get_theta_2()
        {
            double theta_2;
            if (z <= alpha * L)
            {
                theta_2 = ((c_1) / (a)) * (c_2 * a * Math.Cosh(((z) / (a))) - alpha*L*Math.Sinh(((z) / (a))) - a);
            }
            else
            {
                theta_2 = c_1 * (-c_4 * Math.Cosh(((z) / (a))) + c_5 * Math.Sinh(((z) / (a))));
            }
            return theta_2;
        }

        public override double Get_theta_3()
        {
            double theta_3;
            if (z <= alpha * L)
            {
                theta_3 = ((c_1) / (Math.Pow(a, 2))) * (c_2 * a * Math.Sinh(((z) / (a))) - alpha * L * Math.Cosh(((z) / (a))));
            }
            else
            {
                theta_3 = ((c_1) / (a)) * (-c_4 * Math.Sinh(((z) / (a))) + c_5 * Math.Cosh (((z) / (a))));
            }
            return theta_3;
        }
    }
}
