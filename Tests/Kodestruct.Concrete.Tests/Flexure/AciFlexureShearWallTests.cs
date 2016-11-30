//Sample license text.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14.Materials;

namespace Kodestruct.Concrete.ACI318_14.Tests.Flexure
{
    [TestFixture]
    public partial class AciFlexureRectangularBeamTests: ToleranceTestBase
    {
        /// <summary>
        /// Wight. Reinforced concrete. 7th edition
        /// </summary>
        [Test]
        public void ShearWallFlexuralCapacityTopReturnsNominalValue()
        {
            ConcreteSectionFlexure beam = GetShearWall();
            IStrainCompatibilityAnalysisResult MResult = beam.GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition.Top);
            double M_n = MResult.Moment;

            double refValue = 7182.546387 * 1000.0 * 12.0;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }
        [Test]

        public void StrainDistributionReturnsValue()
        {
            ConcreteSectionFlexure beam = GetShearWall();
            LinearStrainDistribution StrainDistribution = new LinearStrainDistribution(192, 0.003, -0.051272);
            List<RebarPointResult> r = beam.CalculateRebarResults(StrainDistribution);
            SectionAnalysisResult TrialSectionResult = beam.GetSectionResult(StrainDistribution, FlexuralCompressionFiberPosition.Top );
            double M_n = TrialSectionResult.Moment/12000.0;
            double refValue = 7182.546387;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        ConcreteSectionFlexure GetShearWall()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(24, 192, 4000, true,
            new RebarInput(1.58, 3),
            new RebarInput(1.58, 15),
            new RebarInput(1.58, 27),
            new RebarInput(1.58, 39),
            new RebarInput(1.58, 51),
            new RebarInput(1.58, 141),
            new RebarInput(1.58, 153),
            new RebarInput(1.58, 165),
            new RebarInput(1.58, 177),
            new RebarInput(1.58, 189)
        );
            return beam;
        }
   
    }
}
