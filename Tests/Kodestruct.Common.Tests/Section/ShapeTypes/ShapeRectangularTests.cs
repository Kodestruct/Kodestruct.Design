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
    public class ShapeRectangularTests: ToleranceTestBase
    {
            public ShapeRectangularTests ()
	{

	

            tolerance = 0.05; //5% can differ from rounding
        }

        double tolerance;

        [Test]
        public void SectionRectangleReturnsCentroid()
        {
            SectionRectangular sr = new SectionRectangular(3, 2);
            sr.Centroid = new Mathematics.Point2D(0, 3);
            Assert.AreEqual(3, sr.GetElasticCentroidCoordinate().Y);
        }


        [Test]
        public void SectionRectangleReturnsFirstMomentOfArea()
        {
            SectionRectangular sr = new SectionRectangular(3, 4);
            double Q = sr.GetFirstMomentOfAreaX(1);
            double refVal = 1.0 * 3.0 * 1.5;
            double actualTolerance = EvaluateActualTolerance(Q, refVal);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void SectionRectangleReturnsPlasticMomentOfArea()
        {
            SectionRectangular sr = new SectionRectangular(0.5, 8);
            double Z = sr.Z_x;
            double refVal = 8.0;
            double actualTolerance = EvaluateActualTolerance(Z, refVal);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

}
}
