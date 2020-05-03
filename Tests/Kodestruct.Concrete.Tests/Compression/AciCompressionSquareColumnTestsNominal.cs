 
using Kodestruct.Tests.Common;
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
using Xunit;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
     
    public partial class AciCompressionSquareColumnTests : ConcreteTestBase
    {


        //EXAMPLE 11-1 Calculation of an Interaction Diagram

        [Fact]
        public void ColumnInteractionReturnsM_n_Z_Neg1()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double P_u = 417000.0;
            var nominalResult = col.GetNominalMomentResult(P_u, FlexuralCompressionFiberPosition.Top);
            double M_n = nominalResult.Moment;
            double refValue = 4630*1000; //from MacGregor
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.True(actualTolerance <= tolerance);

        }

        [Fact]
        public void ColumnInteractionReturnsM_n_Z_Neg2()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double P_u = 247000.0;
            double M_n = col.GetNominalMomentResult(P_u, FlexuralCompressionFiberPosition.Top).Moment;
            double refValue = 4080 * 1000; //from MacGregor
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.True(actualTolerance <= tolerance);
        }

        [Fact]
        public void ColumnInteractionReturnsM_n_Z_Neg4()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double P_u = 43900.0;
            double M_n = col.GetNominalMomentResult(P_u, FlexuralCompressionFiberPosition.Top).Moment;
            double refValue = 257*12 * 1000; //from MacGregor
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.True(actualTolerance <= tolerance);
        }

        [Fact]
        public void ColumnDistributedInteractionReturnsM_n_Z_Neg4()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumnWithDistributed();
            double P_u = 43900.0;
            double M_n = col.GetNominalMomentResult(P_u, FlexuralCompressionFiberPosition.Top).Moment;
            double refValue = 257 * 12 * 1000; //from MacGregor
            double actualTolerance = EvaluateActualTolerance(M_n, refValue);

            Assert.True(actualTolerance <= tolerance);
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

                //ConcreteSectionCompression column = GetConcreteCompressionMember(b, h, f_c, LongitudinalBars, CompressionMemberType.NonPrestressedWithTies);
                ConcreteSectionCompression column = GetConcreteCompressionMember(b, h, f_c, LongitudinalBars, ConfinementReinforcementType.Ties);
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

            ConcreteSectionCompression column = compressionFactory.GetCompressionMemberFromFlexuralSection(flexureMember, ConfinementReinforcementType.Ties);

            return column;

        }

        public ConcreteSectionCompression GetConcreteCompressionMember(double Width, double Height, double fc, List<RebarPoint> LongitudinalBars, ConfinementReinforcementType ConfinementReinforcementType)
        {
            CalcLog log = new CalcLog();
            IConcreteSection Section = GetRectangularSection(Width, Height, fc);
 
            IConcreteFlexuralMember fs = new ConcreteSectionFlexure(Section, LongitudinalBars, new CalcLog(), ConfinementReinforcementType);
            ConcreteSectionCompression column = new ConcreteSectionCompression(fs as IConcreteSectionWithLongitudinalRebar, ConfinementReinforcementType, log);
            return column;
        }
    }
}
