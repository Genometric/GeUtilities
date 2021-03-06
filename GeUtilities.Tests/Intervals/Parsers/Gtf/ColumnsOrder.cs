﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Parsers;
using System.Linq;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.Gtf
{
    public class ColumnsOrder
    {
        [Fact]
        public void TestDefaultColumnOrder()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new GtfParser();
                var parsedFeature = parser.Parse(file.TempFilePath).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

                // Assert
                Assert.True(parsedFeature.CompareTo(rg.GFeature) == 0);
            }
        }

        [Theory]
        [InlineData(0, 1, 2, 3, 4, 5, 6, 7, 8)]
        [InlineData(1, 0, 2, 3, 4, 5, 6, 7, 8)]
        [InlineData(1, 2, 0, 3, 4, 5, 6, 7, 8)]
        [InlineData(1, 2, 3, 0, 4, 5, 6, 7, 8)]
        [InlineData(8, 7, 6, 5, 4, 3, 2, 1, 0)]
        [InlineData(8, 7, 6, 5, 4, 0, 1, 2, 3)]
        [InlineData(5, 6, 8, 7, 0, 2, 1, 4, 3)]
        [InlineData(10, 11, 12, 13, 14, 15, 16, 17, 18)]
        public void ColumnsShuffle(
            byte chrColumn, sbyte sourceColumn, sbyte featureColumn, byte leftColumn, sbyte rightColumn,
            sbyte scoreColumn, sbyte strandColumn, sbyte frameColumn, sbyte attributeColumn)
        {
            // Arrange
            var rg = new RegionGenerator()
            {
                ChrColumn = chrColumn,
                SourceColumn = sourceColumn,
                FeatureColumn = featureColumn,
                LeftColumn = leftColumn,
                RightColumn = rightColumn,
                ScoreColumn = scoreColumn,
                StrandColumn = strandColumn,
                FrameColumn = frameColumn,
                AttributeColumn = attributeColumn
            };

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedGTF = Features.ParseGTF(file.TempFilePath, rg);
                var parsedFeature = parsedGTF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

                // Assert
                Assert.True(parsedFeature.CompareTo(rg.GFeature) == 0);
            }
        }

        [Fact]
        public void ColumnsSetters()
        {
            // Arrange
            var rg = new RegionGenerator
            {
                ChrColumn = 2,
                LeftColumn = 2,
                RightColumn = 9,
                StrandColumn = -1,
                SourceColumn = 2,
                FeatureColumn = -1
            };
            rg.StrandColumn = 0;
            rg.StrandColumn = 19;
            rg.FeatureColumn = 8;

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedGTF = Features.ParseGTF(file.TempFilePath, rg);
                var parsedFeature = parsedGTF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

                // Assert
                Assert.True(parsedFeature.CompareTo(rg.GFeature) == 0);
            }
        }

        [Theory]
        [InlineData(0, "Source_01")]
        [InlineData(3, "Source_02")]
        [InlineData(3, "Source_03")]
        [InlineData(10, "Source_04")]
        [InlineData(10, "Source_05")]
        [InlineData(-1, null)]
        public void TestSourceColumn(sbyte sourceColumn, string source)
        {
            // Arrange
            var rg = new RegionGenerator
            {
                Source = source,
                SourceColumn = sourceColumn
            };

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedGTF = Features.ParseGTF(file.TempFilePath, rg);
                var parsedFeature = parsedGTF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

                // Assert
                Assert.True(parsedFeature.Source == source);
            }
        }

        [Theory]
        [InlineData(0, "Feature_01")]
        [InlineData(3, "Feature_02")]
        [InlineData(3, "Feature_03")]
        [InlineData(10, "Feature_04")]
        [InlineData(10, "Feature_05")]
        [InlineData(-1, null)]
        public void TestFeatureColumn(sbyte featureColumn, string feature)
        {
            // Arrange
            var rg = new RegionGenerator
            {
                Feature = feature,
                FeatureColumn = featureColumn
            };

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedGTF = Features.ParseGTF(file.TempFilePath, rg);
                var parsedFeature = parsedGTF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

                // Assert
                Assert.True(parsedFeature.Feature == feature);
            }
        }

        [Fact]
        public void TestInvalidFeatureColumn()
        {
            // Arrange
            var rg = new RegionGenerator
            {
                // a column number which is more than the number of columns written to the test file.
                FeatureColumn = 10
            };

            using (var file = new TempFileCreator("chr1\tDi4\t.\t10\t20\t100.0\t*\t0\tatt1=1;att2=v2"))
            {
                // Act
                var parsedGTF = Features.ParseGTF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedGTF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Theory]
        [InlineData(0, 1.0)]
        [InlineData(3, 1.1)]
        [InlineData(3, 1.11)]
        [InlineData(10, 1.111)]
        [InlineData(10, 1.1111)]
        [InlineData(-1, double.NaN)]
        public void TestScoreColumn(sbyte scoreColumn, double score)
        {
            // Arrange
            var rg = new RegionGenerator
            {
                Score = score,
                ScoreColumn = scoreColumn
            };

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedGTF = Features.ParseGTF(file.TempFilePath, rg);
                var parsedFeature = parsedGTF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

                // Assert
                Assert.True(double.IsNaN(score) ? double.IsNaN(parsedFeature.Score) : parsedFeature.Score == score);
            }
        }

        [Theory]
        [InlineData(0, "1")]
        [InlineData(3, "2")]
        [InlineData(3, "3")]
        [InlineData(10, "4")]
        [InlineData(10, "5")]
        [InlineData(-1, null)]
        public void TestFrameColumn(sbyte frameColumn, string frame)
        {
            // Arrange
            var rg = new RegionGenerator
            {
                Frame = frame,
                FrameColumn = frameColumn
            };

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedGTF = Features.ParseGTF(file.TempFilePath, rg);
                var parsedFeature = parsedGTF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

                // Assert
                Assert.True(parsedFeature.Frame == frame);
            }
        }

        [Theory]
        [InlineData(0, "a=1;b=B")]
        [InlineData(3, "a1=A1")]
        [InlineData(3, "aa1=1;bBb=2")]
        [InlineData(10, "AA1=AA;BbB=3")]
        [InlineData(10, "aAb=1;bBa=2")]
        [InlineData(-1, null)]
        public void TestAttributeColumn(sbyte attributeColumn, string attribute)
        {
            // Arrange
            var rg = new RegionGenerator
            {
                Attribute = attribute,
                AttributeColumn = attributeColumn
            };

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedGTF = Features.ParseGTF(file.TempFilePath, rg);
                var parsedFeature = parsedGTF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.ToList()[0];

                // Assert
                Assert.True(parsedFeature.Attribute == attribute);
            }
        }
    }
}
