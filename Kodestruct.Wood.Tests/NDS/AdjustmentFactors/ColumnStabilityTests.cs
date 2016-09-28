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
        /// 2005 Example problems. Problem 1 LRFD
        /// </summary>
        [Test]
        public void ReturnColumnStabilityFactorDimensionalLumber()
        {
            CalcLog log = new CalcLog();
            DimensionalLumber m = new DimensionalLumber();
            double b = 1.5;
            double d = 5.5;

            
            double C_F =1.0;
            double C_M = 1.0;
            double C_t = 1.0;
            double C_i = 1.0;
            double C_T = 1.0;
            double C_D = 1.15;
            double E_min =580.0;
            double F_c = 1.6;
            double l_e = 144.0;
            double lambda =0.8;
            double C_P = m.GetColumnStabilityFactor(d, F_c, E_min, l_e, C_M, C_M, C_t, C_t,C_F, C_i, C_i, C_T, lambda);

            double refValue = 0.342;
            double actualTolerance = EvaluateActualTolerance(C_P, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// 2005 Example problems. Problem 2 LRFD
        /// </summary>

        [Test]
        public void ReturnColumnStabilityFactorTimber()
        {
            CalcLog log = new CalcLog();
            DimensionalLumber m = new DimensionalLumber();
            double b = 5.5;
            double d = 5.5;


            double C_F = 1.0;
            double C_M_Fc = 0.91;
            double C_M_E = 1.0;
            double C_t = 1.0;
            double C_i = 1.0;
            double C_T = 1.0;
            double C_D = 1.15;
            double E_min = 470.0;
            double F_c =0.85;
            double l_e = 15*12*0.5; //with K-factor
            double lambda = 0.8;
            double C_P = m.GetColumnStabilityFactor(d, F_c, E_min, l_e,C_M_Fc,C_M_E, C_t,C_t, C_F, C_i,C_i, C_T, lambda);

            double refValue = 0.827;
            double actualTolerance = EvaluateActualTolerance(C_P, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

    }
}
