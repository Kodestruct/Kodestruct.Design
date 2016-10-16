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

namespace Kodestruct.Analysis.Torsion
{
    public class TorsionalFunctionCase3: TorsionalFunctionBase
    {
        public TorsionalFunctionCase3(double G, double J, double L, double z, double a,
            double T, double alpha)
            :base( G,  J,  L,  z,  a)
        {
            this.alpha = alpha;
            this.T = T;
        }



        protected override double Get_c_1()
        {
            double c_1 = ((T) / (G * J));
            return c_1;
        }

        protected override double Get_c_2()
        {
            double c_2 = ((Math.Sinh(((alpha * L) / (a)))) / (Math.Tanh(((L) / (a))))) - Math.Cosh(((alpha * L) / (a)));
            return c_2;
        }
        public override double Get_theta()
        {
            double theta;
            if (z<=alpha*L)
            {
                theta = c_1 * ((1 - alpha) * z + c_2 * a * Math.Sinh(((z) / (a))));
            }
            else
            {
                theta=c_1*((L-z)*alpha+a*(((Math.Sinh(((alpha*L) / (a)))) / (Math.Tanh(((L) / (a)))))*Math.Sinh(((z) / (a)))-Math.Sinh(((alpha*L) / (a)))*Math.Cosh(((z) / (a)))));
            }
            return theta;
        }

        public override double Get_theta_1()
        {
            double theta_1;
            if (z <= alpha * L)
            {
                theta_1 = c_1 * ((1 - alpha) + c_2 * Math.Cosh(((z) / (a))));
            }
            else
            {
                theta_1 = c_1 * (((Math.Sinh(((alpha * L) / (a)))) / (Math.Tanh(((L) / (a))))) * Math.Cosh(((z) / (a))) - Math.Sinh(((alpha * L) / (a))) * Math.Sinh(((z) / (a))) - alpha);
            }
            return theta_1;
        }

        public override double Get_theta_2()
        {
            double theta_2;
            if (z <= alpha * L)
            {
                theta_2 = ((c_1 * c_2) / (a)) * Math.Sinh(((z) / (a)));
            }
            else
            {
                theta_2=((c_1) / (a))*(((Math.Sinh(((alpha*L) / (a)))) / (Math.Tanh(((L) / (a)))))*Math.Sinh(((z) / (a)))-Math.Sinh(((alpha*L) / (a)))*Math.Cosh(((z) / (a))));
            }
            return theta_2;
        }

        public override double Get_theta_3()
        {
            double theta_3;
            if (z <= alpha * L)
            {
                theta_3 = ((c_1 * c_2) / (Math.Pow(a, 2))) * Math.Cosh(((z) / (a)));
            }
            else
            {
                theta_3 = ((c_1) / (Math.Pow(a, 2))) * (((Math.Sinh(((alpha * L) / (a)))) / (Math.Tanh(((L) / (a))))) * Math.Cosh(((z) / (a))) - Math.Sinh(((alpha * L) / (a))) * Math.Sinh(((z) / (a))));
            }
            return theta_3;
        }
    }
}
