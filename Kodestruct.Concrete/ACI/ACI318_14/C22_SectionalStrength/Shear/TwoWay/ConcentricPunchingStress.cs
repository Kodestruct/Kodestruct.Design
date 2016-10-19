using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public partial class ConcreteSectionTwoWayShear
    {


        /// <summary>
        /// Returns shear stress
        /// </summary>
        /// <param name="V_u"> Punching shear force</param>
        /// <returns></returns>
        public double GetConcentricShearStress(double V_u)
        {
            double b_o = Get_b_o();
            double v_u = V_u / (b_o * d);
            return v_u;
        }
    }
}
