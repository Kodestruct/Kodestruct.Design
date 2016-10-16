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

        public double GetTotalShearStrength(double phiV_c, double phiV_s)
        {
            double V_t1 = phiV_c + phiV_s;
            double V_t2 = GetMaximumShearStrength(phiV_c);
            return Math.Min(Math.Abs(V_t1), Math.Abs(V_t2));
        }

        public double GetMaximumShearStrength(double phiV_c)
        {
            StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            double phi = f.Get_phi_ShearReinforced();

            //Section 22.5.1.2 
            double phiV_nMax = phiV_c + phi * 8 * Section.Material.Sqrt_f_c_prime * b_w * d;
            return phiV_nMax;
        }
    }
}
