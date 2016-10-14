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
using Kodestruct.Steel.AISC.Steel.Entities;


namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public partial class RhsNonSlender : ColumnDoublySymmetric
    {


        public override double CalculateCriticalStress()
        {
            double Fcr = 0.0;

            //Flexural

            double FeFlexuralBuckling = GetFlexuralElasticBucklingStressFe(); //this does not apply to unsymmetric sections
            double FcrFlexuralBuckling = GetCriticalStressFcr(FeFlexuralBuckling, 1.0);
            double Qflex = 1.0;
            double FcrFlex = GetCriticalStressFcr(FeFlexuralBuckling, Qflex);

            return FcrFlex;

        }

        public override SteelLimitStateValue GetFlexuralBucklingStrength()
        {
            double FcrFlex = CalculateCriticalStress();
            double phiP_n = GetDesignAxialStrength(FcrFlex);

            SteelLimitStateValue ls = new SteelLimitStateValue(phiP_n, true);
            return ls;
        }

        public override SteelLimitStateValue GetTorsionalAndFlexuralTorsionalBucklingStrength()
        {
            return new SteelLimitStateValue(-1, false);

        }

        
        public RhsNonSlender(ISteelSection Section, double L_ex, double L_ey, double L_ez, ICalcLog CalcLog)
            : base(Section,L_ex,L_ey, L_ez, CalcLog)
        {
            if (Section.Shape is ISectionTube)
            {
                SectionRhs = Section.Shape as ISectionTube;
            }
            else
            {
                throw new Exception("Section of wrong type: Need ISectionTube");
            }

        }


        ISectionTube SectionRhs; 
    }
}
