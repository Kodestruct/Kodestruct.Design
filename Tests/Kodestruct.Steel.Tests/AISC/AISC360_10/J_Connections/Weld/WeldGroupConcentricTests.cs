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
using Kodestruct.Steel.AISC;
using Kodestruct.Steel.AISC.AISC360v10.Connections;
using Kodestruct.Steel.AISC.AISC360v10.Connections.Weld;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.J_Connections.Weld
{
    // 
    public class WeldGroupConcentricTests : ToleranceTestBase
    {

        public WeldGroupConcentricTests()
        {

            tolerance = 0.05; //5% can differ from number of sub-elements
        }

        double tolerance;


        //AISC Design Exaples V14
        //Example J.1 Fillet weld in longitudinal shear
     [Fact]
        public void WeldConcentricParallelLinesReturnsValue()
        {
            FilletWeldGroup wg = new FilletWeldGroup(WeldGroupPattern.ParallelVertical, 5.0, 28.0, 3.0 / 16.0, 70.0);
            double phiR_n = wg.GetConcentricLoadStrenth(0);
            double refValue = 0.75 * 5.57 * 2 * 28.0;
            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
            Assert.True(actualTolerance<= tolerance);

        }
     [Fact]
        public void WeldConcentricCShapeLinesReturnsValue()
        {
            FilletWeldGroup wg = new FilletWeldGroup(WeldGroupPattern.C, 1.0, 2.0, 1.0 / 16.0, 70.0);
            double phiR_n = wg.GetConcentricLoadStrenth(0);
            double ws1 = 4 * 1.392;
            double ws2 =0.85*2 * 1.392+2*1.5*1.392;
            double refValue = Math.Max(ws1, ws2);
            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
            Assert.True(actualTolerance<= tolerance);

        }

     [Fact]
        public void WeldConcentricCShapeLinesReturnsValue45Degrees()
        {
            FilletWeldGroup wg = new FilletWeldGroup(WeldGroupPattern.C, 1.0, 2.0, 1.0 / 16.0, 70.0);
            double phiR_n = wg.GetConcentricLoadStrenth(45);
            double refValue = 4 * 1.392;
            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
            Assert.True(actualTolerance<= tolerance);

        }
    }
}
