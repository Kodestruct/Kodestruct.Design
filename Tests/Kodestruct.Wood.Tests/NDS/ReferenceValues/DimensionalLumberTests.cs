 
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Wood.NDS.NDS2015;
using Kodestruct.Wood.NDS.NDS2015.Material;




namespace Kodestruct.Wood.Tests.NDS.SawnLumber
{
    [TestFixture]
    public partial class WoodVisuallyGradedDimensionLumberTests : ToleranceTestBase
    {

        public WoodVisuallyGradedDimensionLumberTests()
        {

            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;

        [Test]
        public void DougFirReturnsBendingReferenceValue()
        {
            CalcLog log = new CalcLog();
            VisuallyGradedDimensionLumber dl = new VisuallyGradedDimensionLumber("DOUGLAS FIR-LARCH (NORTH)",
                "Stud", "2 in. & wider", log);

            double F_b = dl.F_b;


            double refValue = 650.0;
            double actualTolerance = EvaluateActualTolerance(F_b, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }



    }
}
