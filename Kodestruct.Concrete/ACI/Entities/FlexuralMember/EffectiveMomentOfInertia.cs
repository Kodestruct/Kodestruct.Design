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

namespace Kodestruct.Concrete.ACI.Entities.FlexuralMember
{
    public class EffectiveMomentOfInertiaCalculator
    {
        public double GetI_e(double I_g, double I_cr, double M_cr, double M_a)
        {
            
                //24.2.3.5a
                double I_e = Math.Pow((((M_cr) / (M_a))), 3.0) * I_g + (1.0 - Math.Pow((((M_cr) / (M_a))), 3.0)) * I_cr;
                if (I_e > I_g)
                {
                    I_e = I_g;
                }
                return I_e;
            
        }
    }
}
