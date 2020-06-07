using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public partial class GlulamMember : WoodMember
    {
        public override double GetAdjustedMinimumModulusOfElasticityForStability(double E_min, double C_M_E, double C_t_E, double C_i_E, double C_T)
        {
            double K_F = 1.76;
            double phi = 0.85;
            double E_min_prime = E_min * C_M_E * C_t_E  * K_F * phi; //from Table 5.3.1
            return E_min_prime;
        }
    }   
}
