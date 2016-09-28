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

 
        /// <summary>
        /// 2005 Example problems. Problem 5 LRFD
        /// </summary>
        //[Test]
        //public void ReturnColumnStabilityFactorDimensionalLumber()
        //{
        //    CalcLog log = new CalcLog();
        //    DimensionalLumber m = new DimensionalLumber();
        //    double b = 1.5;
        //    double d = 9.25;

        //    double F_b = 1.050; //ksi
        //    double F_v = 0.175; //ksi
        //    double E = 1600.0; //ksi
        //    double C_F =1.0;
        //    double C_M = 1.0;
        //    double C_t = 1.0;
        //    double C_i = 1.0;
        //    double C_T = 1.0;
        //    double C_r = 1.15;
        //    double E_min =580.0;

        //    double lambda =0.8;
        //    double C_P = m.GetColumnStabilityFactor(b, d,F_c,E_min,l_e,C_M,C_M,C_t,C_F,C_i,C_T,lambda);

        //    double refValue = 0.342;
        //    double actualTolerance = EvaluateActualTolerance(C_P, refValue);
        //    Assert.LessOrEqual(actualTolerance, tolerance);

        //}


    }
}
