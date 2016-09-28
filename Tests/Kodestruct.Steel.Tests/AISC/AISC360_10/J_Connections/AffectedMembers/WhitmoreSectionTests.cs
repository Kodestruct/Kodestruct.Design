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
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.AffectedMembers
{
    [TestFixture]
    public class WhitmoreSectionTests : ToleranceTestBase
    {
        public WhitmoreSectionTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }
        double tolerance;

        /// <summary>
        /// AISC Design Guide 29, Page 51
        /// </summary>
        [Test]
        public void AffectedElementReturnsWhitmoreSectionWidth()
        {
            AffectedElement el = new AffectedElement();

                double b_Whitmore = el.GetWhitmoreSectionWidth(18.0, 3.0);
                double refValue = 23.8;
                double actualTolerance = EvaluateActualTolerance(b_Whitmore, refValue);
                Assert.LessOrEqual(actualTolerance, tolerance);
            
        }
    }
}
