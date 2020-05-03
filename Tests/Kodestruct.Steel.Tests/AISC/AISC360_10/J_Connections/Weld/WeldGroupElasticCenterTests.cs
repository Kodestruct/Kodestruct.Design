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

    //[TestFixture]
    public class WeldGroupElasticCenterTests : ToleranceTestBase
    {
        public WeldGroupElasticCenterTests()
        {
            tolerance = 0.05; //5% can differ from number of sub-elements
        }

        double tolerance;


     [Fact]
        public void WeldGroupChannelReturnsCG()
        {
            double L = 8;
            FilletWeldGroup wg = new FilletWeldGroup("C", 6.0, L, 1.0 / 16.0, 70.0);
            double x = wg.CG_X_Left;
            double refValue = 1.8; // from Murray Connection Seminar Part 2
            double actualTolerance = EvaluateActualTolerance(x, refValue);

            Assert.True(actualTolerance<= tolerance);
        }


    }
}
