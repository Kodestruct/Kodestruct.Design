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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section;

namespace Kodestruct.Common.Tests.Section.ShapeTypes
{
    /// <summary>
    /// Compare calculated properties to W18X35 listed properties.
    /// </summary>
     [TestFixture]
    public class SectionThinWalledTests : ToleranceTestBase
    {

         public SectionThinWalledTests()
        {
            tolerance = 0.06; //6% can differ from fillet areas and rounding and thin wall approximation
        }

        double tolerance;


         //[Test]
         //public void SectionIRolledReturnsIy()
         //{
         //    SectionI shape = new SectionI("", 17.7, 6.0, 0.425, 0.3);
         //    double Iy = shape.I_y;
         //    double refValue = 15.3;
         //    double actualTolerance = EvaluateActualTolerance(Iy, refValue);
         //    Assert.LessOrEqual(actualTolerance, tolerance);
         //}

         [Test]
         public void SectionThinWallReturnsIx()
         {
             List<ThinWallSegment> segments = new List<ThinWallSegment>()
             {
                 new ThinWallSegment(new Line2D(new Point2D(-6.0/2.0,(17.7-0.425)/2.0), new Point2D(6.0/2.0,(17.7-0.425)/2.0)),0.425),
                 new ThinWallSegment(new Line2D(new Point2D(-6.0/2.0,-(17.7-0.425)/2.0), new Point2D(6.0/2.0,-(17.7-0.425)/2.0)),0.425),
                 new ThinWallSegment(new Line2D(new Point2D(0,-(17.7-0.425)/2.0), new Point2D(0,(17.7-0.425)/2.0)),0.3)
             };


             SectionThinWall shape = new SectionThinWall(segments);
             double Ix = shape.I_x;
             double refValue = 510;
             double actualTolerance = EvaluateActualTolerance(Ix, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }

         [Test]
         public void SectionThinWallReturnsIy()
         {
             List<ThinWallSegment> segments = new List<ThinWallSegment>()
             {
                 new ThinWallSegment(new Line2D(new Point2D(-6.0/2.0,(17.7-0.425)/2.0), new Point2D(6.0/2.0,(17.7-0.425)/2.0)),0.425),
                 new ThinWallSegment(new Line2D(new Point2D(-6.0/2.0,-(17.7-0.425)/2.0), new Point2D(6.0/2.0,-(17.7-0.425)/2.0)),0.425),
                 new ThinWallSegment(new Line2D(new Point2D(0,-(17.7-0.425)/2.0), new Point2D(0,(17.7-0.425)/2.0)),0.3)
             };


             SectionThinWall shape = new SectionThinWall(segments);
             double Iy = shape.I_y;
             double refValue = 15.3;
             double actualTolerance = EvaluateActualTolerance(Iy, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }

         [Test]
         public void SectionThinWallReturnsIyMoved()
         {
             List<ThinWallSegment> segments = new List<ThinWallSegment>()
             {
                 new ThinWallSegment(new Line2D(new Point2D(-6.0/2.0,(17.7-0.425)/2.0+6), new Point2D(6.0/2.0,(17.7-0.425)/2.0+6)),0.425),
                 new ThinWallSegment(new Line2D(new Point2D(-6.0/2.0,-(17.7-0.425)/2.0+6), new Point2D(6.0/2.0,-(17.7-0.425)/2.0+6)),0.425),
                 new ThinWallSegment(new Line2D(new Point2D(0,-(17.7-0.425)/2.0+6), new Point2D(0,(17.7-0.425)/2.0+6)),0.3)
             };


             SectionThinWall shape = new SectionThinWall(segments);
             double Iy = shape.I_y;
             double refValue = 15.3;
             double actualTolerance = EvaluateActualTolerance(Iy, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }
    }
}
