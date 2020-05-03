
using Kodestruct.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI.ACI318_14;
using Kodestruct.Concrete.ACI.Entities;
using System.IO;
using Kodestruct.Concrete.ACI318_14.Tests.Output.CSV;
using Xunit;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
     
    public partial class AciCompressionSquareColumnTests : ConcreteTestBase
    {
        [Fact]
        public void ColumnReturnsNominalInteractionDiagram()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            List<PMPair> Pairs = col.GetPMPairs(FlexuralCompressionFiberPosition.Top,50,false);
            var PairsAdjusted = Pairs.Select(pair => new PMPair(pair.P / 1000.0, pair.M / 1000.0 / 12.0));

            string Filename = Path.Combine(Path.GetTempPath(), "PMInteractionMacGregor.csv");
            using (CsvFileWriter writer = new CsvFileWriter(Filename))
            {
                foreach (var pair in PairsAdjusted)
                {
                    CsvRow row = new CsvRow();
                    row.Add(pair.M.ToString());
                    row.Add(pair.P.ToString());
                    writer.WriteRow(row);

                }
            }

        }
        [Fact]
        public void ColumnReturnsReducedInteractionDiagram()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            List<PMPair> Pairs = col.GetPMPairs(FlexuralCompressionFiberPosition.Top, 50, true);
            var PairsAdjusted = Pairs.Select(pair => new PMPair(pair.P / 1000.0, pair.M / 1000.0 / 12.0));

            string Filename = Path.Combine(Path.GetTempPath(), "PMInteractionMacGregorReduced.csv");
            using (CsvFileWriter writer = new CsvFileWriter(Filename))
            {
                foreach (var pair in PairsAdjusted)
                {
                    CsvRow row = new CsvRow();
                    row.Add(pair.M.ToString());
                    row.Add(pair.P.ToString());
                    writer.WriteRow(row);

                }
            }

        }

        [Fact]
        public void ColumnReturnsNegativeNominalInteractionDiagram()
        {
            ConcreteSectionCompression col = GetConcreteExampleColumn();
            List<PMPair> Pairs = col.GetPMPairs(FlexuralCompressionFiberPosition.Bottom, 50, false);
            var PairsAdjusted = Pairs.Select(pair => new PMPair(pair.P / 1000.0, pair.M / 1000.0 / 12.0));

            string Filename = Path.Combine(Path.GetTempPath(), "PMInteractionMacGregorNegative.csv");
            using (CsvFileWriter writer = new CsvFileWriter(Filename))
            {
                foreach (var pair in PairsAdjusted)
                {
                    CsvRow row = new CsvRow();
                    row.Add(pair.M.ToString());
                    row.Add(pair.P.ToString());
                    writer.WriteRow(row);

                }
            }

        }
    }
}
