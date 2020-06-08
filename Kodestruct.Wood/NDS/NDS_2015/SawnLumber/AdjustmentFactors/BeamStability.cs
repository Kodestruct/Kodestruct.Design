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
using Kodestruct.Common.Entities;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {


        public double GetStabilityFactor(double b, 
            double d,
            double F_b,
            double E_min,
            double l_e,
            double C_M_Fb ,
            double C_M_E,
            double C_t_Fb ,
            double C_t_E,
            double C_F_Fb ,
            double C_i_Fb,
            double C_i_E,
            double C_r ,
            double C_T,
            double lambda
            )
        {
            this.d  = d;
            this.F_b= F_b;
            this.E_min = E_min;
            this.l_e = l_e;
            this.C_M_Fb= C_M_Fb;
            this.C_M_E = C_M_E;
            this.C_t_Fb= C_t_Fb;
            this.C_t_E = C_t_E;
            this.C_F_Fb= C_F_Fb;
            this.C_i_Fb= C_i_Fb;
            this.C_i_E = C_i_E;
            this.C_r= C_r;
            this.C_T= C_T;
            this.lambda = lambda;
            double C_L = base.GetC_L(b, d, l_e,  E_min,  C_M_E,  C_t_E,  C_i_E,  C_T);

            return C_L;
        }

        protected override double GetF_b_AdjustedForBeamStability()
        {
            double K_F = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.Bending);
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.Bending); //Table N.3.2;
            return F_b * C_M_Fb * C_t_Fb * C_F_Fb * C_i_Fb * C_r*K_F*phi*lambda; //from Table 4.3.1
            
        }

    }
}
