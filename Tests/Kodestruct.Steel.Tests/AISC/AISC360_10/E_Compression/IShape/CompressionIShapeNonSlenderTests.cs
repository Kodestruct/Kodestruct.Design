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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Steel.AISC.AISC360v10.Compression;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Compression
{



    [TestFixture]
    public class CompressionIShapeSlenderTests : ToleranceTestBase
    {
        public CompressionIShapeSlenderTests()
        {
            tolerance = 0.05; //5% can differ from rounding in the manual
        }

        double tolerance;


        ISteelCompressionMember column { get; set; }
        private void CreateColumn(double L_ex, double L_ey, double L_ez=0)
        {
            CompressionMemberFactory factory = new CompressionMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection section = AiscShapeFactory.GetShape("W14X43", ShapeTypeSteel.IShapeRolled);
            SteelMaterial mat = new SteelMaterial(50.0,29000);
            L_ez = L_ez == 0? L_ex : L_ez;
            column = factory.GetCompressionMember(section,mat, L_ex, L_ey, L_ez);

        }
        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
        [Test]
        public void IShapeReturns_0ft_LengthAxialStrength() 
        {
            CreateColumn(0, 0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 562.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
        [Test]
        public void IShapeReturns_16ft_LengthAxialStrength()
        {
            CreateColumn(16.0 * 12.0, 16.0 * 12.0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 267.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
        [Test]
        public void IShapeReturns_10ft_LengthAxialStrength()
        {
            CreateColumn(10.0 * 12.0, 10.0 * 12.0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 422.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        } 

        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
        [Test]
        public void IShapeReturns_30ft_LengthAxialStrength()
        {
            CreateColumn(30.0 * 12.0, 30.0 * 12.0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 78.5;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// Torsional and Constrained-Axis Flexural-Torsional
        /// Buckling Tables for Steel W-Shapes in Compression
        /// DI LIU, BRAD DAVIS, LEIGH ARBER and RAFAEL SABELLI
        /// Table 1
        /// </summary>
        [Test]
        public void IShapeReturns_CATB_Strength()
        {
            CreateColumn("W24X104", 1.0 * 12.0, 1.0 * 12.0, 20.0 * 12);
            SteelLimitStateValue colFlexure = column.GetTorsionalAndFlexuralTorsionalBucklingStrength(true);
            double phiP_n = colFlexure.Value;
            double refValue = 891.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }


        private void CreateColumn(string ShapeName, double L_ex, double L_ey, double L_ez = 0 )
        {
            CompressionMemberFactory factory = new CompressionMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection section = AiscShapeFactory.GetShape(ShapeName, ShapeTypeSteel.IShapeRolled);
            SteelMaterial mat = new SteelMaterial(50.0, 29000);
            L_ez = L_ez == 0 ? L_ex : L_ez;
            column = factory.GetCompressionMember(section, mat, L_ex, L_ey, L_ez, true);

        }

    }

    
}
