// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.BEDParser
{
    public class ColumnsOrder
    {
        [Fact]
        public void DefaultColumnsOrder()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator())
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, dropPeakIfInvalidValue: true);
                var parsedPeak = bedParser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(columns.Peak) == 0);
            }
        }

        [Theory]
        [InlineData(0, 1, 2, 3, 4)]
        [InlineData(1, 0, 2, 3, 4)]
        [InlineData(1, 2, 0, 3, 4)]
        [InlineData(1, 2, 3, 0, 4)]
        [InlineData(1, 2, 3, 4, 0)]
        [InlineData(4, 3, 2, 1, 0)]
        [InlineData(2, 1, 0, 4, 3)]
        [InlineData(5, 6, 7, 8, 9)]
        public void ColumnsShuffle(byte chrColumn, byte leftColumn, sbyte rightColumn, byte nameColumn, byte valueColumn)
        {
            // Arrange
            var columns = new Columns(chrColumn: chrColumn, leftColumn: leftColumn, rightColumn: rightColumn, nameColumn: nameColumn, valueColumn: valueColumn);
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: chrColumn,
                    leftEndColumn: leftColumn,
                    rightEndColumn: rightColumn,
                    nameColumn: nameColumn,
                    valueColumn: valueColumn,
                    strandColumn: -1);
                var parsedPeak = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(columns.Peak) == 0);
            }
        }

        [Fact]
        public void ColumnsSetters()
        {
            // Arrange
            var columns = new Columns();
            columns.ChrColumn = 2;
            columns.LeftColumn = 2;
            columns.RightColumn = 9;
            columns.ValueColumn = 0;
            columns.SummitColumn = 2;
            columns.NameColumn = 2;
            columns.StrandColumn = 0;
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: columns.ValueColumn,
                    summitColumn: columns.SummitColumn,
                    strandColumn: columns.StrandColumn);
                var parsedPeak = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(columns.Peak) == 0);
            }
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(0, -1)]
        [InlineData(0, 99)]
        [InlineData(3, -1)]
        [InlineData(3, 99)]
        [InlineData(10, -1)]
        [InlineData(10, 99)]
        public void TestSummit(sbyte summitColumn, int summit)
        {
            // Arrange
            var columns = new Columns { SummitColumn = summitColumn };
            columns.Peak.Summit = summit == -1 ? columns.Peak.Left + ((columns.Peak.Right - columns.Peak.Left) / 2) : summit;
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: columns.ValueColumn,
                    summitColumn: columns.SummitColumn,
                    strandColumn: columns.StrandColumn);
                var parsedPeak = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.Summit == columns.Peak.Summit);
            }
        }

        [Theory]
        [InlineData(0, '*')]
        [InlineData(3, '*')]
        [InlineData(9, '*')]
        [InlineData(1, '+')]
        [InlineData(2, '-')]
        [InlineData(-1, '*')]
        public void TestStrand(sbyte strandColumn, char strand)
        {
            // Arrange
            var columns = new Columns(strand: strand) { StrandColumn = strandColumn };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: columns.ValueColumn,
                    strandColumn: columns.StrandColumn);

                // Assert
                Assert.True(parser.Parse().Chromosomes[columns.Chr].Strands.ContainsKey(strand));
            }
        }

        [Fact]
        public void TestMultiStrand()
        {
            // Arrange
            string[] peaks = new string[]
            {
                "chr1\t10\t20\t*\tGeUtilities_00\t100.0",
                "chr1\t30\t40\t+\tGeUtilities_01\t110.0",
                "chr1\t50\t60\t-\tGeUtilities_02\t111.0",
                "chr1\t50\t60\t#\tGeUtilities_02\t111.0", // Any strand name other than '+', '-', and '*' will be parsed as '*'.
            };
            using (TempFileCreator testFile = new TempFileCreator(peaks))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: 0,
                    leftEndColumn: 1,
                    rightEndColumn: 2,
                    strandColumn: 3,
                    nameColumn: 4,
                    valueColumn: 5);

                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes["chr1"].Strands.ContainsKey('*'));
                Assert.True(parsedData.Chromosomes["chr1"].Strands.ContainsKey('+'));
                Assert.True(parsedData.Chromosomes["chr1"].Strands.ContainsKey('-'));
                Assert.False(parsedData.Chromosomes["chr1"].Strands.ContainsKey('#'));
            }
        }
    }
}
