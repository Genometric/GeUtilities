// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using System;
using Xunit;

namespace GeUtilities.Tests.GeneralFeatureParserTests
{
    public class TestColumnOrders
    {
        private string _chr = "chr1";

        private ParsedGeneralFeatures<GeneralFeature> ParseGTF(string filePath, GTFColumns gtfColumns)
        {
            GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(
                    filePath,
                    chrColumn: gtfColumns.ChrColumn,
                    sourceColumn: gtfColumns.SourceColumn,
                    featureColumn: gtfColumns.FeatureColumn,
                    leftEndColumn: gtfColumns.LeftColumn,
                    rightEndColumn: gtfColumns.RightColumn,
                    scoreColumn: gtfColumns.ScoreColumn,
                    strandColumn: gtfColumns.StrandColumn,
                    frameColumn: gtfColumns.FrameColumn,
                    attributeColumn: gtfColumns.AttributeColumn);
            return gtfParser.Parse();
        }

        [Fact]
        public void TestDefaultColumnOrder()
        {
            string chr = "chr1", source = "Di4", feature = "Gene", frame = "0", attribute = "att1=1;att2=v2";
            int left = 10, right = 20;
            double score = 100.0;
            char strand = '*';
            string line =
                chr + "\t" + source + "\t" + feature + "\t" + left + "\t" + right +
                "\t" + score + "\t" + strand + "\t" + frame + "\t" + attribute;

            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(line))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedFeature = gtfParser.Parse().Chromosomes[_chr].Strands['*'].Intervals[0];

                Assert.True(
                    parsedFeature.Source == source &&
                    parsedFeature.Feature == feature &&
                    parsedFeature.Left == left &&
                    parsedFeature.Right == right &&
                    parsedFeature.Score == score &&
                    parsedFeature.Frame == frame &&
                    parsedFeature.Attribute == attribute);
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
        public void TestColumnsShuffle(
            sbyte chrColumn, sbyte sourceColumn, sbyte featureColumn, sbyte leftColumn, sbyte rightColumn,
            sbyte scoreColumn, sbyte strandColumn, sbyte frameColumn, sbyte attributeColumn)
        {
            GTFColumns gtfColumns = new GTFColumns()
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

            string chr = "chr1", source = "Di4", feature = "Gene", frame = "0", attribute = "att1=1;att2=v2";
            int left = 10, right = 20;
            double score = 100.0;
            char strand = '*';

            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(gtfColumns: gtfColumns,
                chr: chr,
                 source: source, feature: feature, left: left.ToString(), right: right.ToString(), score: score, strand: strand, frame: frame, attribute: attribute))
            {
                var parsedGTF = ParseGTF(testFile.TempFilePath, gtfColumns);
                var parsedFeature = parsedGTF.Chromosomes[chr].Strands[strand].Intervals[0];

                Assert.True(
                    parsedFeature.Source == source &&
                    parsedFeature.Feature == feature &&
                    parsedFeature.Left == left &&
                    parsedFeature.Right == right &&
                    parsedFeature.Score == score &&
                    parsedFeature.Frame == frame &&
                    parsedFeature.Attribute == attribute);
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
            var gtfColumns = new GTFColumns
            {
                SourceColumn = sourceColumn
            };

            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(gtfColumns: gtfColumns, source: source))
            {
                var parsedGTF = ParseGTF(testFile.TempFilePath, gtfColumns);
                var parsedFeature = parsedGTF.Chromosomes[_chr].Strands['*'].Intervals[0];

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
            var gtfColumns = new GTFColumns
            {
                FeatureColumn = featureColumn
            };

            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(gtfColumns:gtfColumns, feature: feature))
            {
                var parsedGTF = ParseGTF(testFile.TempFilePath, gtfColumns);
                var parsedFeature = parsedGTF.Chromosomes[_chr].Strands['*'].Intervals[0];

                Assert.True(parsedFeature.Feature == feature);
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
            var gtfColumns = new GTFColumns
            {
                ScoreColumn = scoreColumn
            };

            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(gtfColumns: gtfColumns, score: score))
            {
                var parsedGTF = ParseGTF(testFile.TempFilePath, gtfColumns);
                var parsedFeature = parsedGTF.Chromosomes[_chr].Strands['*'].Intervals[0];

                Assert.True(double.IsNaN(score) ? double.IsNaN(parsedFeature.Score) : parsedFeature.Score == score);
            }
        }
    }
}
