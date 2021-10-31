using Kodestruct.Wood.NDS.NDS2015;
using Kodestruct.Wood.NDS.NDS2015.ShearWall;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kodestruct.Wood.Tests.NDS.ShearWalls
{
    public class ShearWallTests
    {
        [Fact]
        public void WoodBasedPanelReturnsWindShearStrength()
        {
            WoodFrameShearWallWithWoodPanel panel = new WoodFrameShearWallWithWoodPanel();
            var strength = panel.GetShearUnitStrength("WoodStructuralPanel", "Structural_I", "15/32", 4, false, WoodPanelType.Plywood);
            double v = strength.v;
            Assert.Equal(1205, v);
        }
    }
}
