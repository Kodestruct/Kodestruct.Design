 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Concrete.ACI318_14.Tests.Flexure
{
    [TestFixture]
    public partial class AciFlexureRectangularBeamTests: ToleranceTestBase
    {
        /// <summary>
        /// Wight. Reinforced concrete. 7th edition
        /// </summary>
        [Test]
        public void SimpleBeamFlexuralCapacityTopReturnsNominalValue()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 20, 4000, true, new RebarInput(4, 2.5));
            IStrainCompatibilityAnalysisResult MResult = beam.GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition.Top);
            double M_n = MResult.Moment;

            double refValue = 291000*12.0;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void SimpleBeamFlexuralCapacityTopReturnsDesignValue()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 20, 4000, true, new RebarInput(4, 2.5));
            ConcreteFlexuralStrengthResult MResult = beam.GetDesignFlexuralStrength(FlexuralCompressionFiberPosition.Top);
            double M_n = MResult.phiM_n/1000/12.0;

            double refValue = 253.0;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void SimpleBeamFlexuralCapacityTopReturnsDesignValueWRI()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 4.5, 7500, true, new RebarInput(0.3, 1));
            ConcreteFlexuralStrengthResult MResult = beam.GetDesignFlexuralStrength(FlexuralCompressionFiberPosition.Top);
            double M_n = MResult.phiM_n / 1000 / 12.0;

            double refValue = 4.53;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }



        [Test]
        public void SimpleBeamFlexuralCapacityCompressionTopBarsReturnsNominalValue()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 12, 4000, true, new RebarInput(1, 1));
            IStrainCompatibilityAnalysisResult MResult = beam.GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition.Top);
            double M_n = MResult.Moment/12000.0;

            double refValue = 51.32; 
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void BeamReturnsphiMn()
        {
            var ConcreteMaterial =new Kodestruct.Concrete.ACI318_14.Materials.ConcreteMaterial(6000,  ACI.Entities.ConcreteTypeByWeight.Normalweight,null);

            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            double b = 16;
            double h = 40;
            double f_y = 60000;
            RebarMaterialGeneral LongitudinalRebarMaterial = new RebarMaterialGeneral(f_y);

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();

             Rebar TopRebar = new Rebar(4, LongitudinalRebarMaterial);
             RebarPoint TopPoint = new RebarPoint(TopRebar, new RebarCoordinate() { X = 0, Y = h / 2.0 - 3 });
            LongitudinalBars.Add(TopPoint);

            Rebar BottomRebar = new Rebar(4, LongitudinalRebarMaterial);
            RebarPoint BottomPoint = new RebarPoint(BottomRebar, new RebarCoordinate() { X = 0, Y = -h / 2.0 + 3 });
            LongitudinalBars.Add(BottomPoint);

            CrossSectionRectangularShape shape = new CrossSectionRectangularShape(ConcreteMaterial, null, b, h);

            IConcreteFlexuralMember fs = new ConcreteSectionFlexure(shape, LongitudinalBars, null,
                ConfinementReinforcementType.Ties);

            ConcreteFlexuralStrengthResult result = fs.GetDesignFlexuralStrength(FlexuralCompressionFiberPosition.Top);

        }


        [Test]
        public void SimpleBeamFlexuralCapacityCompressionBottomReturnsNominalValue()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 12, 4000, false, new RebarInput(1, 1));
            IStrainCompatibilityAnalysisResult MResult = beam.GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition.Bottom);
            double M_n = MResult.Moment/12000.0;

            double refValue = 51.32; 
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

         public void TwoLayerBeamFlexuralCapacityReturnsValue()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 12, 4000,true, new RebarInput(1, 1), new RebarInput(1, 3));
            IStrainCompatibilityAnalysisResult MResult = beam.GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition.Top);
            double phiMn = MResult.Moment;
            Assert.AreEqual(1023529, Math.Round(phiMn, 0));
        }

         public void ThreeLayerBeamFlexuralCapacityReturnsValue()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 12, 4000, true, new RebarInput(1, 1), new RebarInput(1, 3), new RebarInput(1, 7));
            IStrainCompatibilityAnalysisResult MResult = beam.GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition.Top);
            double phiMn = MResult.Moment;
            Assert.AreEqual(1101327, Math.Round(phiMn, 0));
        }
    }
}
