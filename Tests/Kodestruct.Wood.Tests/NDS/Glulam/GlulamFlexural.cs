using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.NDS.NDS2015.GluLam;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kodestruct.Wood.Tests.NDS.Glulam
{
    public partial class GlulamTests
    {
        [Fact]
        public void SoftwoodSimpleFlexuralMemberReturnsF_bx_Plus()
        {
            //AWC presentation example
            GlulamSoftwoodFlexuralMemberSimple member = new GlulamSoftwoodFlexuralMemberSimple(5, 30.25, 12, GlulamSimpleFlexuralStressClass.SC_24F1_8E, GlulamWoodSpeciesSimple.SouthernPineNoWanes);
            double F_bx_plus = member.Material.F_bx_p;
            Assert.Equal(2400, F_bx_plus);
        }
        [Fact]
        public void SoftwoodSimpleFlexuralMemberReturnsF_bx_Minus()
        {
            //AWC presentation example
            GlulamSoftwoodFlexuralMemberSimple member = new GlulamSoftwoodFlexuralMemberSimple(5, 30.25, 12, GlulamSimpleFlexuralStressClass.SC_24F1_8E, GlulamWoodSpeciesSimple.SouthernPineNoWanes);
            double F_bx_minus = member.Material.F_bx_n;
            Assert.Equal(1450, F_bx_minus);
        }
        [Fact]
        public void SoftwoodSimpleFlexuralMemberReturnsF_cPerp_X()
        {
            //AWC presentation example
            GlulamSoftwoodFlexuralMemberSimple member = new GlulamSoftwoodFlexuralMemberSimple(5, 30.25, 12, GlulamSimpleFlexuralStressClass.SC_24F1_8E, GlulamWoodSpeciesSimple.SouthernPineNoWanes);
            double F_cPerp_x= member.Material.F_c_perp_x;
            Assert.Equal(650, F_cPerp_x);
        }

        [Fact]
        public void SoftwoodSimpleFlexuralMemberReturnsF_vx()
        {
            //AWC presentation example
            GlulamSoftwoodFlexuralMemberSimple member = new GlulamSoftwoodFlexuralMemberSimple(5, 30.25, 12, GlulamSimpleFlexuralStressClass.SC_24F1_8E, GlulamWoodSpeciesSimple.SouthernPineNoWanes);
            double F_vx = member.Material.F_vx;
            Assert.Equal(265, F_vx);
        }
    }
}
