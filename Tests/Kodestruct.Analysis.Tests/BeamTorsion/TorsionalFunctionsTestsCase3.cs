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
        public void TorsionalFunctionCase3ReturnsFirstDerivativeAtMidspan()
        {
            //AISC Design Guide 9 Example 5.1
            //Case 3, with alpha = 0.5:
            SetAiscDG9Example5_1Parameters();
            z = L / 2;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case3,E,G,J,L,z,T_u,C_w,t,alpha);
            double theta_1der = function.Get_theta_1();
            double refValue = 0;

            double actualTolerance = EvaluateActualTolerance(theta_1der, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        
        }

        [Test]
        public void TorsionalFunctionCase3ReturnsFirstDerivativeAtSupport()
        {
            //AISC Design Guide 9 Example 5.1
            //Case 3, with alpha = 0.5:
            SetAiscDG9Example5_1Parameters();
            z = 0;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case3,E, G, J, L, z, T_u,C_w, t, alpha);
            double theta_1der = function.Get_theta_1();
            double refValue = 0.28 * (-5.78) * Math.Pow(10, -3.0);

            double actualTolerance = EvaluateActualTolerance(theta_1der, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);


        }

        [Test]
        public void TorsionalFunctionCase3ReturnsSecondDerivativeAtMidspan()
        {
            //AISC Design Guide 9 Example 5.1
            //Case 3, with alpha = 0.5:
            SetAiscDG9Example5_1Parameters();
            z = L / 2;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case3, E, G, J, L, z, T_u,C_w, t, alpha);
            double theta_2der = function.Get_theta_2();
            double refValue = 0.44 * (-5.78) * Math.Pow(10, -3.0)/62.1;

            double actualTolerance = EvaluateActualTolerance(theta_2der, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void TorsionalFunctionCase3ReturnsSecondDerivativeAtSupport()
        {
            //AISC Design Guide 9 Example 5.1
            //Case 3, with alpha = 0.5:
            SetAiscDG9Example5_1Parameters();
            z = 0;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case3, E, G, J, L, z, T_u,C_w, t, alpha);
            double theta_2der = function.Get_theta_2();
            double refValue = 0;

            double actualTolerance = EvaluateActualTolerance(theta_2der, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }


        [Test]
        public void TorsionalFunctionCase3ReturnsThirdDerivativeAtMidspan()
        {
            //AISC Design Guide 9 Example 5.1
            //Case 3, with alpha = 0.5:
            SetAiscDG9Example5_1Parameters();
            z = L / 2;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case3, E, G, J, L, z, T_u,C_w, t, alpha);
            double theta_3der = function.Get_theta_3();
            double refValue = 0.5 * (-5.78) * Math.Pow(10, -3.0) / 62.1;

            double actualTolerance = EvaluateActualTolerance(theta_3der, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void TorsionalFunctionCase3ReturnsThirdDerivativeAtSupport()
        {
            //AISC Design Guide 9 Example 5.1
            //Case 3, with alpha = 0.5:
            SetAiscDG9Example5_1Parameters();
            z = 0;
            TorsionalFunctionFactory tf = new TorsionalFunctionFactory();
            ITorsionalFunction function = tf.GetTorsionalFunction(TorsionalFunctionCase.Case3, E, G, J, L, z, T_u,C_w, t, alpha);
            double theta_3der = function.Get_theta_3();
            double refValue = 0.22 * (-5.78) * Math.Pow(10, -3.0) / 62.1;

            double actualTolerance = EvaluateActualTolerance(theta_3der, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }


    }
}
