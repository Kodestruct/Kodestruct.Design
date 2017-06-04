 
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

        public AciCompressionSquareColumnTests()
        {
            tolerance = 0.05; //5% can differ from rounding
        }

        double tolerance;

        //EXAMPLE 11-1 Calculation of an Interaction Diagram

        [Test]
        public void ColumnInteractionReturnsM_n_Z_Neg1Factored()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double phiP_n = 271*1000;
            double phiM_nRef = 251.0 * 1000*12;

            double phiM_n = col.GetDesignMomentWithCompressionStrength(phiP_n, FlexuralCompressionFiberPosition.Top).phiM_n;
            double actualTolerance = EvaluateActualTolerance(phiM_n, phiM_nRef);

            
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnInteractionReturnsM_n_Z_Neg2Factored()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double phiP_n = 205 * 1000;
            double phiM_nRef = 282.0 * 1000 * 12;

            double phiM_n = col.GetDesignMomentWithCompressionStrength(phiP_n, FlexuralCompressionFiberPosition.Top).phiM_n;
            double actualTolerance = EvaluateActualTolerance(phiM_n, phiM_nRef);


            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnInteractionReturnsM_n_Z_Neg4Factored()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            double phiP_n = 39.5 * 1000;
            double refValue = 232 * 12 * 1000; //from MacGregor


            double phiM_n = col.GetDesignMomentWithCompressionStrength(phiP_n, FlexuralCompressionFiberPosition.Top).phiM_n;


            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void ColumnDistributedInteractionReturnsM_n_Z_Neg4Factored()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumnWithDistributed();
            double phiP_n = 39.5 * 1000;
            double refValue = 232 * 12 * 1000; //from MacGregor


            double phiM_n = col.GetDesignMomentWithCompressionStrength(phiP_n, FlexuralCompressionFiberPosition.Top).phiM_n;

            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }



    }
}
