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
 
using Kodestruct.Common.Entities;
namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {
        public double GetColumnStabilityFactor( double d,
                                                double F_c,
                                                double E_min,
                                                double l_e,
                                                double C_M_Fc,
                                                double C_M_E,
                                                double C_t_Fc,
                                                double C_t_E,
                                                double C_F_Fc,
                                                double C_i_Fc,
                                                double C_i_E,                                   
                                                double C_T,
                                                double lambda
                                                )
        {
            this.d = d;
            this.F_c = F_c;
            this.E_min = E_min;
            this.l_e = l_e;
            this.C_M_Fc = C_M_Fc;
            this.C_M_E = C_M_E;
            this.C_t_Fc = C_t_Fc;
            this.C_t_E = C_t_E;
            this.C_F_Fc = C_F_Fc;
            this.C_i_Fc = C_i_Fc;
            this.C_i_E = C_i_E;
            this.C_T = C_T;
            this.lambda = lambda;
            double FcStar = Get_FcStar();
            double E_minPrime = GetAdjustedMinimumModulusOfElasticityForStability(E_min,C_M_E,C_t_E,C_i_E,C_T);
            C_P = base.GetC_P(FcStar, E_minPrime, l_e,d);

            return C_P;
        }

        private double Get_FcStar()
        {
            double K_F = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.CompresionParallelToGrain);
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.CompresionParallelToGrain);
            double FcStar = F_c * C_M_Fc * C_t_Fc * C_F_Fc * C_i_Fc *K_F * phi * lambda; //from Table 4.3.1
            return FcStar;
        }



        /// <summary>
        /// Factor from NDS 2105 section 3.7.1.5 required for the column stability factor
        /// </summary>
        /// <returns></returns>

        protected override double Get_c()
        {
            return 0.8;
        }
    }
}
