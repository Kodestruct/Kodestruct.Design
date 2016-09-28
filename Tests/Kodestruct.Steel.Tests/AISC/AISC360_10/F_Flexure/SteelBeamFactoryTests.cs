using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Flexure
{

    [TestFixture]
    public partial class SteelBeamFactoryTests : ToleranceTestBase
    {
        public SteelBeamFactoryTests()
        {

            tolerance = 0.02; //2% can differ from rounding in the manual
        }
         double tolerance;

               
        [Test]
        public void DoublySymmetricIReturnsLateralTorsionalStrength () 
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection r = new  SectionI("", 12.2, 6.49, 0.38, 0.23);


            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

            SteelLimitStateValue LTB =
            beam12.GetFlexuralLateralTorsionalBucklingStrength(1.0, 19 * 12, FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = 60 * 12; // from AISC Steel Manual Table 3-10
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);


            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void IShapeReturnsYieldStrength()
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection r = AiscShapeFactory.GetShape("W18X35");


            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

            SteelLimitStateValue Y =
            beam12.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 249 * 12; // from AISC Steel Manual Table 3-2
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);


            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void RectangleShapeReturnsYieldStrength()
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            SectionRectangular r = new SectionRectangular(0.5, 12.0);


            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

            SteelLimitStateValue Y =
            beam12.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 67.5; 
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);


            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void FactoryReturnsChannel()
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISectionChannel r = new SectionChannel("", 12.0, 6, 0.4, 0.25);


            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

            Assert.IsTrue(true);
        }
    }
}
