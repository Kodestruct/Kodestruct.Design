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
