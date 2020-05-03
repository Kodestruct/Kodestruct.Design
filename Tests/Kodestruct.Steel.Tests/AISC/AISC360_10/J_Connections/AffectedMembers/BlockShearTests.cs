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
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.AffectedMembers
{
    // 
    public class BlockShearTests : ToleranceTestBase
    {
        public BlockShearTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;

        /// <summary>
        /// AISC Design Guide 29
        /// Example 5.1 page 48
        /// </summary>
      [Fact]
        public void BlockShearReturnsValue()
        {
            AffectedElement element = new AffectedElement(36.0, 58.0);
            double phiR_n = element.GetBlockShearStrength(39.0, 26.0, 7.0, true);
            double refValue = 938.0;
            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }
    }
}
