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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Steel.AISC.AISC360v10.Compression;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Compression
{

    //[TestFixture]
    public class CompressionIShapeNonCompactTests : ToleranceTestBase
    {
        public CompressionIShapeNonCompactTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;


        ISteelCompressionMember column { get; set; }
        private void CreateColumn(double L_ex, double L_ey, double L_ez = 0)
        {
            double d=17.0; 
            double b_f= 8.0;
            double t_f= 1.0;
            double t_w = 0.25;
            ISection r = new SectionI("", d, b_f, t_f, t_w);
            SteelMaterial mat = new SteelMaterial(50.0,29000);
            L_ez = L_ez == 0? L_ex : L_ez;
            CompressionMemberFactory factory = new CompressionMemberFactory();
            column = factory.GetCompressionMember(r,mat, L_ex, L_ey, L_ez);

        }
        /// <summary>
        /// AISC Design Examples E.2
        /// </summary>
     [Fact]
        public void IShapeReturnsAxialStrength() 
        {
            CreateColumn(15 * 12, 15 * 12);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 508.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }



        }

    
}
