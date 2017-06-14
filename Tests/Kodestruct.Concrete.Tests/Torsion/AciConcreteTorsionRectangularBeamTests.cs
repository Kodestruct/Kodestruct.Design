 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Concrete.ACI318_14.Tests.Shear
{
    [TestFixture]
    public partial class AciConcreteTorsionRectangularBeamTests : AciConcreteTorsionTestsBase
    {
        /// <summary>
        /// MacGregor, Wight. Reinforced concrete. 6th edition
        /// Example 7-2
        /// </summary>
        [Test]
        public void RectangularBeamReturnsThresholdTorsionValue()
        {
            double h = 24.0;
            double d = 21.5;
            double b = 14.0;
            double N_u = 0.0;
            double fc = 3000.0;
            double c_center = 0.0;

            ConcreteSectionTorsion tb = GetConcreteTorsionBeam(b,h,fc,d,false,c_center);

            double T_th = tb.GetThreshholdTorsion(N_u) / 1000.0; //Conversion from ACI psi units to ksi units

            double refValue = 61; //Example 7-2
            double actualTolerance = EvaluateActualTolerance(T_th, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// MacGregor, Wight. Reinforced concrete. 6th edition
        /// Example 7-2
        /// </summary>
        [Test]
        public void RectangularBeamReturnsInteractionValue()
        {
            double h = 24.0;
            double d = 21.5;
            double b = 14.0;
            double N_u = 0.0;
            double V_u = 57100.0;
            double T_u = 28.0*12000.0;
            double fc = 3000.0;
            double c_center = 1.75;
            bool IsLightWeight=false;
            //double phiV_c = beam.GetConcreteShearStrength(N_u, rho_w, M_u, V_u);

            IConcreteMaterial mat = GetConcreteMaterial(fc, IsLightWeight);
            CrossSectionRectangularShape section = new CrossSectionRectangularShape(mat, null, b, h);
            ConcreteSectionOneWayShearNonPrestressed beam = new ConcreteSectionOneWayShearNonPrestressed(d,section);
            double phiV_c =beam.GetConcreteShearStrength(0,0,0,0);

            ConcreteSectionTorsion tb = GetConcreteTorsionBeam(b, h, fc, d, false, c_center);

            double IR = tb.GetMaximumForceInteractionRatio(V_u, T_u, phiV_c, b, d); 

            double refValue = 326.0/411.0; //Example 7-2
            double actualTolerance = EvaluateActualTolerance(IR, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// MacGregor, Wight. Reinforced concrete. 6th edition
        /// Example 7-2
        /// </summary>
        [Test]
        public void RectangularBeamReturnsRequiredTransverseRebarValue()
        {
            double h = 24.0;
            double d = 21.5;
            double b = 14.0;
            double N_u = 0.0;
            double V_u = 57100.0;
            double T_u = 28.0 * 12000.0;
            double fc = 3000.0;
            double s = 1.0;
            double c_center = 1.75;
            bool IsLightWeight = false;


            ConcreteSectionTorsion tb = GetConcreteTorsionBeam(b, h, fc, d, false, c_center);

            double A_s = tb.GetRequiredTorsionTransverseReinforcementArea(T_u,s,60000.00); //Conversion from ACI psi units to ksi units

            double refValue = 0.0204; //Example 7-2
            double actualTolerance = EvaluateActualTolerance(A_s, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// MacGregor, Wight. Reinforced concrete. 6th edition
        /// </summary>
        [Test]
        public void RectangularBeamReturnsRequiredLongitudinalRebarValue()
        {
            double h = 24.0;
            double d = 21.5;
            double b = 14.0;
            double N_u = 0.0;
            double V_u = 57100.0;
            double T_u = 28.0 * 12000.0;
            double fc = 3000.0;
            double s = 1.0;
            double c_center = 1.75;
            bool IsLightWeight = false;


            ConcreteSectionTorsion tb = GetConcreteTorsionBeam(b, h, fc, d, false, c_center);

            double A_s = tb.GetRequiredTorsionLongitudinalReinforcementArea(T_u, 60000); //Conversion from ACI psi units to ksi units

            double refValue = 1.26; //Example 7-2
            double actualTolerance = EvaluateActualTolerance(A_s, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
}
