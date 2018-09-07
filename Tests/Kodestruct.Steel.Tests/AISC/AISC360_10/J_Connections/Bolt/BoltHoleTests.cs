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
using Kodestruct.Steel.AISC.SteelEntities.Bolts;
using b = Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Bolt;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Bolt
{
    /// <summary>
    /// Comparison to AISC Manual values
    /// </summary>
    [TestFixture]

    public class BoltHoleTests
    {
        [Test]
        public void BoltHoleSTDReturnsSize()
        {
            b.BoltGeneral b = new b.BoltGeneral(0.75,0,0);
            double d_h = b.GetBoltHoleWidth(BoltHoleType.STD, false);
            Assert.AreEqual(13.0 / 16.0, d_h);
        }
        [Test]
        public void BoltHoleSTD1InReturnsSize()
        {
            b.BoltGeneral b = new b.BoltGeneral(1.0, 0, 0);
            double d_h = b.GetBoltHoleWidth(BoltHoleType.STD, false);
            Assert.AreEqual(17.0 / 16.0, d_h);
        }

        [Test]
        public void BoltHoleOVSReturnsSize()
        {
            b.BoltGeneral b = new b.BoltGeneral(0.75, 0, 0);
            double d_h = b.GetBoltHoleWidth(BoltHoleType.OVS, false);
            Assert.AreEqual(15.0 / 16.0, d_h);
        }

        [Test]
        public void BoltHoleSSLReturnsWidth()
        {
            b.BoltGeneral b = new b.BoltGeneral(0.75, 0, 0);
            double d_h = b.GetBoltHoleWidth(BoltHoleType.SSL_Parallel, false);
            Assert.AreEqual(13.0 / 16.0, d_h);
        }


        [Test]
        public void BoltHoleSSLReturnsLength()
        {
            b.BoltGeneral b = new b.BoltGeneral(0.75, 0, 0);
            double d_l = b.GetBoltHoleLength(BoltHoleType.SSL_Parallel, false);
            Assert.AreEqual(1.0, d_l);
        }

    }
}
