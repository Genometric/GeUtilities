// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.GeneralFeatureParser
{
    public class DefaultArguments
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(2, 2)]
        public void AvoidHeader(int headerCount, byte startOffset)
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator(new Columns(), headerLineCount: headerCount))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath, startOffset: startOffset);
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
            var columns = new Columns(chr: chr);
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
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
            var columns = new Columns(chr: "chr1");
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Fact]
        public void ReadStrand()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands.ContainsKey(columns.Strand));
            }
        }

        [Fact]
        public void ReadLeft()
        {
            // Arrange
            var columns = new Columns(left: 10);
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Left == columns.Left);
            }
        }

        [Fact]
        public void FailReadLeft()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\tSource\tFeature\t10V\t20\t100.0\t*\t0\tatt1=1;att2=v2"))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadRight()
        {
            // Arrange
            var columns = new Columns(right: 20);
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Right == columns.Right);
            }
        }

        [Fact]
        public void FailReadRight()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\tSource\tFeature\t10\t20V\t100.0\t*\t0\tatt1=1;att2=v2"))
            {
                // ACt
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadSource()
        {
            // Arrange
            var columns = new Columns(source: "Source_01");
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Source == columns.Source);
            }
        }

        [Fact]
        public void ReadFeature()
        {
            // Arrange
            var columns = new Columns(feature: "Feature_01");
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Feature == columns.Feature);
            }
        }

        [Fact]
        public void ReadScore()
        {
            // Arrange
            var columns = new Columns(score: 123.456);
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Score == columns.Score);
            }
        }

        [Fact]
        public void ReadAttribute()
        {
            // Arrange
            var columns = new Columns(attribute: "att1=at1;att2=at2;att3=3");
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Attribute == columns.Attribute);
            }
        }

        [Fact]
        public void AssignHashKey()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].HashKey != 0);
            }
        }
    }
}
