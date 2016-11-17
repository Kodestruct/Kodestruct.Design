//Sample license text.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14;


namespace Kodestruct.Concrete.ACI318_14.Tests
{
    [TestFixture]
    public partial class AciTensionDevelopmentTests : ToleranceTestBase
    {


        [Test]
        public void GetSqrt_fc_NormalVal_ReturnsValue()
        {
            double concreteStrength = 10000.0;
            DevelopmentTension tensDev = CreateDevelopmentObject(concreteStrength, 1.0, 1.0, false, 1.0, false);
            double sqrt_fc = tensDev.GetSqrt_fc();
            Assert.AreEqual(100.0, sqrt_fc);
        }


        [Test]
        public void GetSqrt_fc_HighVal_Returns100()
        {
            double concreteStrength = 20000.0;
            DevelopmentTension tensDev = CreateDevelopmentObject(concreteStrength, 1.0, 1.0, false, 1.0, false);
            double sqrt_fc = tensDev.GetSqrt_fc();
            Assert.AreEqual(100.0, sqrt_fc);
        }



        [Test]
        public void ExcessReinf_NormalValue_ReturnsReduced()
        {
            double concreteStrength = 5000.0;
            double excessReinf = 0.5;
            double ld = 20;
            DevelopmentTension tensDev = CreateDevelopmentObject(concreteStrength, 1.0, 1.0, false, excessReinf, false);
            tensDev.ExcessFlexureReinforcementRatio = excessReinf;
            double ldReduced = tensDev.CheckExcessReinforcement(ld, true, false);
            Assert.AreEqual(ld * excessReinf, ldReduced);
        }


        [Test]
        //[ExpectedException(ExpectedMessage = "Exceess reinforcement ratio cannot be more than 1.0")]
        public void ExcessReinf_ExcessRebarMoreThan1_ThrowsException()
        {
            double excessReinf = 2.0;
            double ld = 20;
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, 1.0, 1.0, false, excessReinf, false);
            tensDev.ExcessFlexureReinforcementRatio = excessReinf;
            //double ldReduced = tensDev.CheckExcessReinforcement(ld, true, false);

            var ex = Assert.Throws<Exception>(() => tensDev.CheckExcessReinforcement(ld, true, false));
            Assert.That(ex.Message, Is.EqualTo("Exceess reinforcement ratio cannot be more than 1.0"));

        }

        [Test]
        public void CheckLambda_NormalConc_Returns1()
        {
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, ConcreteTypeByWeight.Normalweight, 0.0, 1.0, 1.0, false, 1.0, false);
            double lambda = tensDev.CheckLambda(1.0);
            Assert.AreEqual(1.0, lambda);
        }
        [Test]
        public void CheckLambda_LWConc_Returns0_75()
        {
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, ConcreteTypeByWeight.Lightweight, 0.0, 1.0, 1.0, false, 1.0, false);
            double lambda = tensDev.CheckLambda(0.5);
            Assert.AreEqual(0.5, lambda);
        }
        [Test]
        public void CheckLambda_LWConcNoSplitStrength_ReturnsLargerThan0_75()
        {
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, ConcreteTypeByWeight.Lightweight, 0.0, 1.0, 1.0, false, 1.0, false);
            double lambda = tensDev.CheckLambda(0.85);
            Assert.AreEqual(0.75, lambda);
        }
        [Test]
        public void CheckLambda_LWConcWithSplitStrength_ReturnsLargerThan0_75()
        {
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, ConcreteTypeByWeight.Lightweight, 100.0, 1.0, 1.0, false, 1.0, false);
            double lambda = tensDev.CheckLambda(0.85);
            Assert.AreEqual(0.85, lambda);
        }

    }

}
