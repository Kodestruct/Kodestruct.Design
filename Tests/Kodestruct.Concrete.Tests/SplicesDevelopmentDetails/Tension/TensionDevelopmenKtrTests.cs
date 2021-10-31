 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Tests.Common;
using Kodestruct.Concrete.ACI318_14;
using Xunit;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
     
    public partial class AciTensionDevelopmentTests
    {


        [Fact]
        public void GetKtr_NormalPCAValues_ReturnsCorrectValue()
        {
            //ACI318-08 PCA notes page 4-38
            //Example 4.3
            double Atr = 0.4;
            double s = 10.0;
            double n = 2.0;
            double RebarDiam = 0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double Ktr = tensDev.GetKtr(Atr, s, n);
            Assert.Equal(0.8, Ktr);

        }


        [Fact]
        public void GetKtr_ZeroSpacing_Returns0()
        {
            double Atr = 0.4;
            double s = 0.0;
            double n = 2.0;
            double RebarDiam = 0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double Ktr = tensDev.GetKtr(Atr, s, n);
            Assert.Equal(0, Ktr);
        }

        [Fact]
        public void GetKtr_ZeroNumber_Returns0()
        {
            double Atr = 0.4;
            double s = 10.0;
            double n = 0.0;
            double RebarDiam = 0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double Ktr = tensDev.GetKtr(Atr, s, n);
            Assert.Equal(0, Ktr);
        }
        [Fact]
        public void GetConfinementTerm_NormalPCAvalues_ReturnsValue()
        {

            double cb = 1.17;
            double Ktr = 0.8;
            double RebarDiam = 1.0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double ConfinementTerm = tensDev.GetConfinementTerm(cb, Ktr);
            Assert.Equal(1.97, ConfinementTerm);
        }

        [Fact]

        public void GetConfinementTerm_HighValue_Returns2_5()
        {

            double cb = 1.0;
            double Ktr = 100;
            double RebarDiam = 1.0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double ConfinementTerm = tensDev.GetConfinementTerm(cb, Ktr);
            Assert.Equal(2.5, ConfinementTerm);
        }

        [Fact]

        public void GetConfinementTerm_0RebarDiam_throws()
        {

            double cb = 1.0;
            double Ktr = 100;
            double RebarDiam = 0.0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);

            var ex = Assert.Throws<Exception>(() => tensDev.GetConfinementTerm(cb, Ktr));
            Assert.True(ex.Message == "Rebar diameter cannot be 0.0. Check input");


        }
    }
}
