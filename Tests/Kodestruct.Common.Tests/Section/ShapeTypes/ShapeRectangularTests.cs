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

}
}
