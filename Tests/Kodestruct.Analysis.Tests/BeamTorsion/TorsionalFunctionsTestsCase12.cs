//Sample license text.
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
        public void TorsionalFunctionCase12ReturnsSecondDerivativeAtSupport()
        {

            SetAiscDG9Example5_1Parameters();
            z = 0.0 * L;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            T_u = 0.0;
            t = 5.0;
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case12, E, G, J, L, z, T_u,C_w , t, alpha);
            double theta_2der = function.Get_theta_2();
            double refValue = 0.84;
            double ratio = L / a;
            double GraphValuePredicted = theta_2der * G * J / t;
            double actualTolerance = EvaluateActualTolerance(GraphValuePredicted, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void TorsionalFunctionCase12ReturnsFirstDerivativeAtMidspan()
        {

            SetAiscDG9Example5_1Parameters();
            z = 0.2 * L;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            //double a_rev = Math.Sqrt(E * C_w / (G * J));
            T_u = 0.0;
            t = 5.0;
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case12, E, G, J, L, z, T_u, C_w, t, alpha);
            double theta_1der = function.Get_theta_1();
            double refValue = 0.62;
            double ratio = L / a;
            double GraphValuePredicted = theta_1der * G * J / t*2/a;
            double actualTolerance = EvaluateActualTolerance(GraphValuePredicted, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
}
