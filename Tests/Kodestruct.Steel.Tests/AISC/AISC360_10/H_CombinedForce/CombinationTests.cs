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
using Kodestruct.Steel.AISC.AISC360v10.Combination;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.CombinedForce
{
    // 
    public class CombinationTests
    {
     [Fact]
        public void CombinationEllipticNandVReturnsValue()
        {
            Combination combo = new Combination();
            double IR = combo.GetInteractionRatio(Steel.AISC.CombinationCaseId.Elliptical,1,0,0,0,1.5,2,0,0,0,2);
            Assert.True(0.8125== IR);
        }
    }
}
