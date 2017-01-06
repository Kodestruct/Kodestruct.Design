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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;
using Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public class BeamUncoped : IBeamCope
    {
        public BeamUncoped(ISectionI Section, ISteelMaterial Material)
        {
            this.Section = Section;
            this.Material = Material;
        }

        public double GetFlexuralStrength()
        {
            AffectedElementInFlexure ae = new AffectedElementInFlexure(Section,
                null, Material.YieldStress, Material.UltimateStress, true, true);
            //Confirm if need to use Z_x
            //double F_y = Material.YieldStress;
            //double S_x = Math.Min(this.Section.S_xTop, this.Section.S_xBot);
            return ae.GetFlexuralStrength(0.0);
        }

        public ISectionI Section { get; set; }
        public ISteelMaterial Material { get; set; }
    }
}
