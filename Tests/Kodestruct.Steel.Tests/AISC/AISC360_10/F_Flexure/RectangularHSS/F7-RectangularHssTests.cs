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
    public class RectangularHssTests : ToleranceTestBase
    {
        public RectangularHssTests()
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
            SteelMaterial mat = new SteelMaterial(46.0,29000);
            beam = factory.GetBeam(section,mat,null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

        }
        [Test]
        public void RectangularHSSReturnsFlexuralYieldingStrength                 () 
        {
            SteelLimitStateValue Y=
             beam.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 30.5*46*0.9; // Z_x*F_y*0.9
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void RectangularHSSReturnsFlexuralLateralTorsionalBucklingStrengthInelastic()
        {
            SteelLimitStateValue LTB =
            beam.GetFlexuralLateralTorsionalBucklingStrength(1.0, 9 * 12, FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = 192*12; // from AISC Steel Manual Table 3-10
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void RectangularHSSReturnsFlexuralLateralTorsionalBucklingStrengthElastic()
        {
            SteelLimitStateValue LTB =
            beam.GetFlexuralLateralTorsionalBucklingStrength(1.0, 14 * 12, FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = -1; // from AISC Steel Manual Table 3-10
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }


        public void RectangularHSSReturnsFlexuralFlangeLocalBucklingStrength()
        {
            SteelLimitStateValue FLB =
            beam.GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = FLB.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
         [Test]
        public void RectangularHSSReturnsFlexuralCompressionFlangeYieldingStrength()
        {
            SteelLimitStateValue CFY =
            beam.GetFlexuralCompressionFlangeYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = CFY.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
         [Test]
         public void RectangularHSSReturnsFlexuralTensionFlangeYieldingStrength()
        {
            SteelLimitStateValue TFY =
            beam.GetFlexuralTensionFlangeYieldingStrength( FlexuralCompressionFiberPosition.Top);
            double phiM_n = TFY.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
         [Test]
         public void RectangularHSSReturnsFlexuralWebOrWallBucklingStrength()
        {
            SteelLimitStateValue WLB =
            beam.GetFlexuralWebOrWallBucklingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = WLB.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
         [Test]
         public void RectangularHSSReturnsFlexuralLegOrStemBucklingStrength() 
        {
            SteelLimitStateValue LSLB =
            beam.GetFlexuralLegOrStemBucklingStrength(FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.FullLateralBracing);
            double phiM_n = LSLB.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
         [Test]
         public void RectangularHSSReturnsLimitingLengthForInelasticLTB_Lr() 
        {
            SteelLimitStateValue L_r =
            beam.GetLimitingLengthForInelasticLTB_Lr(FlexuralCompressionFiberPosition.Top);
            double phiM_n = L_r.Value;
            double refValue = -1; // AISC manual table 3.2
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
         [Test]
         public void RectangularHSSReturnsLimitingLengthForFullYielding_Lp() 
        {
            SteelLimitStateValue L_p =
            beam.GetLimitingLengthForFullYielding_Lp(FlexuralCompressionFiberPosition.Top);
            double phiM_n = L_p.Value;
            double refValue = -1; // AISC manual table 3.2
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
