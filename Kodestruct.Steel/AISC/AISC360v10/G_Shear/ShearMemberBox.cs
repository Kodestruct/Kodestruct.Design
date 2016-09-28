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
