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
    /// <summary>
    /// Compare calculated properties to W18X35 listed properties.
    /// </summary>
     [TestFixture]
    public class SectionCompoundStackedTests : ToleranceTestBase
    {

         public SectionCompoundStackedTests()
        {
            tolerance = 0.06; //6% can differ from fillet areas and rounding in the manual
        }

        double tolerance;


        [Test]
        public void SectionCompoundStackedReturnsIy()
        {
            List<SectionRectangular> Rectangles = new List<SectionRectangular>()
             {
                 new SectionRectangular(6.0, 0.425),
                 new SectionRectangular(0.3,17.7-2.0*0.425),
                 new SectionRectangular(6.0, 0.425)
             };

            SectionCompoundStacked shape = new SectionCompoundStacked(Rectangles);

            double Iy = shape.I_y;
            double refValue = 15.3;
            double actualTolerance = EvaluateActualTolerance(Iy, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

         [Test]
        public void SectionCompoundStackedReturnsIx()
         {
             List<SectionRectangular>Rectangles = new List<SectionRectangular>()
             {
                 new SectionRectangular(6.0, 0.425),
                 new SectionRectangular(0.3,17.7-2.0*0.425),
                 new SectionRectangular(6.0, 0.425)
             };

             SectionCompoundStacked shape = new SectionCompoundStacked(Rectangles);

             double Ix = shape.I_x;
             double refValue = 500;
             double actualTolerance = EvaluateActualTolerance(Ix, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }

    }
}
