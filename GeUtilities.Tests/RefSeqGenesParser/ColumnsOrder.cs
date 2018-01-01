// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.RefSeqGenesParser
{
    public class ColumnsOrder
    {
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
            byte refSeqIDColumn,
            byte geneSymbolColumn,
            sbyte strandColumn)
        {
            // Arrange
            var columns = new Columns()
            {
                ChrColumn = chrColumn,
                LeftColumn = leftColumn,
                RightColumn = rightColumn,
                RefSeqIDColumn = refSeqIDColumn,
                GeneSymbolColumn = geneSymbolColumn,
                StrandColumn = strandColumn
            };

            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqGenesParser<Gene> parser = new RefSeqGenesParser<Gene>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    refSeqIDColumn: columns.RefSeqIDColumn,
                    geneSymbolColumn: columns.GeneSymbolColumn,
                    strandColumn: columns.StrandColumn);
                var parsedGene = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(columns.Gene) == 0);
            }
        }
    }
}
