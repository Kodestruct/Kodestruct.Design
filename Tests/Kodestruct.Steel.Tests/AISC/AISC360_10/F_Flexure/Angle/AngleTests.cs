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
    public class AngleTests : ToleranceTestBase
    {
        public AngleTests()
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
            ISection section = AiscShapeFactory.GetShape("L6X6X5/16", ShapeTypeSteel.Angle);
            SteelMaterial mat = new SteelMaterial(36.0,29000);
            beam = factory.GetBeam(section,mat,null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

        }

        //AISC Night School February 2, 2016
        //Steel Design 2: Selected Topics Session 1: Introduction and Single Angle Flexural Members
        //Example 1


        [Test]
        public void AngleReturnsFlexuralYieldingStrength() 
        {
            SteelLimitStateValue Y=
             beam.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 159.0*0.9;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        //AISC Night School February 2, 2016
        //Steel Design 2: Selected Topics Session 1: Introduction and Single Angle Flexural Members
        //Example 2

        [Test]
        public void AngleReturnsFlexuralLTBStrength()
        {
            SteelLimitStateValue LTB =
             beam.GetFlexuralLateralTorsionalBucklingStrength(1.0,8*12,FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = 88.9;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }


        //AISC Night School February 2, 2016
        //Steel Design 2: Selected Topics Session 1: Introduction and Single Angle Flexural Members
        //Example 2

        [Test]
        public void AngleReturnsFlexuralLLBStrength()
        {
            SteelLimitStateValue LLB =
             beam.GetFlexuralLegOrStemBucklingStrength(FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LLB.Value;
            double refValue = 107.3*0.9;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

    }
}
