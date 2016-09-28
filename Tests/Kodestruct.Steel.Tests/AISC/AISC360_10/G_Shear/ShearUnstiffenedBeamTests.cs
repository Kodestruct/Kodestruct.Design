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
using Kodestruct.Steel.AISC.AISC360v10.Shear;
using aisc = Kodestruct.Steel.AISC;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Shear
{
    [TestFixture]
    public class ShearUnstiffenedBeamTests: ToleranceTestBase
    {
        public ShearUnstiffenedBeamTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;

        [Test]
        public void ShearBeamIShapeReturnsValue()
        {
                double h = 26.3;
                double t_w = 1.04;
                double a=0;
                double F_y = 50.0;
                double E = 29000;
                ShearMemberFactory factory = new ShearMemberFactory();
                IShearMember member = factory.GetShearMemberNonCircular(ShearNonCircularCase.MemberWithoutStiffeners, h, t_w, a, F_y,E);
                double phiV_n = member.GetShearStrength();

                double refValue = 821.0;
                double actualTolerance = EvaluateActualTolerance(phiV_n, refValue);

                Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
}
