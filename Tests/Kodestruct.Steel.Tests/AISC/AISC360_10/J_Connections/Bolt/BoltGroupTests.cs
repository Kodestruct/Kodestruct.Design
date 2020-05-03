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


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.Bolt
{

    // 
    public class BoltGroupTests : ToleranceTestBase
    {
        public BoltGroupTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;
        /// <summary>
        /// AISC manual 14th Edition Table 7-6
        /// </summary>
     [Fact]
        public void BoltGroupSingleLine0DegreesReturnsCValue()
        {
            BoltGroup bg = new BoltGroup(4, 1, 0, 3);
            double C = bg.GetInstantaneousCenterCoefficient(8, 0);
            double refValue = 1.34; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C,refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void BoltGroupSingleLine45DegreesReturnsC()
        {
            BoltGroup bg = new BoltGroup(4, 1, 0, 3);
            double C = bg.GetInstantaneousCenterCoefficient(8, 45);
            double refValue = 1.64; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void BoltGroup4X4ReturnsCValue()
        {
            BoltGroup bg = new BoltGroup(2, 2, 3, 3);
            double C = bg.GetInstantaneousCenterCoefficient(10, 0);
            double refValue = 0.78; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void BoltGroup4X4NegativeEccentricitySameCValue()
        {
            BoltGroup bg = new BoltGroup(2, 2, 3, 3);
            double C = bg.GetInstantaneousCenterCoefficient(-10, 0);
            double refValue = 0.78; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

        ///// <summary>
        ///// Elastic moment. Checked against spreadsheet calculation
        ///// </summary>
        //[Fact]
        //public void BoltGroupElasticReturnsElasticMoment()
        //{
        //    BoltGroup bg = new BoltGroup(4, 2, 3, 3);
        //    double C = bg.CalculateElasticGroupMomentCoefficientC();
        //    double boltStrength = 4.39205;
        //    double MomentCapacity = C * boltStrength;
        //    Assert.Equal(100.0, Math.Round(MomentCapacity));
        //}


     [Fact]
        public void BoltGroupTripleLine0DegreesReturnsCValue()
        {
            BoltGroup bg = new BoltGroup(3, 3, 3, 3);
            double C = bg.GetInstantaneousCenterCoefficient(3, 0);
            double refValue = 5.79; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void BoltGroup3X2ReturnsC_primePureMomentValue()
        {
            BoltGroup bg = new BoltGroup(3, 2, 3, 3);
            double C_prime = bg.GetPureMomentCoefficient();
            double refValue = 15.8; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C_prime, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

    }
}
