//Sample license text.
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
    public partial class WoodAdjustedValueTests : ToleranceTestBase
    {

        ///// <summary>
        ///// 2005 Example problems. Problem 15 LRFD
        ///// </summary>
        //[Test]
        //public void ReturnsTensionValueDimensionalLumber()
        //{
        //    CalcLog log = new CalcLog();
        //    DimensionalLumber m = new DimensionalLumber();

        //    double C_fu = 1.0;
        //    double C_F =1.0;
        //    double C_M = 1.0;
        //    double C_t = 1.0;
        //    double C_i = 1.0;
        //    double C_L = 1.0;
        //    double C_r = 1.15;
        //    double E_min =580.0;
        //    double F_b = 1.05;
        //    double lambda =0.8;


        //    double F_b_prime = m.GetAdjustedBendingDesignValue(F_b,C_M, C_t, C_L, C_F, C_fu, C_i, C_r, lambda);
        //    double refValue = 2.087;
        //    double actualTolerance = EvaluateActualTolerance(F_b_prime, refValue);
        //    Assert.LessOrEqual(actualTolerance, tolerance);

        //}


    }
}
