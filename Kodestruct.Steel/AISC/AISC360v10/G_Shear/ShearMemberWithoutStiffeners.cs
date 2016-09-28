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
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Shear
{
    public partial class ShearMemberWithoutStiffeners : ShearMemberGeneral
    {
        public ShearMemberWithoutStiffeners(double h, double t_w, ISteelMaterial material, bool IsTeeShape = false) 
        :base (h,t_w,0,material,IsTeeShape)
        {

        }

        protected override double Get_k_v()
        {
            if (h/t_w>260)
            {
                throw new Exception("Web slenderness exceeds h/t_w =260 limit. Revise member section.");
            }
            //section G2.1(b)
            if (IsTeeShape == true)
            {
                return 1.2;
            }
            else
            {
                return 5.0;
            }
        }
    }
}
