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

using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;

namespace  Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public partial class AffectedElementInShear: AffectedElement
    {

        public AffectedElementInShear(ISteelSection Section,ICalcLog CalcLog)
            : base(Section, CalcLog)
        {

        }

        public AffectedElementInShear(ISection Section, ISteelMaterial Material, ICalcLog CalcLog)
            :base(Section,Material, CalcLog)
        {

        }

        public AffectedElementInShear(double F_y, double F_u): base(F_y,F_u)
        {
            SteelMaterial mat = new SteelMaterial(F_y, F_u, SteelConstants.ModulusOfElasticity,
                SteelConstants.ShearModulus);
           
        }

        /// <summary>
        /// The available strength of affected or connecting element in shear
        /// </summary>
        /// <param name="A_gv">Gross area subject to shear</param>
        /// <param name="A_nv">Net area subject to shear</param>
        /// <returns></returns>
        public double GetShearCapacity(double A_gv, double A_nv)
        {
            double F_y = Section.Material.YieldStress;
            double F_u = Section.Material.UltimateStress;

            double phiR_nY = GetShearYieldingStrength(F_y,A_gv);
            double phiR_nU = GetShearRuptureStrength(F_u, A_nv);

            double phiR_n = Math.Min(phiR_nY, phiR_nU);
            return phiR_n;
        }

        private double GetShearRuptureStrength(double F_u, double A_nv)
        {

            double R_n = 0.6 * F_u * A_nv; // (J4-4)
            double phi = 0.75;
            return phi * R_n;
        }

        private double GetShearYieldingStrength(double F_y, double A_gv)
        {
            double R_n = 0.6 * F_y * A_gv; // (J4-3)
            double phi = 1.0;
            return phi * R_n;
        }
    }
}
