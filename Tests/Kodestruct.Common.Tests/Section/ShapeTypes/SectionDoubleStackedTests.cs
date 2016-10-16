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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Common.Section.SectionTypes;

namespace Kodestruct.Common.Tests.Section.ShapeTypes
{
    [TestFixture]
    public class SectionDoubleStackedTests : ToleranceTestBase
    {

        public SectionDoubleStackedTests()
        {
            tolerance = 0.05; //5% can differ from fillet areas and rounding in the manual
        }


        double tolerance;

        [Test]
        public void SectionDoubleStackedReturnsMomentOfInertia()
        {
            //W18X50 + C15X33.9
            //Table 1-19
            SectionI IShape = new SectionI(null, 18.2d,7.56d, 0.695d, 0.415d);
            SectionChannel Channel = new SectionChannel(null, 15.0d, 3.4d, 0.65d, 0.4d, true, true);
            SectionDoubleStacked shape = new SectionDoubleStacked(Channel, IShape, 0.4);
            double I_x = shape.I_x;
            double refValue1 = 1456.0; //from Autocad. Does not agree with Manual which is 1250

            double I_y = shape.I_y;
            double refValue2 = 363.6; //from Autocad. Does not agree with Manual which is 1250

            double actualTolerance1 = EvaluateActualTolerance(I_x, refValue1);
            double actualTolerance2 = EvaluateActualTolerance(I_y, refValue2);
            Assert.LessOrEqual(actualTolerance1, tolerance);
            Assert.LessOrEqual(actualTolerance2, tolerance);
        }
    }
}
