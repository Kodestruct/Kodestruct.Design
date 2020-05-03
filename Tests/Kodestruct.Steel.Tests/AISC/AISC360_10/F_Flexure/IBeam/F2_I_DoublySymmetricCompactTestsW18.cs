#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion
 

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
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Flexure
{

    //[TestFixture]
    public partial class DoublySymmetricICompactTests : ToleranceTestBase
    {
        public DoublySymmetricICompactTests()
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
            ISection section = AiscShapeFactory.GetShape("W18X35", ShapeTypeSteel.IShapeRolled);
            SteelMaterial mat = new SteelMaterial(50.0,29000);
            beam = factory.GetBeam(section,mat,null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

        }
     [Fact]
        public void DoublySymmetricIReturnsFlexuralYieldingStrength                 () 
        {
            SteelLimitStateValue Y=
             beam.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 249*12.0; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void DoublySymmetricIReturnsFlexuralLateralTorsionalBucklingStrengthInelastic ()
        {
            SteelLimitStateValue LTB =
            beam.GetFlexuralLateralTorsionalBucklingStrength(1.0, 9 * 12, FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = 192*12; // from AISC Steel Manual Table 3-10
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void DoublySymmetricIReturnsFlexuralLateralTorsionalBucklingStrengthElastic()
        {
            SteelLimitStateValue LTB =
            beam.GetFlexuralLateralTorsionalBucklingStrength(1.0, 14 * 12, FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = 123*12; // from AISC Steel Manual Table 3-10
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }


        public void DoublySymmetricIReturnsFlexuralFlangeLocalBucklingStrength      ()
        {
            SteelLimitStateValue FLB =
            beam.GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = FLB.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }
      [Fact]
        public void DoublySymmetricIReturnsFlexuralCompressionFlangeYieldingStrength()
        {
            SteelLimitStateValue CFY =
            beam.GetFlexuralCompressionFlangeYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = CFY.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.True(actualTolerance<=tolerance);
        }
      [Fact]
        public void DoublySymmetricIReturnsFlexuralTensionFlangeYieldingStrength    ()
        {
            SteelLimitStateValue TFY =
            beam.GetFlexuralTensionFlangeYieldingStrength( FlexuralCompressionFiberPosition.Top);
            double phiM_n = TFY.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }
      [Fact]
        public void DoublySymmetricIReturnsFlexuralWebOrWallBucklingStrength        ()
        {
            SteelLimitStateValue WLB =
            beam.GetFlexuralWebOrWallBucklingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = WLB.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }
      [Fact]
        public void DoublySymmetricIReturnsFlexuralLegOrStemBucklingStrength        () 
        {
            SteelLimitStateValue LSLB =
            beam.GetFlexuralLegOrStemBucklingStrength(FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.FullLateralBracing);
            double phiM_n = LSLB.Value;
            double refValue = -1; // Limit state not applicable
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }
      [Fact]
        public void DoublySymmetricIReturnsLimitingLengthForInelasticLTB_Lr         () 
        {
            SteelLimitStateValue L_r =
            beam.GetLimitingLengthForInelasticLTB_Lr(FlexuralCompressionFiberPosition.Top);
            double phiM_n = L_r.Value;
            double refValue = 12.3*12; // AISC manual table 3.2
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }
      [Fact]
        public void DoublySymmetricIReturnsLimitingLengthForFullYielding_Lp() 
        {
            SteelLimitStateValue L_p =
            beam.GetLimitingLengthForFullYielding_Lp(FlexuralCompressionFiberPosition.Top);
            double phiM_n = L_p.Value;
            double refValue = 4.31*12; // AISC manual table 3.2
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }
    }
}
