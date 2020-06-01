using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Compression;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kodestruct.Steel.Tests.AISC.DesignExamples
{
    public class E_CompressionMembersTests
    {
        [Fact]
        public void E1AReturnsAxialCapacity()
        {
            //AiscCatalogShape section = new AiscCatalogShape("W14X90", null);
 
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISectionI section = (ISectionI) AiscShapeFactory.GetShape("W14X90", ShapeTypeSteel.IShapeRolled);
            ISteelMaterial Material = new SteelMaterial(65,29000);
            ISteelSection steelSection = new SteelSectionI(section, Material);
            IShapeCompact col = new IShapeCompact(steelSection, true,19 * 12, 19 * 12, 19 * 12);
            double phiPn = col.GetFlexuralBucklingStrength().Value;
            Assert.True(phiPn ==903);
        }
    }
}
