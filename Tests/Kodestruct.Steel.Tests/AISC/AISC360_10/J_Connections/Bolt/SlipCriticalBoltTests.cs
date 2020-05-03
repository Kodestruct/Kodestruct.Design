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
using Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Bolt
{
    //[TestFixture]
    public class SlipCriticalBoltTests : ToleranceTestBase
    {

        public SlipCriticalBoltTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;
        /// <summary>
        /// Example J.4A Slip-Critical Connection With Short-Slotted Holes
        /// </summary>
     [Fact]
        public void BoltSlipCritical_SSL_ReturnsSlipResistance()
        {
            BoltSlipCriticalGroupA bolt = new BoltSlipCriticalGroupA(0.75, BoltThreadCase.Included, BoltFayingSurfaceClass.ClassA,
            BoltHoleType.SSL_Perpendicular, BoltFillerCase.One, 2, null);
            double phiR_n = bolt.GetSlipResistance();
            double refValue = 19.0; // from Design Examples
            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
            Assert.True(actualTolerance<=tolerance);
        }

        /// <summary>
        /// Example J.4B Slip-Critical Connection With Long-Slotted Holes
        /// </summary>
     [Fact]
        public void BoltSlipCritical_LSL_ReturnsSlipResistance()
        {
            BoltSlipCriticalGroupA bolt = new BoltSlipCriticalGroupA(0.75, BoltThreadCase.Included, BoltFayingSurfaceClass.ClassA,
            BoltHoleType.LSL_Parallel, BoltFillerCase.One, 2, null);
            double phiR_n = bolt.GetSlipResistance();
            double refValue = 13.3; // from Design Examples
            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }

        /// <summary>
        /// Example J.5 Combined tension and shear in a slip-critical connection
        /// </summary>
     [Fact]
        public void BoltSlipCriticalSTDReturnsReducedSlipResistance()
        {
            BoltSlipCriticalGroupA bolt = new BoltSlipCriticalGroupA(0.75, BoltThreadCase.Included, BoltFayingSurfaceClass.ClassA,
            BoltHoleType.STD, BoltFillerCase.One, 1, null);
            double T_u = 72.0/8.0;
            double phiR_n = bolt.GetReducedSlipResistance(T_u);
            double refValue = 54.4/8.0; // from Design Examples
            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }
    }
}
