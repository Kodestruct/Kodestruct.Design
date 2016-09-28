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
using Kodestruct.Steel.AISC.AISC360v10;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Tension
{
    [TestFixture]
    public class ShearLagFactorTests : ToleranceTestBase
    {

        /// <summary>
        /// Design Guide 29. Example 5.1 
        /// Page 46.
        /// </summary>
        [Test]
        public void TensionShearLagFactorCase2ReturnsValue()
        {
            ShearLagFactor slf = new ShearLagFactor();
            double U = slf.GetShearLagFactor(ShearLagCase.Case2,1.65,0.0,18.0,0,0,0,0,true);
            Assert.AreEqual(0.908,Math.Round(U,3));
        }
        //AISC Design Examples 14
        //EXAMPLE D.1 W-SHAPE TENSION MEMBER 
        [Test]
        public void TensionShearLagFactorCase2ForIBeamReturnsValue()
        {
            ShearLagFactor slf = new ShearLagFactor();
            double U = slf.GetShearLagFactor(ShearLagCase.Case2,0.831,5.27,9,0,0,0,0,true);
            double refValue = 0.908;
            double actualTolerance = EvaluateActualTolerance(U, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        //AISC Design Examples 14
        //EXAMPLE D.1 W-SHAPE TENSION MEMBER 
        [Test]
        public void TensionShearLagFactorCase7ForIBeamReturnsValue()
        {
            ShearLagFactor slf = new ShearLagFactor();
            double U = slf.GetShearLagFactor(ShearLagCase.Case7a, double.PositiveInfinity, 5.27, 9, 5.27, 8.28,0,0,true);
            double refValue = 0.85;
            double actualTolerance = EvaluateActualTolerance(U, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        public ShearLagFactorTests()
        {
            tolerance = 0.02; //5% can differ from rounding 
        }

        double tolerance;  
        
    }
}
