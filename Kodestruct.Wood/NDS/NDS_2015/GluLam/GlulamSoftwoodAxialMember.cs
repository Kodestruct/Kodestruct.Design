using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.NDS.NDS2015.Material.Laminated;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public class GlulamSoftwoodAxialMember : GlulamMember
    {
        public GluelamSoftwoodMaterialAxial Material { get; set; }
        public ISectionRectangular Section { get; set; }
        public double b { get; set; }
        public double d { get; set; }
        public int NumberLaminations { get; set; }

        public GlulamSoftwoodAxialMember(double b, double d, int NumberLaminations,
            GlulamSoftWoodAxialCombinationSymbol CombinationSymbol)
        {
            this.b = b;
            this.d = d;
            this.Material = new GluelamSoftwoodMaterialAxial(CombinationSymbol, d, NumberLaminations);
            this.Section = new SectionRectangular(b, d);
            this.NumberLaminations = NumberLaminations;

        }
    }
}
