using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public partial class GlulamMember : WoodMember
    {
        /// <summary>
        /// Factor per 3.7.1.3 
        /// </summary>
        /// <returns></returns>
        protected override double Get_c()
        {
            return 0.9;
        }

        public double Get_FcStar(double F_c, double C_M_Fc, double C_t_Fc, double lambda)
        {
            double K_F = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.CompresionParallelToGrain);
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.CompresionParallelToGrain);
            double FcStar = F_c * C_M_Fc * C_t_Fc * K_F * phi * lambda; //from Table 4.3.1
            return FcStar;
        }

    }
}
