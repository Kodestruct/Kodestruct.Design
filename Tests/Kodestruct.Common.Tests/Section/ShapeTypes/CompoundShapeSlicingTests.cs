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
    public partial class CompoundShapeTests : ToleranceTestBase
    {
        private ArbitraryCompoundShape GetCustomTeeShape()
        {

            List<CompoundShapePart> rectX = new List<CompoundShapePart>()
            {
                new CompoundShapePart(0.5,6, new Point2D(0,3)),
                new CompoundShapePart(5,1, new Point2D(0,6.5)),
            };
            ArbitraryCompoundShape shape = new ArbitraryCompoundShape(rectX, null);

            return shape;
        }

        [Test]
        public void CompoundShapeReturnsSlicePlaneLocation()
        {
            ArbitraryCompoundShape shape = GetCustomTeeShape();
            double YPlane = shape.GetSlicePlaneTopOffset(6.25);
            Assert.AreEqual(3.5, YPlane);
        }

        [Test]
        public void CompoundShapeReturnsTopSliceOfArea()
        {
            ArbitraryCompoundShape shape = GetCustomTeeShape();
            IMoveableSection topSlice = shape.GetTopSliceOfArea(6.25);
            Assert.AreEqual(6.25, topSlice.A);
            Assert.AreEqual(3.5, topSlice.YMin);
        }

        [Test]
        public void CompoundShapeReturnsBottomSliceOfArea()
        {
            CompoundShape shape = GetRectangularShape();
            IMoveableSection bottomSlice = shape.GetBottomSliceOfArea(6);
            Assert.AreEqual(6, bottomSlice.A);
            Assert.AreEqual(-5.5, bottomSlice.YMax);
        }

        [Test]
        public void CompoundShapeReturnsTopSliceAtCoordinate()
        {
            CompoundShape shape = GetRectangularShape();
            IMoveableSection topSlice = shape.GetTopSliceSection(2, SlicingPlaneOffsetType.Top);
            Assert.AreEqual(24, topSlice.A);
            Assert.AreEqual(4, topSlice.YMin);
        }

        [Test]
        public void CompoundShapeReturnsBottomSliceAtCoordinate()
        {
            CompoundShape shape = GetRectangularShape();
            IMoveableSection topSlice = shape.GetBottomSliceSection(2, SlicingPlaneOffsetType.Bottom);
            Assert.AreEqual(24, topSlice.A);
            Assert.AreEqual(-4, topSlice.YMax);
        }

        private CompoundShape GetRectangularShape()
        {
            SectionRectangular rect = new SectionRectangular(12, 12);
            return rect;
        }


        [Test]
        public void CompoundShapeReturnsTopSLiceAtOffset()
        {
            ArbitraryCompoundShape shape = GetCustomTeeShape();
            IMoveableSection topSLice = shape.GetTopSliceSection(3.5, SlicingPlaneOffsetType.Top);
            Assert.AreEqual(6.25, topSLice.A);
        }
        //GetTopSliceSection
    }
}
