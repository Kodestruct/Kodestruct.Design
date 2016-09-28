#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

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
using Kodestruct.Common.Entities;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI318_14
{
    /// <summary>
    ///  This class encpsulates sectional shear provisions per ACI.
    /// </summary>
    public partial class ConcreteSectionOneWayShearNonPrestressed : AnalyticalElement
    {

        public double GetUpperLimitShearStrength(double phiV_c)
        {

            double h = Section.SliceableShape.YMax - Section.SliceableShape.YMin;
            this.A_g = Section.SliceableShape.A;
            this.N_u = N_u;
            this.rho_w = rho_w;
            double V_max;
            double f_c = Section.Material.SpecifiedCompressiveStrength;

            double lambda = Section.Material.lambda;
            StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            double phi = f.Get_phi_ShearReinforced();

            V_max = phiV_c + phi*(10.0 * lambda * Section.Material.Sqrt_f_c_prime * b_w * d); // (22.5.1.2)
  

            return V_max;
        }


    }
}
