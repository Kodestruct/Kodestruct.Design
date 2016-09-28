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
using Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Bolt
{
    [TestFixture]
    public class BoltNominalStressTests
    {

        // AISC Design examples V14
        //EXAMPLE J.3 COMBINED TENSION AND SHEAR IN BEARING TYPE CONNECTIONS 
        [Test]
        public void BoltReturnsNominalShearStress()
        {
            BoltFactory bf = new BoltFactory("A325");
            IBoltMaterial material = bf.GetBoltMaterial();
            double F_nv = material.GetNominalShearStress(BoltThreadCase.Included);
            Assert.AreEqual(54.0, F_nv);
        }

        [Test]
        public void BoltReturnsNominalShearStressStringInput()
        {
            BoltFactory bf = new BoltFactory("A325");
            IBoltMaterial material = bf.GetBoltMaterial();
            double F_nv = material.GetNominalShearStress("N");
            Assert.AreEqual(54.0, F_nv);
        }

        [Test]
        public void BoltReturnsNominalTensileStress()
        {
            BoltFactory bf = new BoltFactory("A325");
            IBoltMaterial material = bf.GetBoltMaterial();
            double F_nt = material.GetNominalTensileStress(BoltThreadCase.Included);
            Assert.AreEqual(90.0, F_nt);
        }

        [Test]
        public void BoltReturnsNominalTensileStressStringInput()
        {
            BoltFactory bf = new BoltFactory("A325");
            IBoltMaterial material = bf.GetBoltMaterial();
            double F_nt = material.GetNominalTensileStress("N");
            Assert.AreEqual(90.0, F_nt);
        }
    }
}
