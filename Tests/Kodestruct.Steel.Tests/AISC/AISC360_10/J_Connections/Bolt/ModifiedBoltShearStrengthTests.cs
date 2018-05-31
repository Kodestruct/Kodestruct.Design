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
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Bolt
{
    [TestFixture]
    public class ModifiedBoltShearStrengthTests
    {
        //AISC Design Examples V14
        //EXAMPLE J.3 COMBINED TENSION AND SHEAR IN BEARING TYPE CONNECTIONS 

         [Test]
        public void GetNominalTensileStrengthModifiedToIncludeTheEffectsOfShearStress()
        {
            BoltFactory bf = new BoltFactory("A325");

            BoltBearingGroupA bolt = new BoltBearingGroupA(3.0 / 4.0, BoltThreadCase.Included, null);
            double V = 8.0;
            double phi_R_n = bolt.GetAvailableTensileStrength(V);

            Assert.AreEqual(25.4, Math.Round(phi_R_n,1));
        }

        //AISC Desin guide 29
        //Example 5.4 
        //Page 110

         [Test]
         public void GetNominalTensileStrengthModifiedToIncludeTheEffectsOfShearStressDG29()
         {
             BoltFactory bf = new BoltFactory("A325");

             IBoltBearing bolt = bf.GetBearingBolt(7.0 / 8.0, "N");
             double V = 8.05;
             double phi_R_n = bolt.GetAvailableTensileStrength(V);

             Assert.AreEqual(39.3, Math.Round(phi_R_n, 1));
         }


    }
}
