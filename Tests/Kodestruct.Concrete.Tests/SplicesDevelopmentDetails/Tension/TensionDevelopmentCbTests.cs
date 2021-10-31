 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Tests.Common;
using Kodestruct.Concrete.ACI318_14;
using Xunit;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
     
    public partial class AciTensionDevelopmentTests : ToleranceTestBase
    {

        [Fact]
        public void GetCb_NormalValuesClearCover_ReturnsValue()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 2.0;
            double ClearCover = 1.0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double Cb = tensDev.GetCb();
            Assert.Equal(1.5, Cb);
        }

        [Fact]
        public void GetCb_NormalValuesClearSpacing_ReturnsValue()
        {
            double RebarDiam = 1.0;
            double ClearSpacing = 1.0;
            double ClearCover = 3.0;
            DevelopmentTension tensDev = this.CreateDevelopmentObject(RebarDiam, ClearSpacing, ClearCover);
            double Cb = tensDev.GetCb();
            Assert.Equal(1.0, Cb);
        }



    }
}
