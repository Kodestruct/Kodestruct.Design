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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.AISC360v10.AffectedMembers.Splices;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Splices
{
    [TestFixture]
    public class ColumnSpliceTests: ToleranceTestBase
    {
        public ColumnSpliceTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;
        [Test]
        /// <summary>
        /// Tamboli handbook example. Page 117
        /// </summary>
        public void ColumnSpliceCalculatesFlangeForce()
        {
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection section = AiscShapeFactory.GetShape("W14X99", ShapeTypeSteel.IShapeRolled);
            ColumnFlangeSplice sp = new ColumnFlangeSplice(section as PredefinedSectionI, 50, 7.5);
            double F = sp.GetEffectiveFlangeForce(0, 0, 376.0 * 2.0);
            double refValue = 55; // from Tamboli
             double actualTolerance = EvaluateActualTolerance(F, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

    


    }
}
