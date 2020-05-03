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

    // 
    public class AngleTests : ToleranceTestBase
    {
        public AngleTests()
        {
            CreateBeam();
            tolerance = 0.03; //3% can differ from rounding in the manual
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


     [Fact]
        public void AngleReturnsFlexuralYieldingStrength() 
        {
            SteelLimitStateValue Y=
             beam.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 159.0*0.9;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

        //AISC Night School February 2, 2016
        //Steel Design 2: Selected Topics Session 1: Introduction and Single Angle Flexural Members
        //Example 2

     [Fact]
        public void AngleReturnsFlexuralLTBStrength()
        {
            SteelLimitStateValue LTB =
             beam.GetFlexuralLateralTorsionalBucklingStrength(1.0,8*12,FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = 88.9;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }


        //AISC Night School February 2, 2016
        //Steel Design 2: Selected Topics Session 1: Introduction and Single Angle Flexural Members
        //Example 2

     [Fact]
        public void AngleReturnsFlexuralLLBStrength()
        {
            SteelLimitStateValue LLB =
             beam.GetFlexuralLegOrStemBucklingStrength(FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LLB.Value;
            double refValue = 107.3*0.9;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void AngleReturnsFlexuralLTBStrengthL3X3()
        {
            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection section = AiscShapeFactory.GetShape("L3X3X3/8", ShapeTypeSteel.Angle);
            SteelMaterial mat = new SteelMaterial(36.0, 29000);
            ISteelBeamFlexure thisBeam = factory.GetBeam(section, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

            SteelLimitStateValue LTB =
             beam.GetFlexuralLateralTorsionalBucklingStrength(1.0, 4.5 * 12, FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = 88.9;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<=tolerance);
        }

    }
}
