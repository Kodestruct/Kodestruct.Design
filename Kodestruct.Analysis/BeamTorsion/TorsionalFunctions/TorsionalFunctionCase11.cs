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
    public class TorsionalFunctionCase11: TorsionalFunctionBase
    {
        public TorsionalFunctionCase11(double G, double J, double L, double z, double a,
            double t)
            :base( G,  J,  L,  z,  a)
        {
            this.t = t;
        }

        public override double Get_theta()
        {
            double theta = c_1 * (1 - ((5 * Math.Pow(L, 2)) / (6 * Math.Pow(a, 2))) - (((a) / (L)) - ((L) / (2 * a))) * Math.Tanh(((L) / (a))) + z * (-((1) / (L)) + ((L) / (Math.Pow(a, 2)))) + (((a) / (L)) - ((L) / (2 * a))) * (((Math.Sinh(((z) / (a)))) / (Math.Cosh(((L) / (a)))))) - ((Math.Pow(z, 3)) / (6 * Math.Pow(a, 2) * L)));
            return theta;
        }

        public override double Get_theta_1()
        {
            double theta_1 = ((c_1) / (a)) * (-((a) / (L)) + ((L) / (a)) + (((a) / (L)) - ((L) / (2 * a))) * (((Math.Cosh(((z) / (a)))) / (Math.Cosh(((L) / (a)))))) - ((Math.Pow(z, 2)) / (2 * a * L)));
            return theta_1;
        }

        public override double Get_theta_2()
        {
            double theta_2 = ((c_1) / (Math.Pow(a, 2))) * ((((a) / (L)) + ((L) / (2 * a))) * (((Math.Sinh(((z) / (a)))) / (Math.Cosh(((L) / (a)))))) - ((z) / (L)));
            return theta_2;
        }

        public override double Get_theta_3()
        {
            double theta_3 = ((c_1) / (Math.Pow(a, 3))) * ((((a) / (L)) - ((L) / (2 * a))) * (((Math.Cosh(((z) / (a)))) / (Math.Cosh(((L) / (a)))))) - ((a) / (L)));
            return theta_3;
        }
    }
}
