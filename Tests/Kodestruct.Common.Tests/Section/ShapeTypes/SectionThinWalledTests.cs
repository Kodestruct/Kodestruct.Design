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
         public void SectionThinWallGeneralReturnsI_y()
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

         [Test]
         public void SectionThinWallReturnsIxMoved()
         {
             List<ThinWallSegment> segments = new List<ThinWallSegment>()
             {
                 new ThinWallSegment(new Line2D(new Point2D(-440.05,98.27), new Point2D(-424.15, 98.27)),1.77),
                 new ThinWallSegment(new Line2D(new Point2D(-432.1,98.27), new Point2D(-432.1,56.04)),1.07),
                 new ThinWallSegment(new Line2D(new Point2D(-424.15,56.04), new Point2D(-440.05,56.04)),1.77)
             };


             SectionThinWall shape = new SectionThinWall(segments);
             double Ix = shape.I_x;
             double refValue = 31100.00;
             double actualTolerance = EvaluateActualTolerance(Ix, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }




         [Test]
         public void SectionThinWallReturnsIyMoved()
         {
             List<ThinWallSegment> segments = new List<ThinWallSegment>()
             {
                 new ThinWallSegment(new Line2D(new Point2D(-440.05,98.27), new Point2D(-424.15, 98.27)),1.77),
                 new ThinWallSegment(new Line2D(new Point2D(-432.1,98.27), new Point2D(-432.1,56.04)),1.07),
                 new ThinWallSegment(new Line2D(new Point2D(-424.15,56.04), new Point2D(-440.05,56.04)),1.77)
             };


             SectionThinWall shape = new SectionThinWall(segments);
             double Iy = shape.I_y;
             double refValue = 1200.00;
             double actualTolerance = EvaluateActualTolerance(Iy, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }


         [Test]
         public void SectionThinWallReturnsIxMoved1()
         {
             List<ThinWallSegment> segments = new List<ThinWallSegment>()
             {
                 new ThinWallSegment(new Line2D(new Point2D(-27.79 *12.0, 10.742 *12.0), new Point2D(   -27.79   *12.0, 7.075  *12.0)),1.77),
                 new ThinWallSegment(new Line2D(new Point2D(-28.415*12.0, 10.742 *12.0), new Point2D(   -27.165  *12.0, 10.742 *12.0)),1.07),
                 new ThinWallSegment(new Line2D(new Point2D(-28.415*12.0, 7.075  *12.0), new Point2D(    -27.165 *12.0, 7.075  *12.0)),1.77)
             };


             SectionThinWall shape = new SectionThinWall(segments);
             double Ix = shape.I_x;
             double refValue = 31100.00;
             double actualTolerance = EvaluateActualTolerance(Ix, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }

         [Test]
         public void SectionThinWallReturnsIxMoved2()
         {
             List<ThinWallSegment> segments = new List<ThinWallSegment>()
             {
                 new ThinWallSegment(new Line2D(new Point2D(-36.009*12.0,8.108 *12.0), new Point2D(  -36.009*12.0, 4.754*12.0)),1.03),
                 new ThinWallSegment(new Line2D(new Point2D(-36.671*12.0, 8.190*12.0), new Point2D(  -35.346*12.0, 8.190*12.0)),1.77),
                 new ThinWallSegment(new Line2D(new Point2D(-36.671*12.0, 4.670*12.0), new Point2D(  -35.346*12.0, 4.670*12.0)),1.77)
             };


             SectionThinWall shape = new SectionThinWall(segments);
             double Ix = shape.I_x;
             double refValue = 31100.00;
             double actualTolerance = EvaluateActualTolerance(Ix, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }
    }
}
