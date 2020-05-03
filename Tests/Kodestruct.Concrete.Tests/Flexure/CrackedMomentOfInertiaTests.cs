 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Tests.Common;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI.Entities.FlexuralMember;
using Xunit;

namespace Kodestruct.Concrete.ACI318_14.Tests.Flexure
{
     
    public partial class AciFlexureRectangularBeamTests : ToleranceTestBase
    {
        /// <summary>
        /// PCA notes on ACI 318-11 Example 10.1
        /// </summary>
        [Fact]
        public void CrackedMomentOfInertiaReturnsValue()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 22, 3000, true, new RebarInput(1.8, 2.5), new RebarInput(0.6, 19.5));
            double Icr = beam.GetCrackedMomentOfInertia(FlexuralCompressionFiberPosition.Top);

            double refValue = 3770.0;
            double actualTolerance = EvaluateActualTolerance(Icr, refValue);

            Assert.True(actualTolerance <= tolerance);
        }

        /// <summary>
        /// PCA notes on ACI 318-11 Example 10.1
        /// </summary>
        [Fact]
        public void CrackingMomentReturnsValue()
        {
            ConcreteSectionFlexure beam = GetConcreteBeam(12, 22, 3000,true, new RebarInput(1.8, 2.5), new RebarInput(0.6, 19.5));
            double Mcr = beam.GetCrackingMoment(FlexuralCompressionFiberPosition.Top)/1000.00; //to get to kip*in
             
            double refValue = 33.2*12;
            double actualTolerance = EvaluateActualTolerance(Mcr, refValue);

            Assert.True(actualTolerance <= tolerance);
        }

        /// <summary>
        /// PCA notes on ACI 318-11 Example 10.1
        /// </summary>
        [Fact]
        public void EffectiveMomentOfInertiaReturnsValue()
        {
            double I_cr = 3770.0;
            double M_cr = 33.2 * 12;
            double I_g = 10650.00;
            double M_a = 42.6 * 12;

            EffectiveMomentOfInertiaCalculator emic = new EffectiveMomentOfInertiaCalculator();
            double I_e = emic.GetI_e(I_g, I_cr, M_cr, M_a);

            double refValue = 7025.00;
            double actualTolerance = EvaluateActualTolerance(I_e, refValue);

            Assert.True(actualTolerance <= tolerance);
        }
    }
    

}
