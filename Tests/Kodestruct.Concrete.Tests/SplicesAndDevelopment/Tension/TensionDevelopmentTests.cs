using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI.Entities;


namespace Kodestruct.Concrete.ACI318_14.Tests
{
    [TestFixture]
    public partial class TensionDevelopmentTests
    {
        //PCA Notes on ACI 318-08
        //Example 4.1—Development of Bars in Tension

        //Lightweight concrete
        //2.5 in. clear cover to stirrups
        //Epoxy-coated bars
        //fc' = 4000 psi
        //fy = 60,000 psi
        // Nominal diameter of No. 9 bar = 1.128 in.
        // Clear spacing between spliced bars 21.7 in.

        [Test]
        public void GetTensionDevelopmentLength_PCAExample_4_1_Case1_ReturnsValue()
        {
            double RebarDiam = 1.1280;
            bool IsTopRebar = true;
            bool IsEpoxyCoated = true;
           ConcreteTypeByWeight type = ConcreteTypeByWeight.Lightweight;
            double ClearSpacing = 21.7;
            double ClearCover = 3.0;
            double fc = 4000;
            double ExcessRebarRatio = 1.0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(fc, RebarDiam, IsEpoxyCoated, type,
                0.0, ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, true);
            double ld = Math.Round(tensDev.GetTensionDevelopmentLength(true), 1);
            Assert.AreEqual(121.3, ld);
        }

        [Test]
        public void GetTensionDevelopmentLength_PCAExample_4_1_Case2_ReturnsValue()
        {
            double RebarDiam = 1.1280;
            bool IsTopRebar = true;
            bool IsEpoxyCoated = true;
            ConcreteTypeByWeight type = ConcreteTypeByWeight.Lightweight;
            double ClearSpacing = 21.7;
            double ClearCover = 3.0;
            double fc = 4000;
            double ExcessRebarRatio = 1.0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(fc, RebarDiam, IsEpoxyCoated, type,
                0.0, ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, true);
            double ld = Math.Round(tensDev.GetTensionDevelopmentLength(100, 10, 2), 1);
            Assert.AreEqual(72.8, ld);
        }

        [Test]
        public void GetTensionDevelopmentLength_PCAExample_4_2_ReturnsValue()
        {
            //need to find way to add sand light weight
        }

        //  No. 8 bars fc' = 4000 psi (normalweight concrete) and
        //  fy = 60,000 psi, and uncoated bars. Stirrups provided satisfy the minimum code requirements for beam shear
        //  reinforcement.
        [Test]
        public void GetTensionDevelopmentLength_PCAExample_4_3_Case1_ReturnsValue()
        {
            double RebarDiam = 1.0;
            bool IsTopRebar = true;
            bool IsEpoxyCoated = false;
            ConcreteTypeByWeight type = ConcreteTypeByWeight.Normalweight;
            double ClearSpacing = 1.33;
            double ClearCover = 2.0;
            double fc = 4000;
            double ExcessRebarRatio = 1.0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(fc, RebarDiam, IsEpoxyCoated, type,
                0.0, ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, true);
            double ld = Math.Round(tensDev.GetTensionDevelopmentLength(true), 1);
            Assert.AreEqual(61.7, ld);
        }
        [Test]
        public void GetTensionDevelopmentLength_PCAExample_4_3_Case2_ReturnsValue()
        {
            double RebarDiam = 1.0;
            bool IsTopRebar = true;
            bool IsEpoxyCoated = false;
            ConcreteTypeByWeight type = ConcreteTypeByWeight.Normalweight;
            double ClearSpacing = 1.33;
            double ClearCover = 2.0;
            double fc = 4000;
            double ExcessRebarRatio = 1.0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(fc, RebarDiam, IsEpoxyCoated, type,
                0.0, ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, true);
            double ld = Math.Round(tensDev.GetTensionDevelopmentLength(0.4, 10, 2), 0);
            Assert.AreEqual(47.0, ld);
        }
    }
}
