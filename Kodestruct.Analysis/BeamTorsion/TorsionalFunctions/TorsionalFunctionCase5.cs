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
    public class TorsionalFunctionCase5: TorsionalFunctionBase
    {
        public TorsionalFunctionCase5(double G, double J, double L, double z, double a,
            double t)
            :base( G,  J,  L,  z,  a)
        {
            this.t = t;
        }

        protected override double Get_c_1()
        {
            double c_1 = ((t) / (G * J));
            return c_1;
        }
        public override double Get_theta()
        {
            double theta = c_1 * L * (((z) / (6)) - ((z * Math.Pow(a, 2)) / (Math.Pow(L, 2))) + ((Math.Pow(a, 2)) / (L)) * ((Math.Sinh(((z) / (a)))) / (Math.Sinh(((L) / (a))))) - ((Math.Pow(z, 3)) / (6 * Math.Pow(L, 2))));
            return theta;
        }

        public override double Get_theta_1()
        {
            double theta_1 = c_1 * L * (((1) / (6)) - ((Math.Pow(a, 2)) / (Math.Pow(L, 2))) + ((a) / (L)) * ((Math.Cosh(((z) / (a)))) / (Math.Sinh(((L) / (a))))) - ((Math.Pow(z, 2)) / (2 * Math.Pow(L, 2))));
            return theta_1;
        }

        public override double Get_theta_2()
        {
            double theta_2 = c_1 * (((Math.Sinh(((z) / (a)))) / (Math.Sinh(((L) / (a))))) - ((z) / (L)));
            return theta_2;
        }

        public override double Get_theta_3()
        {
            double theta_3 = c_1 * (((Math.Cosh(((z) / (a)))) / (a * Math.Sinh(((L) / (a))))) - ((1) / (L)));
            return theta_3;
        }
    }
}
