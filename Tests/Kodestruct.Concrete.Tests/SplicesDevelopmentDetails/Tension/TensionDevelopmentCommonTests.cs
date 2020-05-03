 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Tests.Common;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14;
using Xunit;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
     
    public partial class AciTensionDevelopmentTests : ToleranceTestBase
    {


        [Fact]
        public void GetSqrt_fc_NormalVal_ReturnsValue()
        {
            double concreteStrength = 10000.0;
            DevelopmentTension tensDev = CreateDevelopmentObject(concreteStrength, 1.0, 1.0, false, 1.0, false);
            double sqrt_fc = tensDev.GetSqrt_fc();
            Assert.Equal(100.0, sqrt_fc);
        }


        [Fact]
        public void GetSqrt_fc_HighVal_Returns100()
        {
            double concreteStrength = 20000.0;
            DevelopmentTension tensDev = CreateDevelopmentObject(concreteStrength, 1.0, 1.0, false, 1.0, false);
            double sqrt_fc = tensDev.GetSqrt_fc();
            Assert.Equal(100.0, sqrt_fc);
        }



        [Fact]
        public void ExcessReinf_NormalValue_ReturnsReduced()
        {
            double concreteStrength = 5000.0;
            double excessReinf = 0.5;
            double ld = 20;
            DevelopmentTension tensDev = CreateDevelopmentObject(concreteStrength, 1.0, 1.0, false, excessReinf, false);
            tensDev.ExcessFlexureReinforcementRatio = excessReinf;
            double ldReduced = tensDev.CheckExcessReinforcement(ld, true, false);
            Assert.Equal(ld * excessReinf, ldReduced);
        }


        [Fact]
        //[ExpectedException(ExpectedMessage = "Exceess reinforcement ratio cannot be more than 1.0")]
        public void ExcessReinf_ExcessRebarMoreThan1_ThrowsException()
        {
            double excessReinf = 2.0;
            double ld = 20;
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, 1.0, 1.0, false, excessReinf, false);
            tensDev.ExcessFlexureReinforcementRatio = excessReinf;
            //double ldReduced = tensDev.CheckExcessReinforcement(ld, true, false);

            var ex = Assert.Throws<Exception>(() => tensDev.CheckExcessReinforcement(ld, true, false));
            Assert.True(ex.Message ==  "Exceess reinforcement ratio cannot be more than 1.0");

        }

        [Fact]
        public void CheckLambda_NormalConc_Returns1()
        {
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, ConcreteTypeByWeight.Normalweight, 0.0, 1.0, 1.0, false, 1.0, false);
            double lambda = tensDev.CheckLambda(1.0);
            Assert.Equal(1.0, lambda);
        }
        [Fact]
        public void CheckLambda_LWConc_Returns0_75()
        {
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, ConcreteTypeByWeight.Lightweight, 0.0, 1.0, 1.0, false, 1.0, false);
            double lambda = tensDev.CheckLambda(0.5);
            Assert.Equal(0.5, lambda);
        }
        [Fact]
        public void CheckLambda_LWConcNoSplitStrength_ReturnsLargerThan0_75()
        {
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, ConcreteTypeByWeight.Lightweight, 0.0, 1.0, 1.0, false, 1.0, false);
            double lambda = tensDev.CheckLambda(0.85);
            Assert.Equal(0.75, lambda);
        }
        [Fact]
        public void CheckLambda_LWConcWithSplitStrength_ReturnsLargerThan0_75()
        {
            DevelopmentTension tensDev = CreateDevelopmentObject(4000.0, ConcreteTypeByWeight.Lightweight, 100.0, 1.0, 1.0, false, 1.0, false);
            double lambda = tensDev.CheckLambda(0.85);
            Assert.Equal(0.85, lambda);
        }

    }

}
