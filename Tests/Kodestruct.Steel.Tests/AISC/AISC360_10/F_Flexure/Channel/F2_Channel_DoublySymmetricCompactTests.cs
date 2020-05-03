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
    public partial class DoublySymmetricCompactChannelTests : ToleranceTestBase
    {
        public DoublySymmetricCompactChannelTests()
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
            ISection section = AiscShapeFactory.GetShape("C15X33.9", ShapeTypeSteel.Channel);
            SteelMaterial mat = new SteelMaterial(36.0, 29000);
            beam = factory.GetBeam(section, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top);

        }
     [Fact]
        public void DoublySymmetricChannelReturnsFlexuralYieldingStrength()
        {
            SteelLimitStateValue Y =
             beam.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 137.0*12; // from AISC  design examples 13 F.2-1b
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void DoublySymmetricChannelReturnsFlexuralLateralTorsionalBucklingStrengthInelastic()
        {
            SteelLimitStateValue LTB =
            beam.GetFlexuralLateralTorsionalBucklingStrength(1.0, 5 * 12, FlexuralCompressionFiberPosition.Top, Steel.AISC.FlexuralAndTorsionalBracingType.NoLateralBracing);
            double phiM_n = LTB.Value;
            double refValue = 131 * 12; // from AISC Steel Manual Table 3-10
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }



    }
}
