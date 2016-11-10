//Sample license text.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Concrete.ACI318_14.Tests.Shear
{
    [TestFixture]
    public partial class AciTwoWayShearTests : AciConcreteShearTestsBase
    {
        /// <summary>
        /// MacGregor, Wight. Reinforced concrete. 6th edition
        /// Example 13-11
        /// </summary>
        [Test]
        public void SlabPunchingConcentricReturnsValue()
        {
            IConcreteMaterial mat = this.GetConcreteMaterial(3000, false);
            PerimeterFactory f = new PerimeterFactory();
            double d = 4.75;
            double b_1 = 26.0;
            double b_2 = 12.0;
            Point2D ColumnCenter = new Point2D(0, 0);
            PunchingPerimeterData perimeter = f.GetPerimeterData(PunchingPerimeterConfiguration.Interior, b_1, b_2, d,0,0, ColumnCenter);
            ConcreteSectionTwoWayShear sec = new ConcreteSectionTwoWayShear(mat, perimeter, d, b_1, b_2, true, PunchingPerimeterConfiguration.Interior);
            double phi_v_c = sec.GetTwoWayStrengthForUnreinforcedConcrete()/1000.0; //convert to kips
            
            double refValue = 71.3/d/95.0; //from example
            double actualTolerance = EvaluateActualTolerance(phi_v_c, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        
        }
        /// <summary>
        /// MacGregor, Wight. Reinforced concrete. 6th edition
        /// Example 13-13
        /// </summary>
        [Test]
        public void SlabPunchingMomentAndShearReturnsStress()
        {
            IConcreteMaterial mat = this.GetConcreteMaterial(3000, false);
            PerimeterFactory f = new PerimeterFactory();
            double d = 5.5;
            double cx = 12.0;
            double cy = 16.0;
            Point2D ColumnCenter = new Point2D(0, 0);
            PunchingPerimeterData data = f.GetPerimeterData(PunchingPerimeterConfiguration.EdgeLeft, cx, cy, d, 4.0, 0.0,ColumnCenter);
            ConcreteSectionTwoWayShear sec = new ConcreteSectionTwoWayShear(mat, data, d, cx, cy, true, PunchingPerimeterConfiguration.Interior);
            double phi_v_c = sec.GetCombinedShearStressDueToMomementAndShear(0,599.7,31.3,false).v_max / 1000.0; //note: moment is adjusted to be at column centroid

            double refValue = 0.144; //from example
            double actualTolerance = EvaluateActualTolerance(phi_v_c, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);


        }

        /// <summary>
        /// ACI 421.1R-19  SHEAR REINFORCEMENT FOR SLABS 
        /// Example D.1
        /// </summary>
        [Test]
        public void InteriorSlabReturnsPerimeterJ_yProperty()
        {
            IConcreteMaterial mat = this.GetConcreteMaterial(3000, false);
            PerimeterFactory f = new PerimeterFactory();
            double d = 5.62;
            double cx = 12.0;
            double cy = 20.0;
            Point2D ColumnCenter = new Point2D(0, 0);
            PunchingPerimeterData data = f.GetPerimeterData(PunchingPerimeterConfiguration.Interior, cx, cy, d, 0.0, 0.0, ColumnCenter);
            ConcreteSectionTwoWayShear sec = new ConcreteSectionTwoWayShear(mat, data, d, cx, cy, true, PunchingPerimeterConfiguration.Interior);
            double J_y = sec.GetJy(sec.AdjustedSegments);

            double refValue = 27474; //from example  (page 19)
            double actualTolerance = EvaluateActualTolerance(J_y, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// ACI 421.1R-19  SHEAR REINFORCEMENT FOR SLABS 
        /// Example D.1
        /// </summary>
        [Test]
        public void InteriorSlabReturnsPerimeter_gamma_vy_Property()
        {
            IConcreteMaterial mat = this.GetConcreteMaterial(3000, false);
            PerimeterFactory f = new PerimeterFactory();
            double d = 5.62;
            double cx = 12.0;
            double cy = 20.0;
            Point2D ColumnCenter = new Point2D(0, 0);
            PunchingPerimeterData data = f.GetPerimeterData(PunchingPerimeterConfiguration.Interior, cx, cy, d, 0.0, 0.0, ColumnCenter);
            ConcreteSectionTwoWayShear sec = new ConcreteSectionTwoWayShear(mat, data, d, cx, cy, true, PunchingPerimeterConfiguration.Interior);
            double gamma_vy = sec.Get_gamma_vy(cx + d, cy + d);

            double refValue = 0.36; //from example 
            double actualTolerance = EvaluateActualTolerance(gamma_vy, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// ACI 421.1R-19  SHEAR REINFORCEMENT FOR SLABS 
        /// Example D.1
        /// </summary>
        [Test]
        public void InteriorSlabReturnsPunchingShearStress()
        {
            IConcreteMaterial mat = this.GetConcreteMaterial(3000, false);
            PerimeterFactory f = new PerimeterFactory();
            double d = 5.62;
            double cx = 12.0;
            double cy = 20.0;
            Point2D ColumnCenter = new Point2D(0, 0);
            PunchingPerimeterData data = f.GetPerimeterData(PunchingPerimeterConfiguration.Interior, cx, cy, d, 0.0, 0.0, ColumnCenter);
            ConcreteSectionTwoWayShear sec = new ConcreteSectionTwoWayShear(mat, data, d, cx, cy, true, PunchingPerimeterConfiguration.Interior);
            double v_u = sec.GetCombinedShearStressDueToMomementAndShear(0, 600, 110,true).v_max;

            double refValue = 0.294; //from example 
            double actualTolerance = EvaluateActualTolerance(v_u, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
