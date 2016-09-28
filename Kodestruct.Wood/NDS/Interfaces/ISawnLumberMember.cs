using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Wood.NDS
{
    public interface ISawnLumberMember
    {
        double GetAdjustedBendingDesignValue(double F_b, double C_M,double C_t, double C_L,double C_F, double C_fu_Fb,double C_i, double C_r, double lambda);
        double GetAdjustedCompressionDesignValue(double F_c, double C_M, double C_t, double C_F, double C_i, double C_P, double lambda);
        double GetAdjustedShearDesignValue(double F_v, double C_M, double C_t, double C_i, double lambda);
        double GetAdjustedTensionValue(double F_t, double C_M, double C_t, double C_F, double C_i, double lambda);
        double GetAdjustedModulusOfElasticity(double E, double C_M, double C_t, double C_i);
        double GetAdjustedMinimumModulusOfElasticityForStability(double E_min, double C_M_E, double C_t, double C_i, double C_T);
    }
}
