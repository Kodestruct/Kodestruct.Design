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

using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.Tests.ASCE7_10.SeismicLoads
{

    
    public class AsceSiteCoefficientTests : ToleranceTestBase
    {
        public AsceSiteCoefficientTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;

        [Fact]
        public void CoefficientF_aReturnsValue1()
        {
            double S_S = 0.273;
            SeismicSiteClass cl = SeismicSiteClass.D;
            CalcLog log = new CalcLog();
            Site s = new Site(cl,null);
            double F_a = s.SiteCoefficientFa(S_S);

            double refValue = 1.58;
            double actualTolerance = EvaluateActualTolerance(F_a, refValue);

            Assert.True(actualTolerance <= tolerance);
        }

        [Fact]
        public void CoefficientF_aReturnsValue2()
        {
            double S_S = 1.2;
            SeismicSiteClass cl = SeismicSiteClass.D;
            CalcLog log = new CalcLog();
            Site s = new Site(cl, null);
            double F_a = s.SiteCoefficientFa(S_S);

            double refValue = 1.02;
            double actualTolerance = EvaluateActualTolerance(F_a, refValue);

            Assert.True(actualTolerance <= tolerance);
        }


        [Fact]
        public void CoefficientF_vReturnsValue1()
        {
            double S_1 = 0.15;
            SeismicSiteClass cl = SeismicSiteClass.D;
            CalcLog log = new CalcLog();
            Site s = new Site(cl, null);
            double F_v = s.SiteCoefficientFv(S_1);

            double refValue = 2.2;
            double actualTolerance = EvaluateActualTolerance(F_v, refValue);

            Assert.True(actualTolerance <= tolerance);
        }

        [Fact]
        public void CoefficientF_vReturnsValue2()
        {
            double S_1 = 0.45;
            SeismicSiteClass cl = SeismicSiteClass.D;
            CalcLog log = new CalcLog();
            Site s = new Site(cl, null);
            double F_v = s.SiteCoefficientFv(S_1);

            double refValue = 1.55;
            double actualTolerance = EvaluateActualTolerance(F_v, refValue);

            Assert.True(actualTolerance <= tolerance);
        }

    }
}
