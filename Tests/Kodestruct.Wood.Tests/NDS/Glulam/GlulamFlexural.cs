using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.NDS.NDS_2015.Entities.Beam;
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

        [Fact]
        public void SPFlexuralMemberReturnsFlexuralStrength()
        {
            //AWC presentation example
            double l_e = 148;
            double L = 32 * 12;
            GlulamBeam beam = new GlulamBeam(5, 30.25, 12, GlulamSimpleFlexuralStressClass.SC_24F1_8E, GlulamWoodSpeciesSimple.SouthernPineNoWanes,l_e,L,75, Wood.NDS.Entities.MoistureCondition.Dry, false);
            double phiM_n = beam.GetFlexuralStrengthMajorAxis(Wood.NDS.LoadCombinationType.FullWind);

            double F_b_prime = 2584.0 / 1.15 * 2.54 * 0.85; //Convert from ASD example to LRFD calculation value
            double phiM_n_Example = F_b_prime * 762.6;
            Assert.True(Math.Abs(phiM_n_Example - phiM_n)<= phiM_n_Example*0.02);
        }
    }
}
