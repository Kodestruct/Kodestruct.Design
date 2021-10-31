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
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Steel.AISC.AISC360v10.Connections;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Weld
{

    // 
    public class WeldGroupInstantaneousCenterTests : ToleranceTestBase
    {
        public WeldGroupInstantaneousCenterTests()
        {
            tolerance = 0.05; //5% can differ from number of sub-elements
        }

        double tolerance;


     [Fact]
        public void WeldGroupChannelReturnsValue()
        {
            double L = 10;
            FilletWeldGroup wg = new FilletWeldGroup("C", 5.0, L, 1.0 / 16.0, 70.0);
            double C = wg.GetInstantaneousCenterCoefficient(5.0, 0);
            double refValue = 2.85; // from AISC Steel Manual
            double P_n = refValue * L;
            double spreadsheetPn = 20.4 / 0.75; //Yakpol.net version 2008.1
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }


     [Fact]
        public void WeldGroup2LinesReturnsValue()
        {
            double L = 10;
            FilletWeldGroup wg = new FilletWeldGroup("ParallelVertical", 5.0, L, 1.0 / 16.0, 70.0);
            double C = wg.GetInstantaneousCenterCoefficient(5.0, 0);
            double refValue = 2.44; // from AISC Steel Manual
            double P_n = refValue * L;
            double spreadsheetPn = 18.34 / 0.75; //Yakpol.net version 2008.1
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void WeldGroup2LinesHorizontalReturnsValue()
        {
            double L = 10;
            FilletWeldGroup wg = new FilletWeldGroup("ParallelHorizontal",L, 5.0, 1.0 / 16.0, 70.0);
            double C = wg.GetInstantaneousCenterCoefficient(5.0, 0);
            double refValue = 2.62; // from AISC Steel Manual
            double P_n = refValue * L;
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void WeldGroupRectangleReturnsValue()
        {
            double L = 10;
            FilletWeldGroup wg = new FilletWeldGroup("Rectangle", 5.0, L, 1.0 / 16.0, 70.0);
            double C = wg.GetInstantaneousCenterCoefficient(10, 0);
            double refValue = 2.45; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C, refValue);
            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void WeldGroupAngleReturnsValue()
        {
            double L = 10;
            FilletWeldGroup wg = new FilletWeldGroup("L", 5.0, L, 1.0 / 16.0, 70.0);
            double C = wg.GetInstantaneousCenterCoefficient(5.0, 0);
            double refValue = 1.95; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void WeldGroup2LinesOutOfPlaneReturnsValue()
        {
            double L = 10;
            FilletWeldGroup wg = new FilletWeldGroup("ParallelVertical", 5.0, L, 1.0 / 16.0, 70.0,true);
            double C = wg.GetInstantaneousCenterCoefficient(10.0, 0);
            double refValue = 1.28; // from AISC Steel Manual
            double P_n = refValue * L;
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }
    }
}
