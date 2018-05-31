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
using Kodestruct.Common.Section.SectionTypes;

namespace Kodestruct.Common.Tests.Section.ShapeTypes
{
    /// <summary>
    /// Compare calculated properties to C8X18.7 listed properties.
    /// </summary>
    [TestFixture]
    public class SectionChannelRolledTests
    {

        [Test]
        public void SectionChannelRolledReturnsArea()
        {
            SectionChannelRolled shape = new SectionChannelRolled("", 8, 2.53, 0.39, 0.487, 15.0/16.0);
            double A = shape.A;
            Assert.AreEqual(5.87, Math.Round(A, 2));

        }
        [Test]
        public void SectionChannelRolledReturnsIx()
        {
            SectionChannelRolled shape = new SectionChannelRolled("", 8, 2.53, 0.39, 0.487, 15.0 / 16.0);
            double A = shape.I_x;
            Assert.AreEqual(48.244, Math.Round(A, 3));

        }

        [Test]
        public void SectionChannelRolledReturnsIy()
        {
            //SectionChannelRolled shape = new SectionChannelRolled("", 8, 2.53, 0.39, 0.487, 15.0 / 16.0);
            SectionChannelRolled shape = new SectionChannelRolled("", 8, 2.53, 0.39, 0.487, 0.0);
            double A = shape.I_y;
            //Assert.AreEqual(2.455, Math.Round(A, 3));
            Assert.AreEqual(2.44, Math.Round(A, 2));

        }
        [Test]
        public void SectionChannelReturns_xp()
        {
            //SectionChannelRolled shape = new SectionChannelRolled("", 8, 2.53, 0.39, 0.487, 15.0 / 16.0);
            SectionChannelRolled shape = new SectionChannelRolled("", 8, 2.53, 0.39, 0.487, 0.0);
            double x_pBar = shape.x_pBar;
            //Assert.AreEqual(2.455, Math.Round(A, 3));
            Assert.AreEqual(2.19, Math.Round(x_pBar, 2));

        }
    }
}
