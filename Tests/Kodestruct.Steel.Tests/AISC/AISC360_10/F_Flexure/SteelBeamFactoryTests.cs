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
using Kodestruct.Common.Section.SectionTypes;
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
    public partial class SteelBeamFactoryTests : ToleranceTestBase
    {
        public SteelBeamFactoryTests()
        {

            tolerance = 0.02; //2% can differ from rounding in the manual
        }
         double tolerance;

               
     [Fact]
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


            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
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


            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void IShapeReturnsWeakAxisYieldStrength()
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection r = AiscShapeFactory.GetShape("W18X35");


            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.YAxis, FlexuralCompressionFiberPosition.Left);

            SteelLimitStateValue Y =
            beam12.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 0.9*8.06 * 50; // phi*Z_y*Fy
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);


            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void PipeShapeBeam()
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection r = AiscShapeFactory.GetShape("HSS2.5X.188");


            SteelMaterial mat = new SteelMaterial(35.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.YAxis, FlexuralCompressionFiberPosition.Left);

            Assert.True(beam12 !=null);
        }

     [Fact]
        public void RectangleShapeReturnsYieldStrength()
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            SectionRectangular r = new SectionRectangular(0.5, 12.0);


            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

            SteelLimitStateValue Y =
            beam12.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 810; 
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);


            Assert.True(actualTolerance<=tolerance);
        }

     [Fact]
        public void TubeReturnsWeakAxisYieldStrength()
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();

            ISection r = AiscShapeFactory.GetShape("HSS8X3X.375");


            SteelMaterial mat = new SteelMaterial(46.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.YAxis, FlexuralCompressionFiberPosition.Top);

            SteelLimitStateValue Y =
            beam12.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 326.23;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);


            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void FactoryReturnsChannel()
        {

            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISectionChannel r = new SectionChannel("", 12.0, 6, 0.4, 0.25);


            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            ISteelBeamFlexure beam12 = factory.GetBeam(r, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

            Assert.True(true);
        }
    }
}
