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
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Compression
{

    //[TestFixture]
    public class CompressionIShapeNonSlenderTests : ToleranceTestBase
    {
        public CompressionIShapeNonSlenderTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;


        ISteelCompressionMember column { get; set; }
        private void CreateColumn(double L_ex, double L_ey, double L_ez = 0, string Shape = "W14X82")
        {
            CompressionMemberFactory factory = new CompressionMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection section = AiscShapeFactory.GetShape(Shape, ShapeTypeSteel.IShapeRolled);
            SteelMaterial mat = new SteelMaterial(50.0,29000);
            L_ez = L_ez == 0? L_ex : L_ez;
            column = factory.GetCompressionMember(section,mat, L_ex, L_ey, L_ez);

        }
        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
     [Fact]
        public void IShapeReturns_0ft_LengthAxialStrength() 
        {
            CreateColumn(0, 0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 1080.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
     [Fact]
        public void IShapeReturns_16ft_LengthAxialStrength()
        {
            CreateColumn(16.0 * 12.0, 16.0 * 12.0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 697.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }



     [Fact]
        public void IShapeW16X26Returns_16ft_LengthAxialStrength()
        {
            CreateColumn(14.0 * 12.0, 14.0 * 12.0, 0,"W16X26");
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 76.7; //to be confirmed
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
     [Fact]
        public void IShapeReturns_36ft_LengthAxialStrength()
        {
            CreateColumn(36.0 * 12.0, 36.0 * 12.0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 179;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

        }

    
}
