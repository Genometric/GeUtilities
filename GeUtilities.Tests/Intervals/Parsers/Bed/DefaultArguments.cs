// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Parsers;
using Genometric.GeUtilities.Intervals.Parsers.Model;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.Bed
{
    public class DefaultArguments
    {
        [Fact]
        public void DefaultColumnsOrder()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator())
            {
                // Act
                var parser = new BedParser();
                var parsedPeak = parser.Parse(file.Path).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

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
            using (var file = new TempFileCreator(new RegionGenerator(), headerLineCount: headerCount))
            {
                // Act
                var parser = new BedParser()
                {
                    ReadOffset = readOffset
                };
                var parsedData = parser.Parse(file.Path);

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
            using (var file = new TempFileCreator(new RegionGenerator { Chr = chr }))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

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
            using (var file = new TempFileCreator(new RegionGenerator { Chr = "chr1" }))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

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
                // ACt
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands.ContainsKey(rg.Strand));
            }
        }

        [Fact]
        public void ReadLeft()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0].Left == rg.Left);
            }
        }

        [Fact]
        public void FailReadLeft()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\t10V\t20\tGeUtilities_01\t123.4"))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadRight()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0].Right == rg.Right);
            }
        }

        [Fact]
        public void FailReadRightInvalidValue()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\t10\t20V\tGeUtilities_01\t123.4"))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailReadRightColumnIndexOutOfRange()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123.4"))
            {
                // Act
                var parser = new BedParser(new BedColumns() { Right = 10 });
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadName()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0].Name == rg.Name);
            }
        }

        [Fact]
        public void ReadValue()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0].Value == rg.Value);
            }
        }

        [Theory]
        [InlineData(0.01, "fa-IR")]
        [InlineData(0.02, "fr-FR")]
        [InlineData(0.03, "en-US")]
        [InlineData(0.04, "es-ES")]
        [InlineData(0.05, "it-IT")]
        [InlineData(0.06, "ii-CN")]
        [InlineData(0.07, "ru-RU")]
        [InlineData(0.08, "ja-JP")]
        public void ReadValueInvariantCulture(double value, string culture)
        {
            // Arrange
            var rg = new RegionGenerator
            {
                Value = value,
                Culture = culture
            };

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser
                {
                    Culture = culture
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0].Value == rg.Value);
            }
        }

        [Fact]
        public void ThrowExceptionForInvalidCultureInfo()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var exception = Assert.Throws<ArgumentOutOfRangeException>(
                    () => new BedParser
                    {
                        Culture = "invalid_culture"
                    });

                // Assert
                Assert.False(string.IsNullOrEmpty(exception.Message));
                Assert.Contains("Invalid culture info", exception.Message);
            }            
        }

        [Fact]
        public void FailReadValue()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
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
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0].GetHashCode() != 0);
            }
        }

        [Fact]
        public void DefaultDelimiterIsTab()
        {
            // Arrange & Act
            var parser = new BedParser();

            // Assert
            Assert.True(parser.Delimiter == '\t');
        }

        [Fact]
        public void DefaultUnspecifiedStrandChar()
        {
            // Arrange & Act
            var parser = new BedParser();

            // Assert
            Assert.True(parser.UnspecifiedStrandChar == '.');
        }
    }
}
