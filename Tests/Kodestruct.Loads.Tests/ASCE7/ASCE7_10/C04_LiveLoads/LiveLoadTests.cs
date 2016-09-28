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
using Kodestruct.Loads.ASCE.ASCE7_10.LiveLoads;

namespace Kodestruct.Loads.Tests
{
    [TestFixture]
    public class LiveLoadTests
    {
        [Test]
        public void  ReturnsValueOffice()
        {
            double q = 0;
            LiveLoadBuilding lb = new LiveLoadBuilding();
            q=lb.GetLiveLoad("Office", false);
            Assert.AreEqual(50.0, q);
        }
        [Test]
        public void ReturnsValueHouseWithPartitions()
        {
            double q = 0;
            LiveLoadBuilding lb = new LiveLoadBuilding();
            q = lb.GetLiveLoad("House", true,15);
            Assert.AreEqual(55.0, q);
        }
    }
}
