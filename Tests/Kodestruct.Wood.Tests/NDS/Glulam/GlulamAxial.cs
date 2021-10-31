using Kodestruct.Wood.NDS.Entities;
using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.NDS.NDS_2015.Entities.Column;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kodestruct.Wood.Tests.NDS.Glulam
{
    public partial class GlulamTests
    {
        [Fact]
        public void DFSoftwoodAxiaMemberReturnsCompressiveStrength()
        {
            GlulamColumn column = new GlulamColumn(6.75, 7.5, 5, GlulamSoftWoodAxialCombinationSymbol.c2, 180, 80, MoistureCondition.Dry, false);
            double phiP_n = column.GetCompressionStrength(Wood.NDS.LoadCombinationType.FullWind);
            double forceRatio = (phiP_n / 1000.0) / (45.6*1.5);
            Assert.True(forceRatio > 0.95 && forceRatio < 1.05);
        }

    }
}
