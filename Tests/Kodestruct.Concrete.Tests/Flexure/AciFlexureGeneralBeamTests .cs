 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14.Materials;

namespace Kodestruct.Concrete.ACI318_14.Tests.Flexure
{
    [TestFixture]
    public partial class AciFlexureRectangularBeamTests: ToleranceTestBase
    {
        /// <summary>
        /// Wight. Reinforced concrete. 7th edition
        /// </summary>
        [Test]
        public void GeneralBeamFlexuralCapacityTopReturnsNominalValue()
        {
            double fc = 4000.0;
            FlexuralSectionFactory sf = new FlexuralSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(fc);
            List<Point2D> PolyPoints = new List<Point2D>()
            {
                new Point2D(-6.0,0.0),
                new Point2D(-6.0,12.0),
                new Point2D(6.0,12.0),
                new Point2D(6.0,0)
            };

            Rebar thisBar = new Rebar(1.0, new MaterialAstmA615(A615Grade.Grade60));
            var coord = new RebarCoordinate(0,1.0);
            List<RebarPoint> RebarPoints = new List<RebarPoint>()
            {
                new RebarPoint(thisBar,coord)
            };
            //GetGeneralSection(List<Point2D> PolygonPoints, 
            //ConcreteMaterial Concrete, List<RebarPoint> RebarPoints, double b_w, double d)
            ConcreteSectionFlexure beam = sf.GetGeneralSection(PolyPoints, mat, RebarPoints, 12.0, 9.0);
            IStrainCompatibilityAnalysisResult MResult = beam.GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition.Top);
            double M_n = MResult.Moment / 12000;

            double refValue = 51.32;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

   
    }
}
