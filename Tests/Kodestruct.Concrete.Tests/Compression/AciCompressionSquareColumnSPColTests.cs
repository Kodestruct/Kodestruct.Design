//Sample license text.
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI.ACI318_14;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
    [TestFixture]
    public partial class AciCompressionSquareColumnTests : ConcreteTestBase
    {

        [Test]
        public void ColumnDistributedInteractionReturnsSPCOL_Nominal0()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4.0, 0, 2.5, 2.5,
                mat, rebarMat, ConfinementReinforcementType.Ties, 1);

            ConcreteSectionCompression col = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember,
                CompressionMemberType.NonPrestressedWithTies);




            double P = 1440 * 1000;
            double refValue = 43 * 12 * 1000; //from SP column software


            double M_n = col.GetNominalMomentResult(P, FlexuralCompressionFiberPosition.Top).Moment;

            double phiM_n_KipFt = M_n / 1000.0 / 12.0;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

            double P1 = 1220 * 1000;
            double refValue1 = 160 ; //from SP column software


            double M_n1 = col.GetNominalMomentResult(P1, FlexuralCompressionFiberPosition.Top).Moment/(12*1000.0);
        }


        [Test]
        public void ColumnDistributedInteractionReturnsSPCOL_Nominal1()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4.0, 0, 2.5, 2.5,
                mat, rebarMat, ConfinementReinforcementType.Ties, 1);

            ConcreteSectionCompression col = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember,
                CompressionMemberType.NonPrestressedWithTies);




            double P = 782.65 * 1000;
            double refValue = 308.45 * 12 * 1000; //from SP column software


            double M_n = col.GetNominalMomentResult(P, FlexuralCompressionFiberPosition.Top).Moment;

            double phiM_n_KipFt = M_n / 1000.0 / 12.0;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnDistributedInteractionReturnsSPCOL_Nominal2()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4.0, 0, 2.5, 2.5,
                mat, rebarMat, ConfinementReinforcementType.Ties, 1);

            ConcreteSectionCompression col = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember,
                CompressionMemberType.NonPrestressedWithTies);




            double P = 500 * 1000;
            double refValue = 369 * 12 * 1000; //from SP column software


            double M_n = col.GetNominalMomentResult(P, FlexuralCompressionFiberPosition.Top).Moment;

            double phiM_n_KipFt = M_n / 1000.0 / 12.0;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }


        [Test]
        public void ColumnDistributedInteractionReturnsSPCOL_Nominal3()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4.0, 0, 2.5, 2.5,
                mat, rebarMat, ConfinementReinforcementType.Ties, 1);

            ConcreteSectionCompression col = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember,
                CompressionMemberType.NonPrestressedWithTies);




            double P = 416.6 * 1000;
            double refValue = 385.7 * 12 * 1000; //from SP column software


            double M_n = col.GetNominalMomentResult(P, FlexuralCompressionFiberPosition.Top).Moment;

            double phiM_n_KipFt = M_n / 1000.0 / 12.0;
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnDistributedInteractionReturnsSPCol1()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4.0, 0, 2.5, 2.5, 
                mat, rebarMat,ConfinementReinforcementType.Ties,1);

            ConcreteSectionCompression col = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember, 
                CompressionMemberType.NonPrestressedWithTies);




            double phiP_n = 505 * 1000;
            double refValue = 200 * 12 * 1000; //from SP column software


            double phiM_n = col.GetDesignMomentWithCompressionStrength(phiP_n, FlexuralCompressionFiberPosition.Top).phiM_n;
            double phiM_n_KipFt = phiM_n / 1000.0 / 12.0;
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnDistributedInteractionReturnsSPCol2()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4.0, 0, 2.5, 2.5,
                mat, rebarMat, ConfinementReinforcementType.Ties, 1);

            ConcreteSectionCompression col = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember,
                CompressionMemberType.NonPrestressedWithTies);




            double phiP_n = 797 * 1000;
            double refValue = 102;//from SP column software

            double M_n = col.GetNominalMomentResult(phiP_n/0.65, FlexuralCompressionFiberPosition.Top).Moment;
            double phiM_n_KipFt = M_n / 1000.0 / 12.0;

            double phiM_n = col.GetDesignMomentWithCompressionStrength(phiP_n, FlexuralCompressionFiberPosition.Top).phiM_n/(12 * 1000);
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnDistributedInteractionReturnsSPCOL_Nominal0_Strains()

        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4.0, 0, 2.5, 2.5,
                mat, rebarMat, ConfinementReinforcementType.Ties,1);

            ConcreteSectionCompression col = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember, CompressionMemberType.NonPrestressedWithTies);



            double refForce = 1440;
            double refMoment = 43.0;


            double SteelStrain = 0.00126;
            double P = col.SectionAxialForceResultantFunction(SteelStrain)/1000.0;
            double actualTolerance = EvaluateActualTolerance(P, refForce);

            //IStrainCompatibilityAnalysisResult momentResult = col.GetNominalMomentResult(refForce, FlexuralCompressionFiberPosition.Top);
            //double M_n = momentResult.Moment / 12000.0;
            

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnDistributedInteractionReturnsSPCOL_Nominal1_Strains()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4.0, 0, 2.5, 2.5,
                mat, rebarMat, ConfinementReinforcementType.Ties, 1);

            ConcreteSectionCompression col = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember, CompressionMemberType.NonPrestressedWithTies);



            double refForce = 1220;
            double refMoment = 160.8;


            double SteelStrain = 0.000654;
            double P = col.SectionAxialForceResultantFunction(SteelStrain) / 1000.0;
            double actualTolerance = EvaluateActualTolerance(P, refForce);

            //IStrainCompatibilityAnalysisResult momentResult = col.GetNominalMomentResult(refForce, FlexuralCompressionFiberPosition.Top);
            //double M_n = momentResult.Moment / 12000.0;


            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
