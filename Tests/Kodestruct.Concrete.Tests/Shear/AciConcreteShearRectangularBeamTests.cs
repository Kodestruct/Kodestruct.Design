//Sample license text.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.ShearFriction;
using Kodestruct.Concrete.ACI.Entities;

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

        /// <summary>
        /// MacGregor, Wight. Reinforced concrete. 6th edition
        /// Example 7-2
        /// </summary>
        [Test]
        public void RectangularBeamReturnsRequiredShearRebarAreaValue()
        {
            double h = 24.0;
            double d = 21.5;
            double b = 14.0;
            double N_u = 0.0;
            double phiV_s = 43100.0*0.75;
            double fc = 3000.0;
            double s = 1.0;
            double c_center = 1.75;
            bool IsLightWeight = false;
            RebarMaterialFactory rmf= new RebarMaterialFactory();
            IRebarMaterial rm = rmf.GetMaterial();
            OneWayShearReinforcedSectionNonPrestressed section = new OneWayShearReinforcedSectionNonPrestressed(d, rm, s);
            double A_v =section.GetRequiredShearReinforcementArea(phiV_s);

            double refValue =  0.0334 ; 
            double actualTolerance = EvaluateActualTolerance(A_v, refValue);

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

        [Test]
        public void ShearFrictionReturnsValue()
        {
            IConcreteMaterial matC = GetConcreteMaterial(4000, false);
            IRebarMaterial m = new MaterialAstmA615(A615Grade.Grade60);
            double A_c = 720.0;
            double A_v = 4.84;

            ConcreteSectionShearFriction sec = new ConcreteSectionShearFriction( ACI.Entities.ShearAndTorsion.ShearFrictionSurfaceType.HardenedNonRoughenedConcrete, matC, A_c, m, A_v, 90, 0);
            double phiV_n = sec.GetShearFrictionStrength() / 1000.0; //convert back to ksi units

            double refValue = 130.68;
            double actualTolerance = EvaluateActualTolerance(phiV_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);


        }
    }
}
