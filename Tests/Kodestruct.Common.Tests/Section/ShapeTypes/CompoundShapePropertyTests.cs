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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Common.Section.SectionTypes;

namespace Kodestruct.Common.Tests.Section.ShapeTypes
{
    [TestFixture]
    public partial class CompoundShapeTests : ToleranceTestBase
    {
        /// <summary>
        /// Example from paper:
        /// CALCULATION OF THE PLASTIC SECTION MODULUS USING THE COMPUTER 
        /// DOMINIQUE BERNARD BAUER 
        /// AISC ENGINEERING JOURNAL / THIRD QUARTER /1997 
        /// </summary>
        [Test]
        public void CompoundShapeReturnsPlasticSectionModulusZx()
        {
            List<CompoundShapePart> rectX = new List<CompoundShapePart>()
            {
                new CompoundShapePart(125,8, new Point2D(0,112)),
                new CompoundShapePart(6,100, new Point2D(0,58)),
                new CompoundShapePart(75,8, new Point2D(0,4))
            };
            ArbitraryCompoundShape shape = new ArbitraryCompoundShape(rectX,null);
            double Zx = shape.Z_x;
            Assert.AreEqual(94733.3, Math.Round(Zx,1));

        }

        public CompoundShapeTests()
        {
            tolerance = 0.03; //3% can differ from fillet areas and rounding in the manual
        }

        double tolerance;

        /// <summary>
        /// WT8X50 Plastic neutral axis location
        /// </summary>
        [Test]
        public void CompoundShapeReturnsPNA_WT()
        {


            //Properties
            double d	=	8.49 ;
            double b_f	=	10.4 ;
            double t_w	=	0.585;
            double t_f = 0.985;
            double k = 1.39;
            double y_p = 0.706;
            double refValue = d - y_p;

            CompoundShapePart TopFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, d  - t_f / 2));
            PartWithDoubleFillet TopFillet = new PartWithDoubleFillet(k, t_w, new Point2D(0, d  - t_f), true);
            CompoundShapePart Web = new CompoundShapePart(t_w, d - t_f - k, new Point2D(0, d/2));

            List<CompoundShapePart> tee = new List<CompoundShapePart>()
            {
                 TopFlange,  
                 TopFillet,
                 Web,
            };

            ArbitraryCompoundShape shape = new ArbitraryCompoundShape(tee, null);
            double y_pCalculated = shape.y_pBar;
            double actualTolerance = EvaluateActualTolerance(y_pCalculated, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void CompoundShapeReturnsPNA_W()
        {


            //Properties
            double d	=	17.7 ;
            double b_f	=	6.00 ;
            double t_w	=	0.300;
            double t_f	=	0.425;
            double k = 0.827;

            double refValue = d/2;

            CompoundShapePart TopFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, d - t_f / 2));
            CompoundShapePart BottomFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, t_f / 2));
            CompoundShapePart Web = new CompoundShapePart(t_w, d - 2 * (t_f+k), new Point2D(0, d / 2));
            PartWithDoubleFillet TopFillet = new PartWithDoubleFillet(k, t_w, new Point2D(0, d - t_f), true);
            PartWithDoubleFillet BottomFillet = new PartWithDoubleFillet(k, t_w, new Point2D(0,  t_f), false);


            List<CompoundShapePart> Ishape = new List<CompoundShapePart>()
            {
                BottomFlange,
                BottomFillet,
                Web,
                TopFillet,
                TopFlange,  
            };

            ArbitraryCompoundShape shape = new ArbitraryCompoundShape(Ishape, null);
            double y_pCalculated = shape.y_pBar;
            double actualTolerance = EvaluateActualTolerance(y_pCalculated, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void CompoundShapeReturnsPNA_SymmetricIShape()
        {


            //Properties
            double d = 10;
            double b_f = 8;
            double t_w = 1;
            double t_f = 1;
 
            double refValue = d / 2;

            CompoundShapePart TopFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, d - t_f / 2));
            CompoundShapePart BottomFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, t_f/2));
            CompoundShapePart Web = new CompoundShapePart(t_w, d - 2 * t_f , new Point2D(0, d/2));

            List<CompoundShapePart> Ishape = new List<CompoundShapePart>()
            {
                 BottomFlange,
                   Web,
                 TopFlange  
            };

            ArbitraryCompoundShape shape = new ArbitraryCompoundShape(Ishape, null);
            double y_pCalculated = shape.y_pBar;
            double actualTolerance = EvaluateActualTolerance(y_pCalculated, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void CompoundShapeReturnsZ_netWithMidHole()
        {
            double t_p = 0.5;
            double d_pl = 9.0;
            double n = 3;
            double d_hole = 13.0 / 16.0 + 1.0 / 16.0;
            SectionOfPlateWithHoles plate = new SectionOfPlateWithHoles("", t_p, d_pl, n, d_hole, 1.5, 1.5, new Point2D(0, 0));
            double Z_net = plate.Z_x;
            double refValue = 7.4;
            double actualTolerance = EvaluateActualTolerance(Z_net, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
        [Test]
        public void CompoundShapeReturnsZ_netWith2Holes()
        {
            double t_p = 0.5;
            double d_pl = 6.0;
            double n = 2;
            double d_hole = 13.0 / 16.0 + 1.0 / 16.0;
            SectionOfPlateWithHoles plate = new SectionOfPlateWithHoles("", t_p, d_pl, n, d_hole, 1.5, 1.5, new Point2D(0, 0));
            double Z_net = plate.Z_x;
            double refValue = 3.19;
            double actualTolerance = EvaluateActualTolerance(Z_net, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
