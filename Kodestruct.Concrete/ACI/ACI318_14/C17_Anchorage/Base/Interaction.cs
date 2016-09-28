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

namespace Kodestruct.Concrete.ACI318_14.Anchorage
{
    public partial class ConcreteAnchorageElement
    {

        public double GetDemandToCapacityRatio(double N_u, double V_u, double phi_Nn, double phi_V_n)
        {
            //Section 17.6—Interaction of tensile and shear forces
            double DCR;

            if (V_u/phi_V_n<=0.2)
            {
                //17.6.1
                DCR = N_u / phi_Nn;
            }
            else if (N_u/phi_Nn <=0.2)
            {
                //17.6.2
                DCR = V_u / phi_V_n;
            }
            else
            {
                //17.6.3
                DCR = (N_u / phi_Nn + V_u / phi_V_n) / 1.2;
            }

            return DCR;
        }

    }
}
