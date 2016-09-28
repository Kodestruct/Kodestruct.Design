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
    [TestFixture]
    public class ShapeTeeTests: ToleranceTestBase
    {

        public ShapeTeeTests()
        {
            tolerance = 0.05; //5% can differ from fillet areas and rounding in the manual
        }

        double tolerance;


        double Z_x;
        double Z_y;
        double y_eTop;
        double y_PlTop;
        double t_w;
        double d;
        double b_f;
        double t_f;
        double k;
        double A;
        double I_x;
        double I_y;
        
        private  void SetUpTests()
        {
                //WT8X13 
                Z_x = 7.36;
                Z_y = 2.73;
                y_eTop = 2.09;
                y_PlTop = 0.372;
                t_w = 0.25;
                d = 7.85;
                b_f = 5.5;
                t_f = 0.345;
                k = 0.747;
                A = 3.84;
                I_x = 23.5;
                I_y = 4.79;
        }
        
        [Test]
        public void CustomTeeReturnsZ_x()
        {
            SetUpTests();
            SectionTeeRolled tee = new SectionTeeRolled(null,d,b_f,t_f,t_w,k);
            double Z_xCalc = tee.Z_x;

            double refValue = Z_x; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(Z_xCalc, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void CustomTeeReturnsZ_y()
        {
            SetUpTests();
            SectionTeeRolled tee = new SectionTeeRolled(null, d, b_f, t_f, t_w, k);
            double Z_yCalc = tee.Z_y;

            double refValue = Z_y; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(Z_yCalc, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void CustomTeeReturnsI_y()
        {
            SetUpTests();
            SectionTeeRolled tee = new SectionTeeRolled(null, d, b_f, t_f, t_w, k);
            double I_yCalc = tee.I_y;

            double refValue = I_y; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(I_yCalc, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }


        [Test]
        public void CustomTeeReturnsPNA()
        {
            SetUpTests();
            SectionTeeRolled tee = new SectionTeeRolled(null, d, b_f, t_f, t_w, k);
            double y_pCalc = tee.y_pBar;
            double y_pManual = 0.372;
            double refValue = d-y_pManual; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(y_pCalc, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
