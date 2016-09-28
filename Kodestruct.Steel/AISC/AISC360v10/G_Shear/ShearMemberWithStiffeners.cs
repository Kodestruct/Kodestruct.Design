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
    public partial class ShearMemberWithStiffeners: ShearMemberGeneral
    {
        public ShearMemberWithStiffeners(double h, double t_w, double a, ISteelMaterial material, bool IsTeeShape = false)
        :base(h,t_w,a,material,IsTeeShape)
        {

        }

        protected override double Get_k_v()
        {
            double k_v;

            if (a/h>3 || a/h>Math.Pow((260 / ((h / t_w))), 2))
            {
                k_v = 5.0;
            }
            else
	        {
                //(G2-6)
                k_v = 5.0 + ((5.0) / (Math.Pow(((a / h)), 2)));
	        }

            return k_v;
        }
    }
}
