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
    [TestFixture]
    public class UFMGeneralTests: ToleranceTestBase
    {
        public UFMGeneralTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;

        /// <summary>
        /// AISC Design Guide 29
        /// Example 5.1 page 52
        /// </summary>
        [Test]
        public void NoMomentCaseExample5_1ReturnsValueBasicCase()
        {
            UFMCaseNoMomentsAtInterfaces ufmCase = new UFMCaseNoMomentsAtInterfaces(21.4, 14.0, 47.2, 17.5, 12.0, 840.0, 0.0,false,0,0);
            double V_uc = ufmCase.V_c;
            double refValueV_uc = 302.0;
            double actualToleranceV_c = EvaluateActualTolerance(V_uc, refValueV_uc);
            Assert.LessOrEqual(actualToleranceV_c, tolerance);

            double V_ub = ufmCase.V_b;
            double refValueV_ub = 269.0;
            double actualToleranceV_b = EvaluateActualTolerance(V_ub, refValueV_ub);
            Assert.LessOrEqual(actualToleranceV_b, tolerance);

            double H_ub = ufmCase.H_b;
            double refValueH_ub = 440.0;
            double actualToleranceH_b = EvaluateActualTolerance(H_ub, refValueH_ub);
            Assert.LessOrEqual(actualToleranceH_b, tolerance);

            double H_uc = ufmCase.H_c;
            double refValueH_uc = 176.0;
            double actualToleranceH_c = EvaluateActualTolerance(H_uc, refValueH_uc);
            Assert.LessOrEqual(actualToleranceH_c, tolerance);

        }

        [Test]
        public void NoMomentCaseExample5_1ReturnsValueDistortionalMomentCase()
        {
            //Note 1270 kip*in distortional moment in gusset is calculated in design guide
            //from equation given in Tamboli book
            //Beam-column connection axial force H_ubc is reduced due to distorional force
            UFMCaseNoMomentsAtInterfaces ufmCase = new UFMCaseNoMomentsAtInterfaces(21.4, 14.0, 47.2, 17.5, 12.0, 840.0, 50.0, true, 127, 100);
            double H_ubc = ufmCase.H_bc;
            double refValueH_ubc = 220.0;
            double actualToleranceV_c = EvaluateActualTolerance(H_ubc, refValueH_ubc);
            Assert.LessOrEqual(actualToleranceV_c, tolerance);
        }

        [Test]
        public void SpecialCase1Example5_2ReturnsValue()
        {
            UFMCase1NonconcentricBraceForce ufmCase = new UFMCase1NonconcentricBraceForce(21.4, 14.0, 47.2,
                17.5, 12.0, 17.5,12,0,0, 840.0, 50.0, 196, 157, false,0,100);

            double V_uc = ufmCase.V_uc;
            double refValueV_uc = 402;
            double actualToleranceV_c = EvaluateActualTolerance(V_uc, refValueV_uc);
            Assert.LessOrEqual(actualToleranceV_c, tolerance);

            double H_uc = ufmCase.H_uc;
            double refValueH_uc = 247;
            double actualToleranceH_c = EvaluateActualTolerance(H_uc, refValueH_uc);
            Assert.LessOrEqual(actualToleranceH_c, tolerance);


            double V_ub = ufmCase.V_ub;
            double refValueV_ub = 169;
            double actualToleranceV_b = EvaluateActualTolerance(V_ub, refValueV_ub);
            Assert.LessOrEqual(actualToleranceV_b, tolerance);

            double H_ub = ufmCase.H_ub;
            double refValueH_ub = 369;
            double actualToleranceH_b = EvaluateActualTolerance(H_ub, refValueH_ub);
            Assert.LessOrEqual(actualToleranceH_b, tolerance);




        }
    }
}
