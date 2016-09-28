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
using Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads;
using Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads.Components;

namespace Kodestruct.Loads.Tests.ASCE7.ASCE7_10.C03_DeadLoads
{
    [TestFixture]
    public partial class AsceComponentDeadWeightTests
    {
        [Test]
        public void GFRC120pcfAnd5_8BackingReturnsValue()
        {
            ComponentGFRC gfrc = new ComponentGFRC(0, 1, 0);
            var gfrcEntr = gfrc.Weight;
            Assert.AreEqual(6.3, gfrcEntr);
        }

         [Test]
        public void GFRC120pcfAnd1_2BackingReturnsValue()
        {
            BuildingElementComponent bec = new BuildingElementComponent("GFRCPanels", 0, 0, 0.0, "");
            double gfrcEntr = bec.GetComponentWeight().LoadValue;
            Assert.AreEqual(5, gfrcEntr);
        }
    }
}
