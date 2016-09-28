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
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Composite;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Composite.Flexure
{
    [TestFixture]
    public partial class CompositeBeamTests: ToleranceTestBase
    {
        public CompositeBeamTests()
        {
            tolerance = 0.05; //5% can differ from rounding in the manual
        }

        double tolerance;

        private CompositeBeamSection GetBeamForTests(double SumQ_n)
        {
            double Y_2 = 5;
            double f_cPrime = 4;
            double h_solid = 3;
            double b_eff;
            double h_rib = 3;
            b_eff = SumQ_n / ((h_rib + h_solid - Y_2) * 2 * 0.85 * f_cPrime); //Back calculate b_eff to get the round number from AISC manual
            double Y_2T = h_solid - (SumQ_n / (0.85 * f_cPrime * b_eff) / 2) + h_rib; //test

            AiscShapeFactory factory = new AiscShapeFactory();
            ISection section = factory.GetShape("W18X35", ShapeTypeSteel.IShapeRolled);
            PredefinedSectionI catI = section as PredefinedSectionI;
            SectionIRolled secI = new SectionIRolled("", catI.d, catI.b_fTop, catI.t_f, catI.t_w, catI.k);
            CompositeBeamSection cs = new CompositeBeamSection(secI, b_eff, h_solid, h_rib, 50.0, f_cPrime);
            return cs;
        }

        [Test]
        public void CompositeBeamSectionReturnsLowerBoundMomentOfInertia()
        {
            double SumQ_n = 387;
            CompositeBeamSection cs = GetBeamForTests(SumQ_n);
            double I_LB = cs.GetLowerBoundMomentOfInertia(SumQ_n);

            double refValue = 1360; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(I_LB, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
