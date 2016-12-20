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

using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace  Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public partial class AffectedElement: SteelDesignElement
    {
        public AffectedElement()
        {

        }

        public AffectedElement(ICalcLog CalcLog)
            : base(CalcLog)
        {
        }
        public AffectedElement(double F_y, double F_u)
        {
            SteelMaterial material = new SteelMaterial(F_y, F_u, SteelConstants.ModulusOfElasticity, SteelConstants.ShearModulus);
            this.Section = new SteelGeneralSection(null, material);
        }
        public AffectedElement(ISteelSection Section, ICalcLog CalcLog)
            : base(CalcLog)
        {
            this.section = Section;
        }

        public AffectedElement(ISection Section, ISteelMaterial Material, ICalcLog CalcLog)
            :base(CalcLog)
        {
            this.section = new SteelGeneralSection(Section, Material); 
        }

        private ISteelSection section;

        public ISteelSection Section
        {
            get { return section; }
            set { section = value; }
        }

        private ISteelSection _SectionNet;

        public ISteelSection SectionNet
        {
            get { return _SectionNet; }
            set { _SectionNet = value; }
        }
    }
}
