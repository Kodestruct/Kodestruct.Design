 
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Wood.NDS.NDS2015;




namespace Kodestruct.Wood.Tests.NDS.SawnLumber
{
    [TestFixture]
    public partial class WoodAdjustmentFactorTests : ToleranceTestBase
    {
        [Test]
        public void ReturnsSizeFactorDimensionalLumber()
        {
            CalcLog log = new CalcLog();
            DimensionalLumber m = new DimensionalLumber();
            double b = 1.5;
            double d = 5.5;

            double C_F = m.GetSizeFactor(d, b, Wood.NDS.Entities.SawnLumberType.DimensionLumber, Wood.NDS.Entities.CommercialGrade.No2,
                 Wood.NDS.Entities.ReferenceDesignValueType.Bending);


            double refValue = 1.3;
            double actualTolerance = EvaluateActualTolerance(C_F, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }



    }
}
