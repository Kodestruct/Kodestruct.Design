//Sample license text.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kodestruct.Concrete.ACI318_14;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
    [TestFixture]
    public partial class AciTensionDevelopmentTests
    {


        [Test]
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
            Assert.AreEqual(0.8, Ktr);

        }


        [Test]
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
            Assert.AreEqual(0, Ktr);
        }

        [Test]
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
            Assert.AreEqual(0, Ktr);
        }
        [Test]
        public void GetConfinementTerm_NormalPCAvalues_ReturnsValue()
        {

            double cb = 1.17;
            double Ktr = 0.8;
            double RebarDiam = 1.0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double ConfinementTerm = tensDev.GetConfinementTerm(cb, Ktr);
            Assert.AreEqual(1.97, ConfinementTerm);
        }

        [Test]

        public void GetConfinementTerm_HighValue_Returns2_5()
        {

            double cb = 1.0;
            double Ktr = 100;
            double RebarDiam = 1.0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double ConfinementTerm = tensDev.GetConfinementTerm(cb, Ktr);
            Assert.AreEqual(2.5, ConfinementTerm);
        }

        [Test]

        public void GetConfinementTerm_0RebarDiam_throws()
        {

            double cb = 1.0;
            double Ktr = 100;
            double RebarDiam = 0.0;
            double ClearSpacing = 0;
            double ClearCover = 0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);

            var ex = Assert.Throws<Exception>(() => tensDev.GetConfinementTerm(cb, Ktr));
            Assert.That(ex.Message, Is.EqualTo("Rebar diameter cannot be 0.0. Check input"));


        }
    }
}
