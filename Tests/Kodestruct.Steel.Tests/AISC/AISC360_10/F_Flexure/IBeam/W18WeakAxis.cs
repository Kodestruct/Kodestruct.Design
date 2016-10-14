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
    public partial class DoublySymmetricICompactTests : ToleranceTestBase
    {

        [Test]
        public void DoublySymmetricIReturnsFlexuralMinorAxisYieldingStrength                 () 
        {
            FlexuralMemberFactory factory = new FlexuralMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection section = AiscShapeFactory.GetShape("W18X35", ShapeTypeSteel.IShapeRolled);
            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            beam = factory.GetBeam(section, mat, null, MomentAxis.YAxis, FlexuralCompressionFiberPosition.Top);


            SteelLimitStateValue Y=
             beam.GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition.Top);
            double phiM_n = Y.Value;
            double refValue = 0.9 * Math.Min(50.0 * 8.06, 1.6 * 50.0 * 5.12); 
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

    }
}
