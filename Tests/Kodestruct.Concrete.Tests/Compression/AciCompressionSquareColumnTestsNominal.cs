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


        //EXAMPLE 11-1 Calculation of an Interaction Diagram

        [Test]
        public void ColumnInteractionReturnsM_n_Z_Neg1()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double P_u = 417000.0;
            var nominalResult = col.GetNominalMomentResult(P_u, FlexuralCompressionFiberPosition.Top);
            double M_n = nominalResult.Moment;
            double refValue = 4630*1000; //from MacGregor
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            //ConcreteCompressionStrengthResult result = new ConcreteCompressionStrengthResult(nominalResult, FlexuralCompressionFiberPosition.Top, col.Section.Material.beta1);
            //StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            //FlexuralFailureModeClassification failureMode = f.GetFlexuralFailureMode(result.epsilon_t, result.epsilon_ty);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        [Test]
        public void ColumnInteractionReturnsM_n_Z_Neg2()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double P_u = 247000.0;
            double M_n = col.GetNominalMomentResult(P_u, FlexuralCompressionFiberPosition.Top).Moment;
            double refValue = 4080 * 1000; //from MacGregor
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnInteractionReturnsM_n_Z_Neg4()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double P_u = 43900.0;
            double M_n = col.GetNominalMomentResult(P_u, FlexuralCompressionFiberPosition.Top).Moment;
            double refValue = 257*12 * 1000; //from MacGregor
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnDistributedInteractionReturnsM_n_Z_Neg4()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumnWithDistributed();
            double P_u = 43900.0;
            double M_n = col.GetNominalMomentResult(P_u, FlexuralCompressionFiberPosition.Top).Moment;
            double refValue = 257 * 12 * 1000; //from MacGregor
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        public ConcreteSectionCompression GetConcreteExampleColumn()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();

                Rebar thisBar1 = new Rebar(4, new MaterialAstmA615(A615Grade.Grade60));
                RebarPoint point1 = new RebarPoint(thisBar1, new RebarCoordinate() { X = 0, Y = -h / 2.0 + 2.5});
                LongitudinalBars.Add(point1);


                Rebar thisBar2 = new Rebar(4, new MaterialAstmA615(A615Grade.Grade60));
                RebarPoint point2 = new RebarPoint(thisBar2, new RebarCoordinate() { X = 0, Y = h / 2.0 - 2.5 });
                LongitudinalBars.Add(point2);

                ConcreteSectionCompression column = GetConcreteCompressionMember(b, h, f_c, LongitudinalBars, CompressionMemberType.NonPrestressedWithTies);
                return column;

        }

        public ConcreteSectionCompression GetConcreteExampleColumnWithDistributed()
        {
            double b = 16;
            double h = 16;
            double f_c = 5000;

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            FlexuralSectionFactory flexureFactory = new FlexuralSectionFactory();
            CompressionSectionFactory compressionFactory = new CompressionSectionFactory();
            IConcreteMaterial mat = GetConcreteMaterial(f_c);
            IRebarMaterial rebarMat = new MaterialAstmA615(A615Grade.Grade60);

            ConcreteSectionFlexure flexureMember = flexureFactory.GetRectangularSectionFourSidesDistributed(b, h, 4, 0, 2.5, 2.5, 
                mat, rebarMat,ConfinementReinforcementType.Ties);

            ConcreteSectionCompression column = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember, CompressionMemberType.NonPrestressedWithTies);

            return column;

        }

        public ConcreteSectionCompression GetConcreteCompressionMember(double Width, double Height, double fc, List<RebarPoint> LongitudinalBars, CompressionMemberType CompressionMemberType)
        {
            CalcLog log = new CalcLog();
            IConcreteSection Section = GetRectangularSection(Width, Height, fc);
            ConcreteSectionCompression column = new ConcreteSectionCompression(Section, LongitudinalBars, CompressionMemberType, log);
            return column;
        }
    }
}
