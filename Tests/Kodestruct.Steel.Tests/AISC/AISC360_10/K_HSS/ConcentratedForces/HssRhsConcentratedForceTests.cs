using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections;
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces;

namespace Kodestruct.Steel.Tests.AISC.AISC36010.HSSTrussConnections
{

    [TestFixture]
    public class HssRhsConcentratedForceTests : ToleranceTestBase
    {

        /// <summary>
        /// AISC DG 24
        /// Example 8.4—Overlapped K-Connection with Rectangular HSS
        /// </summary>
        [Test]
        public void HssRhsConcentratedForceThroughPlateReturnsValue()
        {
            SectionTube ch = new SectionTube(null, 8, 8, 0.25,0.93*0.25,1.5*0.25);
            SteelMaterial matE = new SteelMaterial(46.0);
            SteelRhsSection Element = new SteelRhsSection(ch, matE);

            SectionRectangular rec = new SectionRectangular(0.25,8.0);
            SteelMaterial matR = new SteelMaterial(36.0);
            SteelPlateSection pl = new SteelPlateSection(rec,matR);

            CalcLog log = new CalcLog();

            RhsLongitudinalThroughPlate concForceConnection = new RhsLongitudinalThroughPlate(Element, pl, log, false,45.0, 148.0, 0.0);
            double phiR_n = concForceConnection.GetHssWallPlastificationStrengthUnderAxialLoad().Value;

            double refValueSec = 46.2;
            double actualToleranceSec = EvaluateActualTolerance(phiR_n, refValueSec);
            Assert.LessOrEqual(actualToleranceSec, tolerance);
        }



        public HssRhsConcentratedForceTests()
        {
            tolerance = 0.05; //5% can differ from rounding 
        }


        double tolerance;




        
    }

}
