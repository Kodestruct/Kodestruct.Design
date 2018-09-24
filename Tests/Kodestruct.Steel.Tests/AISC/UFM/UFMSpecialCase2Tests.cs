#region Copyright
/*Copyright (C) 2015 Kodestruct Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Steel.AISC.UFM;

namespace Kodestruct.Steel.Tests.AISC
{
    public class UFMSpecialCase2Tests : ToleranceTestBase
    {
        public UFMSpecialCase2Tests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;
        /// <summary>
        /// AISC Design Guide 29
        /// Example 5.3 page 84
        /// </summary>
        [Test]
        public void SpecialCase2ReturnsValue()
        {
            double beta_bar = 12.0;
            double alpha_bar = 17.5;
            double theta = 47.2;
            double alpha = 17.5;
            double DeltaVb = 102;
            UFMCase2ReducedVerticalBraceShearForceBCConnection ufmCase =
                new UFMCase2ReducedVerticalBraceShearForceBCConnection(21.4, 14.0, theta, alpha, beta_bar, alpha_bar, beta_bar,DeltaVb, 840, 0, false, 0, 0);
            double M_u = ufmCase.M_ub;
            double refValueH_uc = 1785.0;
            double actualToleranceH_c = EvaluateActualTolerance(M_u, refValueH_uc);
            Assert.LessOrEqual(actualToleranceH_c, tolerance);
        }

        [Test]
        public void SpecialCase2ReturnsValue1()
        {
            double beta = 28.5;
            double beta_bar = 28.5;
            double alpha_bar = 22.5;
            double alpha = 11.7;
            double theta = 27.0; ;
            double DeltaVb = 75;
            UFMCase2ReducedVerticalBraceShearForceBCConnection ufmCase =
                new UFMCase2ReducedVerticalBraceShearForceBCConnection(32.9, 22.4, theta, alpha, beta, alpha_bar, beta_bar, DeltaVb, 230, 0, false, 0, 0);
            double M_u = ufmCase.M_ub;
            double refValueH_uc = 877;
            double actualToleranceM_b = EvaluateActualTolerance(M_u, refValueH_uc);
            Assert.LessOrEqual(actualToleranceM_b, tolerance);
        }
    }
}
