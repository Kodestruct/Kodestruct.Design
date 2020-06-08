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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public abstract partial class SawnLumberMember : WoodMember, ISawnLumberMember
    {


        public double GetAdjustedBendingDesignValue(double F_b, double C_M_Fb, double C_t_Fb, double C_L, double C_F_Fb, double C_fu_Fb, double C_i_Fb, double C_r, double lambda)
        {
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.Bending); //Table N.3.2
            double K_f = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.Bending); //Table 4.3.1
            double F_b_prime = F_b* phi * K_f*lambda * C_M_Fb * C_t_Fb * C_L * C_F_Fb * C_fu_Fb * C_i_Fb * C_r;
            return F_b_prime;
        }

        public double GetAdjustedCompressionDesignValue(double F_c, double C_M_Fc, double C_t_Fc, double C_F_Fc, double C_i_Fc, double C_P, double lambda)
        {
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.CompresionParallelToGrain); //Table N.3.2 //Table 4.3.1
            double K_f = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.CompresionParallelToGrain); //Table 4.3.1
            double F_c_prime = F_c*phi * K_f * lambda * C_M_Fc * C_t_Fc *  C_F_Fc * C_i_Fc * C_P;
            return F_c_prime;
        }

        public double GetAdjustedShearDesignValue(double F_v, double C_M_Fv, double C_t_Fv, double C_i_Fv, double lambda)
        {
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.ShearParallelToGrain); //Table N.3.2; //Table 4.3.1
            double K_f = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.ShearParallelToGrain); //Table 4.3.1
            double F_v_prime = F_c * phi * K_f * lambda * C_M_Fv * C_t_Fv *  C_i_Fv ;
            return F_v_prime;
        }

        public double GetAdjustedTensionValue(double F_t, double C_M_Ft, double C_t_Ft, double C_F_Ft, double C_i_Ft, double lambda)
        {
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.TensionParallelToGrain); //Table N.3.2; //Table 4.3.1
            double K_f = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.TensionParallelToGrain); //Table 4.3.1
            double F_t_prime = F_t * phi * K_f * lambda * C_M_Ft * C_t_Ft * C_F_Ft * C_i_Ft;
            return F_t_prime;
        }

        public double GetAdjustedModulusOfElasticity(double E, double C_M, double C_t_E, double C_i )
        {
            double E_prime = E * C_M * C_t_E * C_i;  //from Table 4.3.1
            return E_prime;
        }
        public override double GetAdjustedMinimumModulusOfElasticityForStability(double E_min, double C_M_E, double C_t_E, double C_i_E, double C_T)
        {
            double K_F = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.ModulusOfElasticityMin);
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.ModulusOfElasticityMin); //Table N.3.2;
            double E_min_prime = E_min * C_M_E * C_t_E * C_i_E * C_T * K_F * phi; //from Table 4.3.1
            return E_min_prime;
        }
    }
}
