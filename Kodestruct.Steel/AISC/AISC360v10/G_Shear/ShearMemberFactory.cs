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
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Materials;


namespace Kodestruct.Steel.AISC.AISC360v10.Shear
{
    public class ShearMemberFactory
    {
        public IShearMember GetShearMemberNonCircular(ShearNonCircularCase ShearCase, double h, double t_w, double a, double F_y,double E)
        {
            ISteelMaterial material = new SteelMaterial(F_y,E);
            IShearMember member =null;
            bool IsTeeShape;


            switch (ShearCase)
            {
                case ShearNonCircularCase.MemberWithStiffeners:
                     IsTeeShape =false;
                     member = new ShearMemberWithStiffeners(h, t_w, a, material, IsTeeShape);
                    break;
                case ShearNonCircularCase.Tee:
                    IsTeeShape = true;
                    member = new ShearMemberWithoutStiffeners(h, t_w, material, IsTeeShape);
                    break;
                case ShearNonCircularCase.Box:
                    member = new ShearMemberBox(h, t_w, material);
                    break;
                default:
                    IsTeeShape = false;
                    member = new ShearMemberWithoutStiffeners(h, t_w, material, IsTeeShape);
                    break;
            }
            return member;
        }
        public IShearMember GetShearMemberCircular(double D, double t_nom, bool Is_SAW_member, double L_v, double F_y, double E)
        {
            ISteelMaterial material = new SteelMaterial(F_y,E);
            ShearMemberCircular member = new ShearMemberCircular(D, t_nom, Is_SAW_member, L_v, material);
            return member;
        }
    }
}
