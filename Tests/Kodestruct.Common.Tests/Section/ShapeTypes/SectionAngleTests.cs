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
    public class SectionAngleTests: ToleranceTestBase
    {

        public SectionAngleTests()
        {
            tolerance = 0.05; //5% can differ from rounding
        }

        double tolerance;

        //From Geshwinder Night School. Steel Design 2: Selected Topics 2016
        //Session 1. Page 26
        [Test]
        public void SectionAngle6X4X1_2Returns_y_bar()
        {
            SectionAngle angle = new SectionAngle("", 6, 4, 0.5, AngleRotation.FlatLegBottom, AngleOrientation.LongLegVertical);
            double y_bar = angle.y_Bar;
            double refValue = 1.987;
             
             double actualTolerance = EvaluateActualTolerance(y_bar, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionAngle6X4X1_2Returns_x_bar()
        {
            SectionAngle angle = new SectionAngle("", 6, 4, 0.5, AngleRotation.FlatLegBottom, AngleOrientation.LongLegVertical);
            double x_bar = angle.CentroidYAxisRect;
            double refValue = 0.9868;

            double actualTolerance = EvaluateActualTolerance(x_bar, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionAngle6X4X1_2Returns_I_x()
        {
            SectionAngle angle = new SectionAngle("", 6, 4, 0.5, AngleRotation.FlatLegBottom, AngleOrientation.LongLegVertical);
            double I_x = angle.I_x;
            double refValue = 17.36;

            double actualTolerance = EvaluateActualTolerance(I_x, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionAngle6X4X1_2Returns_I_y()
        {
            SectionAngle angle = new SectionAngle("", 6, 4, 0.5, AngleRotation.FlatLegBottom, AngleOrientation.LongLegVertical);
            double I_y = angle.I_y;
            double refValue = 6.27;

            double actualTolerance = EvaluateActualTolerance(I_y, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionAngle6X4X1_2Returns_alpha()
        {
            SectionAngle angle = new SectionAngle("", 6, 4, 0.5, AngleRotation.FlatLegBottom, AngleOrientation.LongLegVertical);
            double alpha = angle.Angle_alpha;
            double refValue = 23.76;

            double actualTolerance = EvaluateActualTolerance(alpha, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionAngle6X4X1_2Returns_I_w()
        {
            SectionAngle angle = new SectionAngle("", 6, 4, 0.5, AngleRotation.FlatLegBottom, AngleOrientation.LongLegVertical);
            double I_w = angle.I_w;
            double refValue = 20.02;

            double actualTolerance = EvaluateActualTolerance(I_w, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionAngle6X4X1_2Returns_I_z()
        {
            SectionAngle angle = new SectionAngle("", 6, 4, 0.5, AngleRotation.FlatLegBottom, AngleOrientation.LongLegVertical);
            double I_z = angle.I_z;
            double refValue = 3.60;

            double actualTolerance = EvaluateActualTolerance(I_z, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionAngle6X4X1_2Returns_beta_w()
        {
            SectionAngle angle = new SectionAngle("", 6, 4, 0.5, AngleRotation.FlatLegBottom, AngleOrientation.LongLegVertical);
            double beta_w = angle.beta_w;
            double refValue = 3.14;

            double actualTolerance = EvaluateActualTolerance(beta_w, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
