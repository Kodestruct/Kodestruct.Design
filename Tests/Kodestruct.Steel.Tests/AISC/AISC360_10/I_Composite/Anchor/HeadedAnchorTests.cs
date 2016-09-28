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
using Kodestruct.Steel.AISC.AISC360v10.Composite;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Composite
{

    [TestFixture]
    public class HeadedAnchorTests: ToleranceTestBase
    {
        public HeadedAnchorTests()
        {
            tolerance = 0.05; //5% can differ from rounding in the manual
        }

        double tolerance;

        [Test]
        public void HeadedAnchorNoDeckReturnsValue()
        {
            HeadedAnchor a =new HeadedAnchor();
            double Q_n = a.GetNominalShearStrength(DeckAtBeamCondition.NoDeck,HeadedAnchorWeldCase.WeldedDirectly,1,3,3,6,0.75,4,65,110);
            double refValue = 21.2;
            double actualTolerance = EvaluateActualTolerance(Q_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void HeadedAnchorParalleDeckReturnsValue()
        {
            HeadedAnchor a = new HeadedAnchor();
            double Q_n = a.GetNominalShearStrength(DeckAtBeamCondition.Parallel, HeadedAnchorWeldCase.WeldedThroughDeck, 1, 3, 3, 6, 0.75, 4, 65, 110);
            double refValue = 21.2;
            double actualTolerance = EvaluateActualTolerance(Q_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void HeadedAnchorPerpendicularDeckReturnsValue()
        {
            HeadedAnchor a = new HeadedAnchor();
            double Q_n = a.GetNominalShearStrength(DeckAtBeamCondition.Perpendicular, HeadedAnchorWeldCase.WeldedThroughDeck, 2, 3, 3, 6, 0.75, 4, 65, 110);
            double refValue = 18.3;
            double actualTolerance = EvaluateActualTolerance(Q_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
