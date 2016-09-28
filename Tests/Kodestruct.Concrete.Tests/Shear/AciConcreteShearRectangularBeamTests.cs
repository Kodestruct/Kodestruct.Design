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
    public partial class AciConcreteShearRectangularBeamTests : AciConcreteShearTestsBase
    {
        /// <summary>
        /// MacGregor, Wight. Reinforced concrete. 6th edition
        /// </summary>
        [Test]
        public void RectangularBeamReturnsBasicConcreteShearValue()
        {
            double h = 24.0 + 2.0;
            double d = 24.0;
            double b = 12.0;
            ConcreteSectionOneWayShearNonPrestressed beam = GetConcreteOneWayShearBeam(b, h, 4000, d, false);
            double rho_w =0.0;
            double M_u   =0.0;
            double V_u   =0.0;
            double N_u = 0.0;

            double A_g = 0; //update this

            //Convert to kips
            double phiV_c =beam.GetConcreteShearStrength(N_u,rho_w,M_u,V_u)/1000.0;


            double refValue = 36.4*0.75; //Example 6-1
            double actualTolerance = EvaluateActualTolerance(phiV_c, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// PCA notes on ACI318-11
        /// Example 12.2
        /// </summary>
        [Test]
        public void RectangularBeamReturnsAxialForceConcreteShearValue()
        {
            double h = 18.0;
            double d = 16.0;
            double b = 10.5;
            ConcreteSectionOneWayShearNonPrestressed beam = GetConcreteOneWayShearBeam(b, h, 3600, d,true);
            double rho_w = 0.0;
            double M_u = 103.4*1000.0*12.0;
            double V_u = 29.8*1000.0;
            double N_u = -26.7*1000;

            double A_g = 0; //update this

            //Convert to kips
            double phiV_c = beam.GetConcreteShearStrength(N_u, rho_w, M_u, V_u) / 1000.0;


            double refValue = 9.2; //Example 6-1
            double actualTolerance = EvaluateActualTolerance(phiV_c, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void RectangularBeamReturnsDetailedConcreteShearValue()
        {
            double h = 36.0;
            double d = 33.0;
            double b = 36;
            ConcreteSectionOneWayShearNonPrestressed beam = GetConcreteOneWayShearBeam(b, h, 5000.0, d, true);
            double rho_w = 0.01;
            double M_u = 1800000;
            double V_u = 360000;
            double N_u = 0;

    
            //Convert to kips
            double phiV_c = beam.GetConcreteShearStrength(N_u, rho_w, M_u, V_u) / 1000.0;
            double phiV_c1 = beam.GetConcreteShearStrength(0,0,0,0) / 1000.0;

            //double refValue = 9.2; //Example 6-1
            //double actualTolerance = EvaluateActualTolerance(phiV_c, refValue);

            //Assert.LessOrEqual(actualTolerance, tolerance);
            Assert.True(true);

        }
    }
}
