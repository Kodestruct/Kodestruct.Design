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
            List<PerimeterLineSegment> segments = f.GetPerimeterSegments(PunchingPerimeterConfiguration.Interior, b_1, b_2, d);
            ConcreteSectionTwoWayShear sec = new ConcreteSectionTwoWayShear(mat,segments,d,b_1,b_2,true, PunchingPerimeterConfiguration.Interior);
            double phi_v_c = sec.GetTwoWayStrengthForUnreinforcedConcrete()/1000.0; //convert to kips
            
            double refValue = 71.3/d/95.0; //from example
            double actualTolerance = EvaluateActualTolerance(phi_v_c, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        
        }
    }
}
