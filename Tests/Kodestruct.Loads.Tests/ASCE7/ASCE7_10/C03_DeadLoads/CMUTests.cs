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
        public void UngroutedCMULWReturnsValue8In()
        {
            ComponentCMUUngrouted105 rd = new ComponentCMUUngrouted105(2, 0, 0);
            var masEntr = rd.Weight;
            Assert.Equal(31.0, masEntr);
        }
        [Fact]
        public void PartiallyGroutedCMULWReturnsValue8In24OC()
        {
            ComponentCMUPartialGrouted105 rd = new ComponentCMUPartialGrouted105(1, 3, 0);
            var masEntr = rd.Weight;
            Assert.Equal(46.0, masEntr);
        }
        [Fact]
        public void FullyGroutedCMULWReturnsValue8In()
        {
            ComponentCMUGrouted105 rd = new ComponentCMUGrouted105(1, 0, 0);
            var masEntr = rd.Weight;
            Assert.Equal(75.0, masEntr);
        }
        [Fact]
        public void SolidCMULWReturnsValue8In()
        {
            ComponentCMUSolid105 rd = new ComponentCMUSolid105(2, 0, 0);
            var masEntr = rd.Weight;
            Assert.Equal(69.0, masEntr);
        }
    }
}
