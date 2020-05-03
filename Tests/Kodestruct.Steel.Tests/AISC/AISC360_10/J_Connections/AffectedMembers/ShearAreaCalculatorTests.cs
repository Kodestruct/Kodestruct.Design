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
using Kodestruct.Steel.AISC.AISC360v10.J_Connections.AffectedMembers;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.AffectedMembers
{
    //[TestFixture]
    public class ShearAreaCalculatorTests
    {
        /// <summary>
        /// AISC Design guide 29. Page47
        /// </summary>
        /// 
     [Fact]
        public void ShearAreaCalculatorCalculatesA_gv()
        {
            ShearAreaCalculator c = GetShearAreaCalc();
            double A_gv = c.GetGrossAreaShear();
            double refValue = 39.0;
            Assert.Equal(refValue, A_gv);
        }
     [Fact]
        public void ShearAreaCalculatorCalculatesA_nv()
        {
            ShearAreaCalculator c = GetShearAreaCalc();
            double A_nv = c.GetNetAreaShear();
            double refValue = 26.0;
            Assert.Equal(refValue, A_nv);
        }
     [Fact]
        public void ShearAreaCalculatorCalculatesA_nt()
        {
            ShearAreaCalculator c = GetShearAreaCalc();
            double A_nt = c.GetNetAreaTension();
            double refValue = 7.0;
            Assert.Equal(refValue, A_nt);
        }

        private ShearAreaCalculator GetShearAreaCalc()
        {
            double d_hole = 7.0/8.0+2*(1.0/16.0);
            double t =2*1.0;
            ShearAreaCalculator c = new ShearAreaCalculator(Steel.AISC.ShearAreaCase.LBlock, 7, 2, 3, 3, d_hole, t, 1.5, 2.0);
            return c;
        }
    }
}
