// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Parsers;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.GTF
{
    public class DefaultArguments
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(2, 2)]
        public void AvoidHeader(int headerCount, byte readOffset)
        {
            // Arrange
            using (var file = new TempFileCreator(new RegionGenerator(), headerLineCount: headerCount))
            {
                // Act
                var parser = new GtfParser()
                {
                    ReadOffset = readOffset
                };
                var parsedData = parser.Parse(file.TempFilePath);

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
            var rg = new RegionGenerator { Chr = chr };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

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
            var rg = new RegionGenerator { Chr = "chr1" };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Fact]
        public void ReadStrand()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands.ContainsKey(rg.Strand));
            }
        }

        [Fact]
        public void ReadLeft()
        {
            // Arrange
            var rg = new RegionGenerator { Left = 10 };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Left == rg.Left);
            }
        }

        [Fact]
        public void FailReadLeft()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\tSource\tFeature\t10V\t20\t100.0\t*\t0\tatt1=1;att2=v2"))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadRight()
        {
            // Arrange
            var rg = new RegionGenerator { Right = 20 };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Right == rg.Right);
            }
        }

        [Fact]
        public void FailReadRight()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\tSource\tFeature\t10\t20V\t100.0\t*\t0\tatt1=1;att2=v2"))
            {
                // ACt
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadSource()
        {
            // Arrange
            var rg = new RegionGenerator { Source = "Source_01" };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Source == rg.Source);
            }
        }

        [Fact]
        public void ReadFeature()
        {
            // Arrange
            var rg = new RegionGenerator { Feature = "Feature_01" };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Feature == rg.Feature);
            }
        }

        [Fact]
        public void ReadScore()
        {
            // Arrange
            var rg = new RegionGenerator { Score = 123.456 };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Score == rg.Score);
            }
        }

        [Fact]
        public void ReadAttribute()
        {
            // Arrange
            var rg = new RegionGenerator { Attribute = "att1=at1;att2=at2;att3=3" };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Attribute == rg.Attribute);
            }
        }

        [Fact]
        public void AssignHashKey()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].GetHashCode() != 0);
            }
        }
    }
}
