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

using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Steel.Tests.AISC.AISC360_10.F_Flexure.IBeam
{


    [TestFixture]
    public partial class F3_I_DoublySymmetricNonCompactTests : ToleranceTestBase
    {

        public F3_I_DoublySymmetricNonCompactTests()
        {
            CreateBeam();
            tolerance = 0.02; 
        }
        double tolerance;
        [Test]
        public void DoublySymmetricIReturnsFlangeLocalBucklingStrength()
        {
            SteelLimitStateValue FLB =
             beam.GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = FLB.Value;
            double refValue = 23398;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        ISteelBeamFlexure beam { get; set; }
        private void CreateBeam()
        {
            FlexuralMemberFactory factory = new FlexuralMemberFactory();

            ISection section = new SectionI(null, 13, 30, 1.5, 0.75);
            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            beam = factory.GetBeam(section, mat, null, MomentAxis.XAxis, FlexuralCompressionFiberPosition.Top,false);

        }
    }
}
