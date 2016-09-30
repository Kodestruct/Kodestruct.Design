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

using System.Collections.Generic;
using NUnit.Framework;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.General;
using Kodestruct.Common.Section.Interfaces;


namespace Kodestruct.Common.Tests.Section.ShapeTypes
{
    [TestFixture]
    public class GenericShapeTests : ToleranceTestBase
    {
        public GenericShapeTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }


        double tolerance;

        [Test]
        public void SliceTopReturnsPolygonForRectangle()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, -1),
                new Point2D(-1, 1),
                new Point2D(1, 1),
                new Point2D(1, -1)
            };
            var rect = new GenericShape(Points);
            IMoveableSection sect = rect.GetTopSliceSection(1, SlicingPlaneOffsetType.Top);
            Assert.AreEqual(1.0, sect.YMax);
        }

        [Test]
        public void GenericShapeCalculatesMomentOfInertia()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, -2),
                new Point2D(-1, 2),
                new Point2D(1, 2),
                new Point2D(1, -2)
            };
            var rect = new GenericShape(Points);
            double refValue = 2.0 * System.Math.Pow(4.0, 3.0) / 12.0;
            double I_x = rect.I_x;


            double actualTolerance = EvaluateActualTolerance(I_x, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
}