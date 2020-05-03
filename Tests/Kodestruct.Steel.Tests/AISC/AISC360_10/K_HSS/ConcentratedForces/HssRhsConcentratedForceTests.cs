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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections;
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC36010.HSSTrussConnections
{

    //[TestFixture]
    public class HssRhsConcentratedForceTests : ToleranceTestBase
    {

        /// <summary>
        /// AISC DG 24
        /// Example 8.4—Overlapped K-Connection with Rectangular HSS
        /// </summary>
     [Fact]
        public void HssRhsConcentratedForceThroughPlateReturnsValue()
        {
            SectionTube ch = new SectionTube(null, 8, 8, 0.25,0.93*0.25,1.5*0.25);
            SteelMaterial matE = new SteelMaterial(46.0);
            SteelRhsSection Element = new SteelRhsSection(ch, matE);

            SectionRectangular rec = new SectionRectangular(0.25,8.0);
            SteelMaterial matR = new SteelMaterial(36.0);
            SteelPlateSection pl = new SteelPlateSection(rec,matR);

            CalcLog log = new CalcLog();

            RhsLongitudinalThroughPlate concForceConnection = new RhsLongitudinalThroughPlate(Element, pl, log, false,45.0, 148.0, 0.0);
            double phiR_n = concForceConnection.GetHssWallPlastificationStrengthUnderAxialLoad().Value;

            double refValueSec = 46.2;
            double actualToleranceSec = EvaluateActualTolerance(phiR_n, refValueSec);
            Assert.True(actualToleranceSec<= tolerance);
        }

     [Fact]
        public void HssRhsConcentratedForceLongitudinalPlateReturnsValue()
        {
            SectionTube ch = new SectionTube(null, 8, 8, 0.25, 0.93 * 0.25, 1.5 * 0.25);
            SteelMaterial matE = new SteelMaterial(46.0, 65, SteelConstants.ModulusOfElasticity, SteelConstants.ShearModulus);
            SteelRhsSection Element = new SteelRhsSection(ch, matE);

            SectionRectangular rec = new SectionRectangular(0.25, 8.0);
            SteelMaterial matR = new SteelMaterial(36.0);
            SteelPlateSection pl = new SteelPlateSection(rec, matR);

            CalcLog log = new CalcLog();

            RhsLongitudinalPlate concForceConnection = new RhsLongitudinalPlate(Element, pl, log, false, 45.0, 148.0, 0.0);
            double phiR_n = concForceConnection.GetHssMaximumPlateThicknessForShearLoad().Value;

            //double refValueSec = 46.2;
            //double actualToleranceSec = EvaluateActualTolerance(phiR_n, refValueSec);
            //Assert.LessOrEqual(actualToleranceSec, tolerance);
        }


        public HssRhsConcentratedForceTests()
        {
            tolerance = 0.05; //5% can differ from rounding 
        }


        double tolerance;




        
    }

}
