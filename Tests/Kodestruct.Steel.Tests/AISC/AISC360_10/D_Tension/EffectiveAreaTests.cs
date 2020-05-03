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
using Kodestruct.Steel.AISC.AISC360v10;
using Kodestruct.Steel.AISC.AISC360v10.Tension;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Tension
{
    //[TestFixture]
    public class EffectiveAreaTests : ToleranceTestBase
    {

 
    //    //AISC Design Examples 14
    //    //EXAMPLE D.1 W-SHAPE TENSION MEMBER 
    // [Fact]
    //    public void EffectiveNetAreaOpenSectionReturnsValue()
    //    {
    //        TensionMember tm = new TensionMember();
    //        double A_g = 6.16;
    //        double A_connected = 2*5.27*0.4;
    //        double A_e = tm.GetEffectiveNetArea(4.76,0,6.16,A_connected, false,false);
    //        double U = A_e/A_g;
    //        double refValue = 0.684;
    //        double actualTolerance = EvaluateActualTolerance(U, refValue);
    //        Assert.LessOrEqual(actualTolerance, tolerance);
    //    }

    //    public EffectiveAreaTests()
    //    {
    //        tolerance = 0.02; //5% can differ from rounding 
    //    }

    //    double tolerance;  

    }
}
