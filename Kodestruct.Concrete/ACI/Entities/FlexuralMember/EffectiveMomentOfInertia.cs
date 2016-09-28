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
