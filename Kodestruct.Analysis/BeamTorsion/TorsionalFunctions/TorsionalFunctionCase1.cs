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
    public class TorsionalFunctionCase1: TorsionalFunctionBase
    {
        public TorsionalFunctionCase1(double G, double J, double L, double z, double T)
            :base( G,  J,  L,  z,  0.0)
        {
            this.T = T;
        }

        double T;
        public override double Get_theta()
        {
            double theta = ((T * z) / (G * J));
            return theta;
        }

        public override double Get_theta_1()
        {
            double theta_1 = ((T) / (G * J));
            return theta_1;
        }

        public override double Get_theta_2()
        {
            return 0.0;
        }

        public override double Get_theta_3()
        {
            return 0.0;
        }
    }
}
