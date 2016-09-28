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
using Kodestruct.Steel.AISC;
using Kodestruct.Steel.AISC.AISC360v10.Connections.Weld;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Weld
{
    [TestFixture]
    public class WeldStrengthTests : ToleranceTestBase
    {

        public WeldStrengthTests()
        {

            tolerance = 0.05; //5% can differ from number of sub-elements
        }

        double tolerance;

        //AISC Design Exaples V14
        //Example J.2 Fillet weld in longitudinal shear
        [Test]
        public void WeldConcentricLoadAtAngleReturnsValue()
        {
            FilletWeld weld = new FilletWeld(50, 65, 70, 5.0 / 16.0, 2.0, 2.0); //L = 2 because Example uses 2 sided welds
            double phiF_nw = weld.GetStrength( WeldLoadType.WeldShear, 60.0, false);
            double refValue = 19.5;
            double actualTolerance = EvaluateActualTolerance(phiF_nw, refValue); 
            Assert.LessOrEqual(actualTolerance, tolerance);

        }
        [Test]
        public void FilletWeldReturnsUnitStrength()
        {
            FilletWeld weld = new FilletWeld(50, 65, 70, 1 / 16.0, 100, 1); 
            double phiF_nw = weld.GetStrength(WeldLoadType.WeldShear, 0, false);
            double refValue = 1.392;
            double actualTolerance = EvaluateActualTolerance(phiF_nw, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void FilletWeldReturnsUnitStrengthWithoutBaseMetal()
        {
            FilletWeld weld = new FilletWeld(0, 0, 70, 1 / 16.0, 0, 1);
            double phiF_nw = weld.GetStrength(WeldLoadType.WeldShear, 0, true);
            double refValue = 1.392;
            double actualTolerance = EvaluateActualTolerance(phiF_nw, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
}
