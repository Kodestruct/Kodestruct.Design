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
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection section = AiscShapeFactory.GetShape("HSS8X6X.500", ShapeTypeSteel.RectangularHSS);
            SteelMaterial mat = new SteelMaterial(46.0, 29000);
            beam = factory.GetBeam(section, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

        }
        [Test]
        public void RectangularHSSReturnsFlexuralYieldingStrength()
        {
            SteelLimitStateValue Y =
             beam.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 30.5 * 46 * 0.9; // Z_x*F_y*0.9
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
