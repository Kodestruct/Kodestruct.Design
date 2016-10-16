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
    public class TorsionalFunctionCase2: TorsionalFunctionBase
    {
        public TorsionalFunctionCase2(double G, double J, double L, double z, double a, double T)
            :base( G,  J,  L,  z,  a)
        {
            this.T = T;
        }
        double T;

        protected override double Get_c_1()
        {
            double c_1 = ((T * a) / (G * J));
            return c_1;
        }
        protected override double Get_c_2()
        {
            double c_2 = Math.Tanh(((L) / (2 * a)));
            return c_2;
        }
        public override double Get_theta()
        {
            double theta=c_1*(c_2*Math.Cosh(((z) / (a)))-c_2+((z) / (a))-Math.Sinh(((z) / (a))));
            return theta;

        }

        public override double Get_theta_1()
        {
            double theta_1 = ((c_1) / (a)) * (1 - Math.Cosh(((z) / (a))) + c_2 * Math.Sinh(((z) / (a))));
            return theta_1;
        }

        public override double Get_theta_2()
        {
            double theta_2 = ((c_1) / (Math.Pow(a, 2))) * (c_2 * Math.Cosh(((z) / (a))) - Math.Sinh(((z) / (a))));
            return theta_2;
        }

        public override double Get_theta_3()
        {
            double theta_3 = ((c_1) / (Math.Pow(a, 3))) * (-Math.Cosh(((z) / (a))) + c_2 * Math.Sinh(((z) / (a))));
            return theta_3;
        }
    }
}
