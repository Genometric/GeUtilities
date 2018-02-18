﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.TRefSeqParser
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
            var rg = new RegionGenerator()
            {
                ChrColumn = chrColumn,
                LeftColumn = leftColumn,
                RightColumn = rightColumn,
                RefSeqIDColumn = refSeqIDColumn,
                GeneSymbolColumn = geneSymbolColumn,
                StrandColumn = strandColumn
            };

            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new RefSeqParser<Gene>(testFile.TempFilePath, rg.Columns);
                var parsedGene = parser.Parse().Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(rg.Gene) == 0);
            }
        }
    }
}
