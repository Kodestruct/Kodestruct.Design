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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;


namespace Kodestruct.Steel.AISC.AISC360v10.Shear
{
    public partial class ShearMemberBox : ShearMemberGeneral
    {
        public ShearMemberBox(ISectionTube section, ISteelMaterial material)
          
        {
            base.material = material;
            base.t_w = 2.0 * section.t_des;
            base.h = section.H - 3.0 * section.t_des;
        }
        public ShearMemberBox(double h, double t_w, ISteelMaterial material)
            : base(h, t_w, 0, material, false)
        {

        }

        protected override double Get_k_v()
        {

                return 5.0;
            
        }
    }
}
