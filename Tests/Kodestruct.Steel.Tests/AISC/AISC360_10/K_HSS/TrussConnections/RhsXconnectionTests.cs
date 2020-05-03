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
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections;
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC36010.HSSTrussConnections
{

    //[TestFixture]
    public class HssTrussRhsXConnectionTests : ToleranceTestBase
    {

        /// <summary>
        /// AISC DG24
        /// Example 8.3—Cross-Connection with Rectangular HSS
        /// </summary>
     [Fact]
        public void HssXConnectionReturnsYieldingOfChordSidewallsValue()
        {
            CreateElements();
            IHssTrussBranchConnection con = new RhsTrussXConnection(Chord, Branch, 35, Branch, 35,
                AxialForceType.Compression, AxialForceType.Compression, true, 0, 0
                );

            double phiP_n = con.GetChordSidewallLocalYieldingStrength().Value;
            double refValue = 442;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }

        /// <summary>
        /// AISC DG24
        /// Example 8.3—Cross-Connection with Rectangular HSS
        /// </summary>
     [Fact]
        public void HssXConnectionReturnsCripplingOfChordSidewallsValue()
        {
            CreateElements();
            IHssTrussBranchConnection con = new RhsTrussXConnection(Chord, Branch, 35, Branch, 35,
                AxialForceType.Compression, AxialForceType.Compression, true, 0, 0
                );

            double phiP_n = con.GetChordSidewallLocalCripplingStrength().Value;
            double refValue = 75.4;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }

        /// <summary>
        /// AISC DG24
        /// Example 8.3—Cross-Connection with Rectangular HSS
        /// </summary>
     [Fact]
        public void HssXConnectionReturnsLocalYieldingOfBranchesDueToUnevenDistributionValue()
        {
            CreateElements();
            IHssTrussBranchConnection con = new RhsTrussXConnection(Chord, Branch, 35, Branch, 35,
                AxialForceType.Compression, AxialForceType.Compression, true, 0, 0
                );

            double phiP_n = con.GetBranchYieldingFromUnevenLoadDistributionStrength(true).Value;
            double refValue = 209;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);
            Assert.True(actualTolerance<= tolerance);
        }

        public HssTrussRhsXConnectionTests()
        {
            tolerance = 0.05; //5% can differ from rounding 
        }


        double tolerance;

        private void CreateElements()
        {
            SectionTube ch = new SectionTube(null, 8, 8, 1 / 4.0, 0.93 * (1.0 / 4.0), 0.35);
            SteelMaterial mat = new SteelMaterial(46.0);
            Chord = new SteelRhsSection(ch, mat);

            SectionTube br = new SectionTube(null, 6, 8, 3.0 / 8.0, 0.93 * (3.0 / 8.0), 0.35);
            Branch = new SteelRhsSection(br, mat);
        }

        SteelRhsSection Chord;
        SteelRhsSection Branch;
    }

}
