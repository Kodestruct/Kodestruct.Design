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
using Kodestruct.Steel.AISC.AISC360v10.Connections;
using Kodestruct.Tests.Common;
using Xunit;



namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections
{

    //[TestFixture]
    public class PryingActionDesignTests
    {
     [Fact]
        public void PryingActionReturnsAvailableTension()
        {
           
            //AISC Design Guide 29
            //Page 232

            double d_b=1.0;
            double d_holePrime = 1.0625; 
            double b_stem=2.82; //distance to stem of angle AISC manual Fig 9-4
            double a_edge=1.93; //distance to face of angle AISC manual Fig 9-4
            double p=3.0; 
            double B_bolt=56.1;
            double F_u = 58.0;

            PryingActionElement pac = new PryingActionElement( d_b,  d_holePrime,  b_stem,  a_edge,  p,  B_bolt,  F_u);
            
            double t_p=0.5;
            double T =pac.GetMaximumBoltTensionForce(t_p);
            Assert.True(Math.Ceiling(6.96)== Math.Ceiling(T));

        }
    }
}
