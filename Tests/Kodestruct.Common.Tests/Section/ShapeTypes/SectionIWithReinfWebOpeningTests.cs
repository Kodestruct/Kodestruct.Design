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
using Kodestruct.Common.Section.SectionTypes;

namespace Kodestruct.Common.Tests.Section.ShapeTypes
{

    [TestFixture]
    public class SectionIWithReinfWebOpeningTests : ToleranceTestBase
    {

        public SectionIWithReinfWebOpeningTests()
        {
            tolerance = 0.05; //5% can differ from rounding

        }

        double tolerance;

        [Test]
        public void SectionIWithReinfWebOpeningReturnsI_x()
        {
            SectionIWithReinfWebOpening shape = new SectionIWithReinfWebOpening(null, 18.0,8.0,1.0,0.5,8.0,0,2.0,0.5);
            double I_x = shape.I_x;
            double refValue = 1379.0 ; //Calculated in Autocad
            double actualTolerance = EvaluateActualTolerance(I_x, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionIWithOneSidedReinfWebOpeningReturnsI_x()
        {
            SectionIWithReinfWebOpening shape = new SectionIWithReinfWebOpening(null, 18.11, 7.53, 0.63, 0.39, 11.0, 0, 1.75, 0.375,true);
            double I_x = shape.I_x;
            double refValue = 879.0; //Calculated in Autocad
            double actualTolerance = EvaluateActualTolerance(I_x, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionIWithReinfWebOpeningReturnsI_y()
        {
            SectionIWithReinfWebOpening shape = new SectionIWithReinfWebOpening(null, 18.0, 8.0, 1.0, 0.5, 8.0, 0, 2.0, 0.5);
            double I_y = shape.I_y;
            double refValue = 93.0; //Calculated in Autocad
            double actualTolerance = EvaluateActualTolerance(I_y, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionIWithReinfWebOpeningReturnsZ_x()
        {
            SectionIWithReinfWebOpening shape = new SectionIWithReinfWebOpening(null, 18.0, 8.0, 1.0, 0.5, 8.0, 0, 2.0, 0.5);
            double Z_x = shape.Z_x;
            double refValue = 2 * 12 * 7.3750; //Calculated in Autocad
            double actualTolerance = EvaluateActualTolerance(Z_x, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }


    }
}
