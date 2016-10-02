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
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.AffectedMembers
{
    [TestFixture]
    public class GussetPlateTests
    {
        [Test]
        public void GussetSingleBraceReturnsEffectiveLength()
        {
            AffectedElement el = new AffectedElement();
            double KL = el.GetGussetPlateEffectiveCompressionLength(Steel.AISC.GussetPlateConfiguration.SingleBrace, 10, 10);
            Assert.AreEqual(7,KL);
        }

        [Test]
        public void GussetReturnsCompactness()
        {
            double t_g      =  0.5;
            double c_Gusset =  2.0;
            double F_y      =  36.0;
            double E        =  29000.0;
            double l_1      =  2.0;

            AffectedElement el = new AffectedElement();
            bool IsGussetPlateConfigurationCompact = el.IsGussetPlateConfigurationCompact(t_g, c_Gusset, F_y, E, l_1);
            Assert.AreEqual(true, IsGussetPlateConfigurationCompact);
        }

    }
}
