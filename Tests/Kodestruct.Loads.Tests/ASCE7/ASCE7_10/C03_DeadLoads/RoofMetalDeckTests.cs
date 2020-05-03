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
 
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads;
using Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads.Components;

namespace Kodestruct.Loads.Tests.ASCE7.ASCE7_10.C03_DeadLoads
{
    
    public partial class AsceComponentDeadWeightTests
    {
        [Fact]
        public void RoofMetalDeckReturnsValue1_5In()
        {
            ComponentRoofDeck rd = new ComponentRoofDeck(0, 0, 0);
            var rdEntr = rd.Weight;
            Assert.Equal(2.0, rdEntr);
        }
    }
}
