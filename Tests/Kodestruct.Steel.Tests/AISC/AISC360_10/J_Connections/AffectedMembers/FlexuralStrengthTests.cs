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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.AffectedMembers
{
    [TestFixture]
    public class FlexuralStrengthTests : ToleranceTestBase
    {
        public FlexuralStrengthTests()
        {
            tolerance = 0.05; //5% can differ from rounding
        }
    

        double tolerance;
    
        [Test]
        public void ConnectedPlateReturnsFlexuralStrength()
        {
            ICalcLog log = new  CalcLog();
            SectionRectangular Section = new SectionRectangular(0.5, 8);
            SectionOfPlateWithHoles NetSection = new SectionOfPlateWithHoles("", 0.5, 8, 2, 0.01, 2, 2,new Common.Mathematics.Point2D(0,0));
            AffectedElementInFlexure element = new AffectedElementInFlexure(Section, NetSection, 50, 65.0);

            double phiM_n = element.GetFlexuralStrength(0);
            Assert.AreEqual(360.0, phiM_n);
        }
        [Test]
        public void ConnectedIShapeInFlexureReturnsStrength()
        {
            ICalcLog log = new CalcLog();
 
            SectionI sI = new SectionI(null, 18, 7.5, 0.570, 0.355);
            SectionIWithFlangeHoles sIh = new SectionIWithFlangeHoles(null, 18, 7.5, 0.570, 0.355, 1.0, 2);

            AffectedElementInFlexure element = new AffectedElementInFlexure(sI, sIh, 50, 65, true);
            double phiM_n = element.GetFlexuralStrength(0);

            double refValue = 318*12;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);


        }
    }
}
