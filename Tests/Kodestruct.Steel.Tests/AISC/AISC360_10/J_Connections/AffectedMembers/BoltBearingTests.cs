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
using Kodestruct.Steel.AISC.SteelEntities.Bolts;
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.AffectedMembers
{
    //[TestFixture]
    public class BoltBearingTests : ToleranceTestBase
    {
        public BoltBearingTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;

        /// <summary>
        /// AISC Design Guide 29
        /// Example 5.1 page 50
        /// </summary>
      [Fact]
        public void BoltBearingInnerBoltsReturnsValue()
        {
            AffectedElementWithHoles element = new AffectedElementWithHoles();
            double phiR_n = element.GetBearingStrengthAtBoltHole(2.06, 7.0 / 8.0, 1, 50.0, 65.0,BoltHoleType.STD, BoltHoleDeformationType.ConsideredUnderServiceLoad, false);
            double refValue = 102.0;
            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }

      [Fact]
         public void BoltBearingEndBoltsReturnsValue()
         {
             AffectedElementWithHoles element = new AffectedElementWithHoles();
             double phiR_n = element.GetBearingStrengthAtBoltHole(1.03, 7.0 / 8.0, 1, 50.0, 65.0, BoltHoleType.STD, BoltHoleDeformationType.ConsideredUnderServiceLoad, false);
             double refValue = 60.3;
             double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
             Assert.True(actualTolerance<= tolerance);
         }
    }
}
