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


namespace Kodestruct.Loads.Tests.ASCE7_10.SeismicLoads
{

    
    public class AsceSeismicApproximatePeriodTests : ToleranceTestBase
    {
        public AsceSeismicApproximatePeriodTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;

        //[Fact]
        //public void CoefficientForUpperBoundReturnsValue()
        //{
        //    double S_D1 = 0.01;
        //    CalcLog log = new CalcLog();
        //    Building building = new Building(null, log);
        //    double Cu = building.GetCoefficientForUpperBoundOnCalculatedPeriod(S_D1);

        //    double refValue = 1.4; 
        //    double actualTolerance = EvaluateActualTolerance(Cu,refValue);

        //    Assert.LessOrEqual(actualTolerance, tolerance);
        //}

      

    }
}
