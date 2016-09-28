using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Section.SectionTypes;

namespace Kodestruct.Common.Tests.Section.ShapeTypes
{
    [TestFixture]
    public class SteelRhsSectionTests : ToleranceTestBase
    {

        public SteelRhsSectionTests()
        {
            tolerance = 0.05; //5% can differ from fillet areas 
        }


        [Test]
        public void SteelRhsSectionReturnsDesignWall()
        {
            SectionTube secChord = new SectionTube(null, 8, 8, 1 / 4.0, 0.93 * (1 / 4.0), 0.35);
            double t_des = secChord.t_des;
            double refValue = 0.2325;
            double actualTolerance = EvaluateActualTolerance(t_des, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SteelRhsSectionReturnsArea()
        {
            SectionTube secChord = new SectionTube(null, 8, 8, 1 / 4.0, 0.93 * (1 / 4.0), 0.35);
            double A = secChord.A;
            double refValue = 7.1;
            double actualTolerance = EvaluateActualTolerance(A, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
        double tolerance;
    }
}
