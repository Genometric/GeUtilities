// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.RefSeqGenesParserTests
{
    public class TestColumnOrders
    {
        private string _chr = "chr1";

        [Theory]
        [InlineData(0, 1, 2, 3, 4, -1)]
        [InlineData(1, 0, 2, 3, 4, 5)]
        [InlineData(1, 2, 0, 3, 4, -1)]
        [InlineData(1, 2, 3, 0, 4, -1)]
        [InlineData(1, 2, 3, 4, 0, -1)]
        [InlineData(4, 3, 2, 1, 0, -1)]
        [InlineData(2, 1, 0, 4, 3, -1)]
        [InlineData(5, 6, 7, 8, 9, 2)]
        public void TestColumnsShuffle(
            byte chrColumn,
            byte leftColumn,
            sbyte rightColumn,
            sbyte refSeqIDColumn,
            sbyte officialGeneSymbolColumn,
            sbyte strandColumn)
        {
            var refSeqColumns = new RefSeqColumns()
            {
                ChrColumn = chrColumn,
                LeftColumn = leftColumn,
                RightColumn = rightColumn,
                RefSeqIDColumn = refSeqIDColumn,
                OfficialGeneSymbolColumn = officialGeneSymbolColumn,
                StrandColumn = strandColumn
            };

            int left = 10, right = 20;
            string refSeqID = "refSeqID", geneSymbol = "geneSymbol";
            char strand = '*';

            using (TempRefSeqGenesFileCreator testFile = new TempRefSeqGenesFileCreator(
                refSeqColumns,
                chr: _chr,
                left: left.ToString(),
                right: right.ToString(),
                refSeqID: refSeqID,
                geneSymbol: geneSymbol,
                strand: strand.ToString()))
            {
                RefSeqGenesParser<Gene> refSeqParser = new RefSeqGenesParser<Gene>(
                    testFile.TempFilePath,
                    chrColumn: refSeqColumns.ChrColumn,
                    leftEndColumn: refSeqColumns.LeftColumn,
                    rightEndColumn: refSeqColumns.RightColumn,
                    refSeqIDColumn: refSeqColumns.RefSeqIDColumn,
                    officialGeneSymbolColumn: refSeqColumns.OfficialGeneSymbolColumn,
                    strandColumn: refSeqColumns.StrandColumn);
                var parsedGene = refSeqParser.Parse().Chromosomes[_chr].Strands[strand].Intervals[0];

                Assert.True(
                    parsedGene.Left == left &&
                    parsedGene.Right == right &&
                    parsedGene.RefSeqID == refSeqID &&
                    parsedGene.GeneSymbol == geneSymbol);
            }
        }
    }
}
