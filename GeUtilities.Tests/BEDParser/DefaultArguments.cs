// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.TBEDParser
{
    public class DefaultArguments
    {
        [Fact]
        public void DefaultColumnsOrder()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator())
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedPeak = bedParser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(columns.Peak) == 0);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(2, 2)]
        public void AvoidHeader(int headerCount, byte readOffset)
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator(new RegionGenerator(), headerLineCount: headerCount))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                parser.ReadOffset = readOffset;
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 1);
            }
        }

        [Theory]
        [InlineData("chr1")]
        [InlineData("Chr1")]
        [InlineData("chr10")]
        [InlineData("Chr10")]
        [InlineData("chrX")]
        [InlineData("ChrX")]
        public void ReadChr(string chr)
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator(new RegionGenerator { Chr = chr }))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Theory]
        [InlineData("chr2")]
        [InlineData("chrX")]
        public void FailReadChr(string chr)
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator(new RegionGenerator { Chr = "chr1" }))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Fact]
        public void ReadStrand()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // ACt
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands.ContainsKey(columns.Strand));
            }
        }

        [Fact]
        public void ReadLeft()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Left == columns.Left);
            }
        }

        [Fact]
        public void FailReadLeft()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10V\t20\tGeUtilities_01\t123.4"))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadRight()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Right == columns.Right);
            }
        }

        [Fact]
        public void FailReadRightInvalidValue()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10\t20V\tGeUtilities_01\t123.4"))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailReadRightColumnIndexOutOfRange()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123.4"))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, 0, 1, 10, 3, 4);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadName()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Name == columns.Name);
            }
        }

        [Fact]
        public void ReadValue()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Value == columns.Value);
            }
        }

        [Fact]
        public void FailReadValue()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void AssignHashKey()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].HashKey != 0);
            }
        }
    }
}
