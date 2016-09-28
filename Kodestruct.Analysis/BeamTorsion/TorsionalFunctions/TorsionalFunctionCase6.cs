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
    public class TorsionalFunctionCase6: TorsionalFunctionBase
    {
        public TorsionalFunctionCase6(double G, double J, double L, double z, double a,
            double T, double alpha)
            :base( G,  J,  L,  z,  a)
        {
            this.T = T;
            this.alpha = alpha;
        }

        protected override double Get_H()
        {
            double H = H = ((((1 - Math.Cosh(((alpha * L) / (a)))) / (Math.Tanh(((L) / (a))))) + ((Math.Cosh(((alpha * L) / (a)) - 1)) / (Math.Sinh(((L) / (a))))) + Math.Sinh(((alpha * L) / (a))) - ((alpha * L) / (a))) / (((Math.Cosh(((L) / (a))) + Math.Cosh(((alpha * L) / (a))) * Math.Cosh(((L) / (a))) - Math.Cosh(((alpha * L) / (a))) - 1) / (Math.Sinh(((L) / (a))))) + ((L) / (a)) * (alpha - 1) - Math.Sinh(((alpha * L) / (a)))));
            return H;
        }

        protected override double Get_c_1()
        {
            double c_1 = ((T) / ((H + 1) * G * J));
            return c_1;
        }

        protected override double Get_c_2()
        {
            double c_2 = H * (((1) / (Math.Sinh(((L) / (a))))) + Math.Sinh(((alpha * L) / (a))) - ((Math.Cosh(((alpha * L) / (a)))) / (Math.Tanh(((L) / (a)))))) + Math.Sinh(((alpha * L) / (a))) - ((Math.Cosh(((alpha * L) / (a)))) / (Math.Tanh(((L) / (a))))) + ((1) / (Math.Tanh(((L) / (a)))));

            return c_2;


        }

        protected override double Get_c_3()
        {
            double c_3=((Math.Cosh(((alpha*L) / (a)))-1) / (H*Math.Sinh(((L) / (a)))))+((Math.Cosh(((alpha*L) / (a)))-Math.Cosh(((L) / (a)))+((L) / (a))*Math.Sinh(((L) / (a)))) / (Math.Sinh(((L) / (a)))));
            return c_3;
        }

        protected override double Get_c_4()
        {
            double c_4 = ((1 - Math.Cosh(((alpha * L) / (a)))) / (H * Math.Tanh(((L) / (a))))) + ((1 - Math.Cosh(((alpha * L) / (a))) * Math.Cosh(((L) / (a)))) / (Math.Sinh(((L) / (a)))));
            return c_4;
        }

        protected override double Get_c_5()
        {
            double c_5 = ((Math.Cosh(((alpha * L) / (a))) - 1) / (H)) + Math.Cosh(((alpha * L) / (a)));
            return c_5;
        }

        public override double Get_theta()
        {
            double theta;
            if (z<=alpha*L)
            {
                theta = c_1 * a * (c_2 * (Math.Cosh(((z) / (a))) - 1) - Math.Sinh(((z) / (a))) + ((z) / (a)));
            }
            else
            {
                theta = H * c_1 * a * (c_3 + c_4 * Math.Cosh(((z) / (a))) + c_5 * Math.Sinh(((z) / (a))) - ((z) / (a)));
            }
            return theta;
        }

        public override double Get_theta_1()
        {
            double theta_1;
            if (z <= alpha * L)
            {
                theta_1 = c_1 * (c_2 * Math.Sinh(((z) / (a))) - Math.Cosh(((z) / (a))) + 1);
            }
            else
            {
                theta_1 = H*c_1 * (c_4 * Math.Sinh(((z) / (a))) + c_5 * Math.Cosh(((z) / (a))) - 1);
            }
            return theta_1;
        }

        public override double Get_theta_2()
        {
            double theta_2;
            if (z <= alpha * L)
            {
                theta_2 = ((c_1) / (a)) * (c_2 * Math.Cosh(((z) / (a))) - Math.Sinh(((z) / (a))));
            }
            else
            {
                theta_2 = ((H * c_1) / (a)) * (c_4 * Math.Cosh(((z) / (a))) + c_5 * Math.Sinh(((z) / (a))));
            }
            return theta_2;
        }

        public override double Get_theta_3()
        {
            double theta_3;
            if (z <= alpha * L)
            {
                theta_3 = ((c_1) / (Math.Pow(a, 2))) * (c_2 * Math.Sinh(((z) / (a))) - Math.Cosh(((z) / (a))));
            }
            else
            {
                theta_3 = ((H*c_1) / (Math.Pow(a, 2))) * (c_4 * Math.Sinh(((z) / (a))) - c_5*Math.Cosh(((z) / (a))));
            }
            return theta_3;
        }
    }
}
