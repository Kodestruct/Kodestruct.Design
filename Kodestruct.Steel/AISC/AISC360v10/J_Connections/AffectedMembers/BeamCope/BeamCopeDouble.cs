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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public class BeamCopeDouble: BeamCopeBase
    {

         public BeamCopeDouble(double c, double d_c, ISectionI Section, ISteelMaterial Material):
            base(c,d_c,Section,Material)
        {

        }

         private SectionRectangular _rectangle;

         public SectionRectangular rectangle
         {
             get
             {
                 if (_rectangle == null)
                 {
                     GetCopeSection();
                 }
                 return _rectangle;
             }

         }


         private void GetCopeSection()
         {
             _rectangle = new SectionRectangular(this.Section.t_w, this.h_o);
         }

         protected override double GetS_net()
         {
             double S_net = 0.0;
             if (rectangle!=null)
             {
                 S_net = rectangle.S_xTop;
                 // note: top and bottom S_x are the same for rectangles
             }
             return S_net;
         }

         protected override double GetZ_net()
         {
             double Z_net = 0.0;
             if (rectangle != null)
             {
                 Z_net = rectangle.Z_x;
             }
             return Z_net;
         }


         public override double GetF_cr()
         {
             double F_cr;

             bool PermissibleCopeGeometry = CheckCopeGeometry();
             if (PermissibleCopeGeometry==true)
             {
                 double E = Material.ModulusOfElasticity;
                 double f_d = Get_LateralTorsionalBucklingAdjustmentFactor();
                 F_cr = 0.62 * Math.PI * E * ((Math.Pow(t_w, 2)) / (c * h_o)) * f_d; //Manual Eq 9-12
             }
             else
             {
                 F_cr = GetFcrGeneral();
             }

             double F;
             double F_y = Material.YieldStress;
             F = Math.Abs(F_cr) > F_y ? F_y : Math.Abs(F_cr);
             return F;
         }

         private double Get_LateralTorsionalBucklingAdjustmentFactor()
         {
             double d = Section.d;
             double f_d = 3.5 - 7.5 * (((d_c) / (d))); //Manual Eq 9-13
             return f_d;
         }

         protected override double Get_h_o()
         {
             return Section.d - 2 * d_c;
         }

         public override double Get_t_w()
         {
             return Section.t_w;
         }
    }
}
