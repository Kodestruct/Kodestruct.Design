 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Tests.Common;
using Kodestruct.Concrete.ACI318_14;
using Xunit;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
    public partial class AciStandardHookTests
    {
        //PCA Notes on ACI318-08
        //Table 4-4 Development Length ldh (inches) of Standard Hooks for Uncoated Grade 60 Bars

        [Fact]
        public void GetDevelopmentLength_PCAExample_4000psiN6bar_ReturnsValue()
        {
            double ConcStrength = 4000;
            double RebarDiam = 0.75;
            StandardHookInTension stdHook = CreateHookObject(ConcStrength, RebarDiam);
            double ldh = Math.Round(stdHook.GetDevelopmentLength(), 1);
            Assert.Equal(14.2, ldh);
        }

        [Fact]
        public void GetDevelopmentLength_PCAExample_3000psiN18bar_ReturnsValue()
        {
            double ConcStrength = 3000;
            double RebarDiam = 2.257;
            StandardHookInTension stdHook = CreateHookObject(ConcStrength, RebarDiam);
            double ldh = Math.Round(stdHook.GetDevelopmentLength(), 0);
            Assert.Equal(49.0, ldh); //PCA is 49.5
        }

        [Fact]
        public void GetDevelopmentLength_PCAExample_10000psiN3bar_ReturnsValue()
        {
            double ConcStrength = 10000;
            double RebarDiam = 0.375;
            StandardHookInTension stdHook = CreateHookObject(ConcStrength, RebarDiam);
            double ldh = Math.Round(stdHook.GetDevelopmentLength(), 0);
            Assert.Equal(6.0, ldh);
        }
    }
}
