using Kodestruct.Common.Data;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.NDS.NDS2015.Material.Laminated;
using Kodestruct.Wood.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public class GlulamSoftwoodFlexuralMemberSimple: GlulamMember
    {

        public GluelamSoftwoodMaterialFlexureSimple  Material { get; set; }
        public ISectionRectangular Section { get; set; }
        public double b { get; set; }
        public double d { get; set; }
        public int NumberLaminations { get; set; }


        public GlulamSoftwoodFlexuralMemberSimple(double b, double d, int NumberLaminations, 
            GlulamSimpleFlexuralStressClass StressClass, GlulamWoodSpeciesSimple WoodSpecies)
        {
            this.b = b;
            this.d = d;
            this.Material = new GluelamSoftwoodMaterialFlexureSimple(StressClass, WoodSpecies, NumberLaminations);
            this.Section = new SectionRectangular(b, d);
            this.NumberLaminations = NumberLaminations;
   
        }

    }
}
