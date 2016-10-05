using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Common.Section.SectionTypes;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Flexure
{

    [TestFixture]
    public class SolidShapeTests : ToleranceTestBase
    {
        public SolidShapeTests()
        {
            CreateBeam();
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;

        ISteelBeamFlexure beam { get; set; }
        private void CreateBeam()
        {
            FlexuralMemberFactory factory = new FlexuralMemberFactory();

            ISection section = new SectionRectangular(0.5, 30.0);
            SteelMaterial mat = new SteelMaterial(36.0, 29000);
            beam = factory.GetBeam(section, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

        }
        [Test]
        public void RectangularSolidShapeReturnsFlexuralYieldingStrength()
        {
            SteelLimitStateValue Y =
             beam.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 0.5*(30.0*30.0)/4.0 * 36 * 0.9; // Z_x*F_y*0.9
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
