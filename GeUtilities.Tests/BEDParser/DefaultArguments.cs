// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers;
using Genometric.GeUtilities.IntervalParsers.Model.Columns;
using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
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
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator())
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedPeak = parser.Parse().Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(rg.Peak) == 0);
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
            using (var testFile = new TempFileCreator(new RegionGenerator(), headerLineCount: headerCount))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    ReadOffset = readOffset
                };
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
            using (var testFile = new TempFileCreator(new RegionGenerator { Chr = chr }))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
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
            using (var testFile = new TempFileCreator(new RegionGenerator { Chr = "chr1" }))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Fact]
        public void ReadStrand()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // ACt
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands.ContainsKey(rg.Strand));
            }
        }

        [Fact]
        public void ReadLeft()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Left == rg.Left);
            }
        }

        [Fact]
        public void FailReadLeft()
        {
            // Arrange
            using (var testFile = new TempFileCreator("chr1\t10V\t20\tGeUtilities_01\t123.4"))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadRight()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Right == rg.Right);
            }
        }

        [Fact]
        public void FailReadRightInvalidValue()
        {
            // Arrange
            using (var testFile = new TempFileCreator("chr1\t10\t20V\tGeUtilities_01\t123.4"))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailReadRightColumnIndexOutOfRange()
        {
            // Arrange
            using (var testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123.4"))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, new BEDColumns() { Right = 10 });
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadName()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Name == rg.Name);
            }
        }

        [Fact]
        public void ReadValue()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Value == rg.Value);
            }
        }

        [Fact]
        public void FailReadValue()
        {
            // Arrange
            using (var testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void AssignHashKey()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].HashKey != 0);
            }
        }
    }
}
