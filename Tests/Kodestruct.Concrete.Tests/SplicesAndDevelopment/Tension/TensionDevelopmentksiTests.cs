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
    public partial class TensionDevelopmentTests
    {

        [Test]
        public void GetKsi_t_TopRebar_Returns1_3()
        {
            bool isTopRebar = true;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(4000, 0.0, 0.0, isTopRebar, 1.0, false);
            double ksi_t = tensDev.GetKsi_t();
            Assert.AreEqual(1.3, ksi_t);
        }

        [Test]
        public void GetKsi_t_BotRebar_Returns1_0()
        {
            bool isTopRebar = false;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(4000, 0.0, 0.0, isTopRebar, 1.0, false);
            double ksi_t = tensDev.GetKsi_t();
            Assert.AreEqual(1.0, ksi_t);
        }

        [Test]
        public void GetKsi_e_NotEpoxyBar_Returns1_0()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 2.0;
            double ClearCover = 1.0;
            bool IsEpoxyCoated = false;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, IsEpoxyCoated, ClearSpacing, ClearCover);
            double ksi_e = tensDev.GetKsi_e();
            Assert.AreEqual(1.0, ksi_e);
        }

        [Test]
        public void GetKsi_e_EpoxyBar_Returns1_2()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 6.0;
            double ClearCover = 3.0;
            bool IsEpoxyCoated = true;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, IsEpoxyCoated, ClearSpacing, ClearCover);
            double ksi_e = tensDev.GetKsi_e();
            Assert.AreEqual(1.2, ksi_e);
        }

        [Test]
        public void GetKsi_e_EpoxyBarLargeSpacingAndCover_Returns1_2()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 8.0;
            double ClearCover = 4.0;
            bool IsEpoxyCoated = true;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, IsEpoxyCoated, ClearSpacing, ClearCover);
            double ksi_e = tensDev.GetKsi_e();
            Assert.AreEqual(1.2, ksi_e);
        }

        [Test]
        public void GetKsi_e_EpoxyBarSmallSpacingAndCover_Returns1_5()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 2.0;
            double ClearCover = 1.0;
            bool IsEpoxyCoated = true;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, IsEpoxyCoated, ClearSpacing, ClearCover);
            double ksi_e = tensDev.GetKsi_e();
            Assert.AreEqual(1.5, ksi_e);
        }
        [Test]
        public void GetKsi_e_EpoxyBarSmallSpacing_Returns1_5()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 2.0;
            double ClearCover = 10.0;
            bool IsEpoxyCoated = true;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, IsEpoxyCoated, ClearSpacing, ClearCover);
            double ksi_e = tensDev.GetKsi_e();
            Assert.AreEqual(1.5, ksi_e);
        }

        [Test]
        public void GetKsi_e_EpoxyBarSmallCover_Returns1_5()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 12.0;
            double ClearCover = 1.0;
            bool IsEpoxyCoated = true;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, IsEpoxyCoated, ClearSpacing, ClearCover);
            double ksi_e = tensDev.GetKsi_e();
            Assert.AreEqual(1.5, ksi_e);
        }
    }
}
