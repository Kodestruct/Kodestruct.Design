using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections;
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;

namespace Kodestruct.Steel.Tests.AISC.AISC36010.HSSTrussConnections
{

    [TestFixture]
    public class HssTrussRhsXConnectionTests : ToleranceTestBase
    {

        /// <summary>
        /// AISC DG24
        /// Example 8.3—Cross-Connection with Rectangular HSS
        /// </summary>
        [Test]
        public void HssXConnectionReturnsYieldingOfChordSidewallsValue()
        {
            CreateElements();
            IHssTrussBranchConnection con = new RhsTrussXConnection(Chord, Branch, 35, Branch, 35,
                AxialForceType.Compression, AxialForceType.Compression, true, 0, 0
                );

            double phiP_n = con.GetChordSidewallLocalYieldingStrength().Value;
            double refValue = 442;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC DG24
        /// Example 8.3—Cross-Connection with Rectangular HSS
        /// </summary>
        [Test]
        public void HssXConnectionReturnsCripplingOfChordSidewallsValue()
        {
            CreateElements();
            IHssTrussBranchConnection con = new RhsTrussXConnection(Chord, Branch, 35, Branch, 35,
                AxialForceType.Compression, AxialForceType.Compression, true, 0, 0
                );

            double phiP_n = con.GetChordSidewallLocalCripplingStrength().Value;
            double refValue = 75.4;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC DG24
        /// Example 8.3—Cross-Connection with Rectangular HSS
        /// </summary>
        [Test]
        public void HssXConnectionReturnsLocalYieldingOfBranchesDueToUnevenDistributionValue()
        {
            CreateElements();
            IHssTrussBranchConnection con = new RhsTrussXConnection(Chord, Branch, 35, Branch, 35,
                AxialForceType.Compression, AxialForceType.Compression, true, 0, 0
                );

            double phiP_n = con.GetBranchYieldingFromUnevenLoadDistributionStrength(true).Value;
            double refValue = 209;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        public HssTrussRhsXConnectionTests()
        {
            tolerance = 0.05; //5% can differ from rounding 
        }


        double tolerance;

        private void CreateElements()
        {
            SectionTube ch = new SectionTube(null, 8, 8, 1 / 4.0, 0.93 * (1.0 / 4.0), 0.35);
            SteelMaterial mat = new SteelMaterial(46.0);
            Chord = new SteelRhsSection(ch, mat);

            SectionTube br = new SectionTube(null, 6, 8, 3.0 / 8.0, 0.93 * (3.0 / 8.0), 0.35);
            Branch = new SteelRhsSection(br, mat);
        }

        SteelRhsSection Chord;
        SteelRhsSection Branch;
    }

}
