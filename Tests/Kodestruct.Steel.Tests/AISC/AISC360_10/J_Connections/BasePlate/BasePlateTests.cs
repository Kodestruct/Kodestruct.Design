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
using Kodestruct.Steel.AISC;
using Kodestruct.Steel.AISC.AISC360v10.Connections.BasePlate;
using Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.BasePlate
{
    //[TestFixture]
    public class BasePlateTests : ToleranceTestBase
    {
        //AISC Night School
        //Steel Design 2: Selected Topics
        //Louis F. Geschwindner
        //April 2016
        //Session 8: Column Base Plates
        //Example 2
      [Fact]
        public void BasePlateConcentricReturnsMinimumThickness()
        {
            BasePlateIShape plate = new BasePlateIShape(16.5,18.5,12.7,12.2, 0.9, 0, 3, 36, 1224.0);
            BasePlateConcentricallyLoaded bp = new BasePlateConcentricallyLoaded(plate);
            double t_pMin = bp.GetMinimumThicknessConcentricLoad(990.0);
            double refValue = 1.51;
            double actualTolerance = EvaluateActualTolerance(t_pMin, refValue);

            Assert.True(actualTolerance<= tolerance);
 
        }

         //AISC Night School
         //Steel Design 2: Selected Topics
         //Louis F. Geschwindner
         //April 2016
         //Session 8: Column Base Plates
         //Example 3
      [Fact]
         public void BasePlateTensionReturnsMinimumThickness()
         {
             BasePlateIShape plate = new BasePlateIShape(1, 1, 1, 1, 1, 0, 1, 36,0);
             BasePlateTensionLoaded bp = new BasePlateTensionLoaded(plate);
             double t_pMin = bp.GetMinimumBasePlateBasedOnBoltTension(12.5, 2.0 - 0.55 / 2.0, 3.45);
             double refValue = 0.878;
             double actualTolerance = EvaluateActualTolerance(t_pMin, refValue);

             Assert.True(actualTolerance<= tolerance);

         }

         //AISC Night School
         //Steel Design 2: Selected Topics
         //Louis F. Geschwindner
         //April 2016
         //Session 8: Column Base Plates
         //Example 4
      [Fact]
         public void BasePlateSmallMomentReturnsMinimumThicknessNS()
         {
             BasePlateIShape plate = new BasePlateIShape(16.5, 18.5, 12.7, 12.2, 0.9, 0, 4, 36, 0);
             BasePlateEccentricallyLoaded bp = new BasePlateEccentricallyLoaded(plate);
             double t_pMin = bp.GetMinimumThicknessEccentricLoadStrongAxis(376.0, 940, BendingAxis.Major, 8.0);
             double refValue = 1.09;
             double actualTolerance = EvaluateActualTolerance(t_pMin, refValue);
             Assert.True(actualTolerance<= tolerance);

         }


        //AISC Design guide 1
        //Example 4.6

      [Fact]
         public void BasePlateSmallMomentReturnsMinimumThickness()
         {
             BasePlateIShape plate = new BasePlateIShape(19, 19, 12.7, 12.2, 0.9, 0, 4, 36, 0);
             BasePlateEccentricallyLoaded bp = new BasePlateEccentricallyLoaded(plate);
             double t_pMin = bp.GetMinimumThicknessEccentricLoadStrongAxis(376.0, 940, BendingAxis.Major, 8.0);
             double refValue = 1.36;
             double actualTolerance = EvaluateActualTolerance(t_pMin, refValue);
             Assert.True(actualTolerance<= tolerance);

         }

         //AISC Design guide 1
         //Example 4.7
      [Fact]
         public void BasePlateLargeMomentReturnsMinimumThickness()
         {
             BasePlateIShape plate = new BasePlateIShape(22, 22, 12.7, 12.2, 0.9, 0, 4, 36, 0);
             BasePlateEccentricallyLoaded bp = new BasePlateEccentricallyLoaded(plate);
             double t_pMin = bp.GetMinimumThicknessEccentricLoadStrongAxis(376.0, 3600, BendingAxis.Major, 9.5);
             double refValue = 2.26;
             double actualTolerance = EvaluateActualTolerance(t_pMin, refValue);
             Assert.True(actualTolerance<= tolerance);

         }

         public BasePlateTests()
        {
            tolerance = 0.05; //5% can differ from rounding 
        }

        double tolerance;       
    }
}
