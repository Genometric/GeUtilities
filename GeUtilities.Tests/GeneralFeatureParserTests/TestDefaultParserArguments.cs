// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.GeneralFeatureParserTests
{
    public class TestDefaultParserArguments
    {
        private string _chr = "chr1";

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(2, 2)]
        public void AvoidHeader(int headerCount, byte startOffset)
        {
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(headerLineCount: headerCount))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath, startOffset: startOffset);
                var parsedData = gtfParser.Parse();

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
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: chr))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.True(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Theory]
        [InlineData("chr2")]
        [InlineData("chrX")]
        public void FailReadChr(string chr)
        {
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: "chr1"))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Fact]
        public void ReadStrand()
        {
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands.ContainsKey('*'));
            }
        }

        [Fact]
        public void ReadLeft()
        {
            int left = 10;
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr, left: left.ToString()))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Left == left);
            }
        }

        [Fact]
        public void FailReadLeft()
        {
            string left = "10V";
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr, left: left))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(_chr));
            }
        }

        [Fact]
        public void ReadRight()
        {
            int right = 20;
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr, right: right.ToString()))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Right == right);
            }
        }

        [Fact]
        public void FailReadRight()
        {
            string right = "20V";
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr, right: right.ToString()))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(_chr));
            }
        }

        [Fact]
        public void ReadSource()
        {
            string source = "Di4_01";
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr, source: source))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Source == source);
            }
        }

        [Fact]
        public void ReadFeature()
        {
            string feature = "Di4_Gene";
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr, feature: feature))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Feature == feature);
            }
        }

        [Fact]
        public void ReadScore()
        {
            double score = 123.456;
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr, score: score))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Score == score);
            }
        }

        [Fact]
        public void ReadAttribute()
        {
            string attribute = "att1=at1;att2=at2;att3=3";
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(chr: _chr, attribute: attribute))
            {
                GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(testFile.TempFilePath);
                var parsedData = gtfParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Attribute == attribute);
            }
        }
    }
}
