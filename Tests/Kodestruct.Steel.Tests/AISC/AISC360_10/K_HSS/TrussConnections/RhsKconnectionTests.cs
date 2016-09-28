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
    public class HssTrussRhsKConnectionTests : ToleranceTestBase
    {

        /// <summary>
        /// AISC DG 24
        /// Example 8.4—Overlapped K-Connection with Rectangular HSS
        /// </summary>
        [Test]
        public void HssOverlappedKConnectionReturnsYieldingOfBranchesFromUnevenDistributionValue()
        {
            SectionTube ch = new SectionTube(null, 8, 8, 0.5, 0.465);
            SteelMaterial mat = new SteelMaterial(46.0);
            SteelRhsSection Chord = new SteelRhsSection(ch, mat);

            SectionTube mainBranch = new SectionTube(null, 6, 4, 5.0 / 16.0, 0.291);
            SteelRhsSection MainBranch = new SteelRhsSection(mainBranch, mat);

            SectionTube secBranch = new SectionTube(null, 5, 3, 1.0 / 4.0, 0.233);
            SteelRhsSection SecondaryBranch = new SteelRhsSection(secBranch, mat);

            double O_v = 0.533;

            IHssTrussBranchConnection con = new RhsTrussOverlappedConnection(Chord, MainBranch, 45, 
                SecondaryBranch, 45,
                AxialForceType.Compression, AxialForceType.Tension, false, 0, 0, O_v
                );

            double phiP_nMain = con.GetBranchYieldingFromUnevenLoadDistributionStrength(true).Value;
            double refValueMain = 236; 
            double actualToleranceMain = EvaluateActualTolerance(phiP_nMain, refValueMain);
            Assert.LessOrEqual(actualToleranceMain, tolerance);

            double phiP_nSec = con.GetBranchYieldingFromUnevenLoadDistributionStrength(false).Value;
            double refValueSec = 151;
            double actualToleranceSec = EvaluateActualTolerance(phiP_nSec, refValueSec);
            Assert.LessOrEqual(actualToleranceSec, tolerance);
        }

        /// <summary>
        /// AISC DG 24
        /// Example 8.5—Gapped K-Connection with Square HSS and Unbalanced Branch Loads
        /// </summary>
        
        [Test]
        public void HssGappedKConnectionReturnsYieldingOfBranchesFromUnevenDistributionValue()
        {
            SectionTube ch = new SectionTube(null, 12, 12, 0.625, 0.581);
            SteelMaterial mat = new SteelMaterial(46.0);
            SteelRhsSection Chord = new SteelRhsSection(ch, mat);

            SectionTube mainBranch = new SectionTube(null, 8, 8, 0.375, 0.349);
            SteelRhsSection MainBranch = new SteelRhsSection(mainBranch, mat);

            SectionTube secBranch = new SectionTube(null, 8, 8, 0.375, 0.349);
            SteelRhsSection SecondaryBranch = new SteelRhsSection(secBranch, mat);


            IHssTrussBranchConnection con = new RhsTrussGappedKConnection(Chord, MainBranch, 45,
                SecondaryBranch, 45,
                AxialForceType.Compression, AxialForceType.Tension, false, 430, 0
                );
            //Note: not clear in the design guide why moment is ignored in chord utilization calculation
            double phiP_nMain = con.GetChordWallPlastificationStrength(true).Value;
            double refValueMain = 415;
            double actualToleranceMain = EvaluateActualTolerance(phiP_nMain, refValueMain);
            Assert.LessOrEqual(actualToleranceMain, tolerance);

            double phiP_nSec = con.GetBranchYieldingFromUnevenLoadDistributionStrength(false).Value;
            double refValueSec = 415;
            double actualToleranceSec = EvaluateActualTolerance(phiP_nSec, refValueSec);
            Assert.LessOrEqual(actualToleranceSec, tolerance);
        }

        public HssTrussRhsKConnectionTests()
        {
            tolerance = 0.05; //5% can differ from rounding 
        }


        double tolerance;




        
    }

}
