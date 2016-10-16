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
 
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Steel.Tests.AISC.AISC360_10.D_Tension
{
    public class TensileStrengthTests : ToleranceTestBase
    {
        public TensileStrengthTests()
        {
            tolerance = 0.02; //2% can differ from rounding 
        }

        double tolerance;

        [Test]
        public void TensionAffectedElementReturnsValue()
        {
            AffectedElementInTension el = new AffectedElementInTension(50, 65);
            double phiRn = el.GetTensileCapacity(2.0, 1.0);
            double refValue = 0.75 * 65 * 1; 
            double actualTolerance = EvaluateActualTolerance(phiRn, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
