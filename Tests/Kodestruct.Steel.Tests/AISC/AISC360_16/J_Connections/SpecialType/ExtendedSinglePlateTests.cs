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
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v16.Connections;
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace Kodestruct.Steel.Tests.AISC.AISC360v16.Connections.SpecialType
{
    [TestFixture]
    public class ExtendedSinglePlateTestsV16: ToleranceTestBase
    {
        public ExtendedSinglePlateTestsV16()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;



        //AISC Design Examples v14. Example1.
        [Test]
        public void ExtendedPlateUnstiffenedReturnsBucklingStrength()
        {
            double d_pl = 24;
            double a = 9;
            double t_pl = 0.5;
            ExtendedSinglePlate sp = new ExtendedSinglePlate();
            double phiR_n = sp.GetShearStrengthWithoutStabilizerPlate(d_pl,t_pl,a,50.0);
            double refValue = 157;

            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }





    }
}
