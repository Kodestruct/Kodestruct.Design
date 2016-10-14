using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Steel.Tests.AISC.AISC360_10.D_Tension
{
    public class TensileStrengthTests : ToleranceTestBase
    {
        public TensileStrengthTests()
        {
            tolerance = 0.02; //2% can differ from rounding 
        }

        double tolerance;

        [Test]
        public void TensionAffectedElementReturnsValue()
        {
            AffectedElementInTension el = new AffectedElementInTension(50, 65);
            double phiRn = el.GetTensileCapacity(2.0, 1.0);
            double refValue = 0.75 * 65 * 1; 
            double actualTolerance = EvaluateActualTolerance(phiRn, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
