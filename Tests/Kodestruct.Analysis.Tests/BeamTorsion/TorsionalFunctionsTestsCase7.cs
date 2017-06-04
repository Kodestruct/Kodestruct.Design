 
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Analysis.Torsion;

namespace Kodestruct.Analysis.Tests.BeamTorsion
{
    [TestFixture]
    public partial class TorsionalFunctionsTests : ToleranceTestBase
    {

        [Test]
        public void TorsionalFunctionCase7ReturnsSecondDerivativeAtSupport()
        {

            SetAiscDG9Example5_5Parameters();
            z = 0.0* L;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            //double a_rev = Math.Sqrt(E * C_w / (G * J));
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case7, E, G, J, L, z, T_u,C_w , t, alpha);
            double theta_2der = function.Get_theta_2();
            double refValue = 0.46;
            double ratio = L / a;
            double GraphValuePredicted = theta_2der * G * J / t * (2.0*a)/L;
            double actualTolerance = EvaluateActualTolerance(GraphValuePredicted, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void TorsionalFunctionCase7ReturnsFirstDerivativeA02L()
        {

            SetAiscDG9Example5_5Parameters();
            z = 0.2 * L;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            //double a_rev = Math.Sqrt(E * C_w / (G * J));
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case7, E, G, J, L, z, T_u, C_w, t, alpha);
            double theta_1der = function.Get_theta_1();
            double refValue = 0.14;
            double ratio = L / a;
            double GraphValuePredicted = theta_1der * G * J / t * 2.0  / L;
            double actualTolerance = EvaluateActualTolerance(GraphValuePredicted, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void TorsionalFunctionCase7ReturnsThirdDerivativeA02L()
        {

            SetAiscDG9Example5_5Parameters();
            z = 0.2 * L;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            //double a_rev = Math.Sqrt(E * C_w / (G * J));
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case7, E, G, J, L, z, T_u, C_w, t, alpha);
            double theta_3der = function.Get_theta_3();
            double refValue = 0.5;
            double ratio = L / a;
            double GraphValuePredicted = theta_3der * G * J / t * 2.0*Math.Pow(a,2) / L;
            double actualTolerance = EvaluateActualTolerance(GraphValuePredicted, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        //[Test]
        //public void TorsionalFunctionCase6ReturnsFirstDerivativeAt02()
        //{

        //    SetAiscDG9Example5_1Parameters();
        //    z = 0.2 * L;
        //    TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
        //    //double a_rev = Math.Sqrt(E * C_w / (G * J));
        //    ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case6, E, G, J, L, z, T_u, C_w, t, alpha);
        //    double theta_1der = function.Get_theta_1();
        //    double refValue = 0.1;
        //    double ratio = L / a;
        //    double GraphValuePredicted = theta_1der * G * J / T_u;
        //    double actualTolerance = EvaluateActualTolerance(GraphValuePredicted, refValue);
        //    Assert.LessOrEqual(actualTolerance, tolerance);

        //}
    }
}
