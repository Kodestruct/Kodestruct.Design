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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;

using Kodestruct.Steel.AISC.SteelEntities;

namespace Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public partial class AffectedElement : SteelDesignElement
    {
        /// <summary>
        /// Calculates block shear strength per AISC J4.3
        /// </summary>
        /// <param name="A_gv">Gross area subject to shear</param>
        /// <param name="A_nv">Net area subject to shear</param>
        /// <param name="StressIsUniform"> Defines if stress is uniform on the connection group</param>
        /// <param name="A_nt">Net area ubject to tension</param>
        /// 
        public double GetBlockShearStrength(double A_gv, double A_nv, double A_nt, bool StressIsUniform)
        {
            double Rn1 = GetShearYieldingComponent(A_gv)+GetTensionRuptureComponent(StressIsUniform,A_nt);
            double Rn2 = GetShearRuptureComponent(A_nv)+GetTensionRuptureComponent(StressIsUniform,A_nt);
            double phi = 0.75;
            return phi*Math.Min(Math.Abs(Rn1), Math.Abs(Rn2));
        }


        /// <summary>
        /// Calculates shear yielding component of equation (J4-5)
        /// </summary>
        /// <param name="A_gv">Gross area subject to shear</param>
        /// <returns></returns>
        public double GetShearYieldingComponent (double A_gv) 
        {
            double F_y = this.Section.Material.YieldStress;
            return 0.6*F_y * A_gv;
        }

        /// <summary>
        /// Calculates shear rupture component of equation (J4-5)
        /// </summary>
        /// <param name="A_nv">Net area subject to shear</param>
        /// <returns></returns>
        public double GetShearRuptureComponent(double A_nv) 
        {
            double F_u = this.Section.Material.UltimateStress;
            return 0.6* F_u * A_nv;
        }

        /// <summary>
        /// Calculates tension rupture component of equation (J4-5)
        /// </summary>
        /// <param name="StressIsUniform"> Defines if stress is uniform on the connection group</param>
        /// <param name="A_nt">Net area ubject to tension</param>
        /// <returns></returns>
        public double GetTensionRuptureComponent(bool StressIsUniform,double A_nt) 
        {
            double U_bs = StressIsUniform == true ? 1.0 : 0.5;
            double F_u = this.Section.Material.UltimateStress;
            return U_bs * F_u * A_nt;

            
        }

    }
}
