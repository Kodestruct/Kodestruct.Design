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
using System;


namespace Kodestruct.Common.Tests.Section.ShapeTypes
{
    [TestFixture]
    public class PolygonShapeTests : ToleranceTestBase
    {
        public PolygonShapeTests()
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
            var rect = new PolygonShape(Points);
            IMoveableSection sect = rect.GetTopSliceSection(1, SlicingPlaneOffsetType.Top);
            Assert.AreEqual(1.0, sect.YMax);
        }

        [Test]
        public void PolygonShapeCalculatesMomentOfInertia()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, -2),
                new Point2D(-1, 2),
                new Point2D(1, 2),
                new Point2D(1, -2)
            };
            var rect = new PolygonShape(Points);
            double refValue = 2.0 * System.Math.Pow(4.0, 3.0) / 12.0;
            double I_x = rect.I_x;


            double actualTolerance = EvaluateActualTolerance(I_x, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void PolygonShapeCalculatesMomentOfInertiaWithShiftedCoordinates()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, 0),
                new Point2D(-1, 4),
                new Point2D(1, 4),
                new Point2D(1, 0)
            };
            var rect = new PolygonShape(Points);
            double refValue = 2.0 * System.Math.Pow(4.0, 3.0) / 12.0;
            double I_x = rect.I_x;


            double actualTolerance = EvaluateActualTolerance(I_x, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void PolygonShapeCalculatesYMaxAndYmin()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, 0),
                new Point2D(-1, 4),
                new Point2D(1, 4),
                new Point2D(1, 0)
            };
            var rect = new PolygonShape(Points);
            double refValueMax = 4.0;
            double Ymax = rect.YMax;


            double actualTolerance1 = EvaluateActualTolerance(Ymax, refValueMax);
            Assert.LessOrEqual(actualTolerance1, tolerance);

            double refValueMin = 0.0;
            double YMin = rect.YMin;


            double actualTolerance2 = EvaluateActualTolerance(YMin, refValueMin);
            Assert.LessOrEqual(actualTolerance2, tolerance);

        }

        [Test]
        public void PolygonShapeCalculates_y_bar()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, 0),
                new Point2D(-1, 4),
                new Point2D(1, 4),
                new Point2D(1, 0)
            };
            var rect = new PolygonShape(Points);
            double refValue = 2.0;
            double y_bar = rect.y_Bar;


            double actualTolerance = EvaluateActualTolerance(y_bar, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);


        }

        [Test]
        public void PolygonShapeCalculatesArea()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, 0),
                new Point2D(-1, 4),
                new Point2D(1, 4),
                new Point2D(1, 0)
            };
            var rect = new PolygonShape(Points);
            double refValue = 8.0;
            double A = rect.A;


            double actualTolerance = EvaluateActualTolerance(A, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);


        }


        [Test]
        public void PolygonShapeCalculatesZx()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, 0),
                new Point2D(-1, 4),
                new Point2D(1, 4),
                new Point2D(1, 0)
            };
            var rect = new PolygonShape(Points);
            double refValue = 2.0*Math.Pow(4,2)/4.0;
            double Zx = rect.Z_x;


            double actualTolerance = EvaluateActualTolerance(Zx, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);


        }

        [Test]
        public void PolygonShapeCalculatesZy()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, 0),
                new Point2D(-1, 4),
                new Point2D(1, 4),
                new Point2D(1, 0)
            };
            var rect = new PolygonShape(Points);
            double refValue = 4.0 * Math.Pow(2, 2) / 4.0;
            double Zy = rect.Z_y;


            double actualTolerance = EvaluateActualTolerance(Zy, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);


        }

        [Test]
        public void PolygonShapeReturnsTopSliceOfArea1()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-1, 0),
                new Point2D(-1, 4),
                new Point2D(1, 4),
                new Point2D(1, 0)
            };
            var rect = new PolygonShape(Points);
            var slicedShape = rect.GetTopSliceOfArea(2.0);

            double refValue = 2.0;
            double A = slicedShape.A;


            double actualTolerance = EvaluateActualTolerance(A, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);


        }

        [Test]
        public void PolygonShapeReturnsTopSliceOfArea2()
        {
            var Points = new List<Point2D>
            {
                new Point2D(-6.0, 0),
                new Point2D(-6.0, 12.0),
                new Point2D(6.0, 12.0),
                new Point2D(6.0, 0)
            };
            var rect = new PolygonShape(Points);
            var slicedShape = rect.GetTopSliceOfArea(17.64);

            double refValue = 1.47;
            double y = slicedShape.YMax-slicedShape.YMin;


            double actualTolerance = EvaluateActualTolerance(y, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);


        }

        //compressedSection = this.Section.SliceableShape.GetTopSliceOfArea(RequiredConcreteArea);
    }
}