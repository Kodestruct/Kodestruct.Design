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
    public partial class AciTensionDevelopmentTests : ToleranceTestBase
    {

        [Test]
        public void GetCb_NormalValuesClearCover_ReturnsValue()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 2.0;
            double ClearCover = 1.0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double Cb = tensDev.GetCb();
            Assert.AreEqual(1.5, Cb);
        }

        [Test]
        public void GetCb_NormalValuesClearSpacing_ReturnsValue()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 1.0;
            double ClearCover = 3.0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double Cb = tensDev.GetCb();
            Assert.AreEqual(1.0, Cb);
        }



    }
}
