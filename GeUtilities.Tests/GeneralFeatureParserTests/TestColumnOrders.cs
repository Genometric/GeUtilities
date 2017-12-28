// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.GeneralFeatureParserTests
{
    public class TestColumnOrders
    {
        private string _chr = "chr1";

        [Fact]
        public void TestDefaultColumnOrder()
        {
            int left = 10, right = 20;
            string source = "Di4", feature = "Gene", frame = "0";
            double score = 100.0;
            char strand = '*';
            string att1 = "att1", v1 = "v1", att2 = "att2", v2 = "v2";
            string attribute = att1 + "=" + v1 + ";" + att2 + "=" + v2;
            string line =
                _chr + "\t" + source + "\t" + feature + "\t" + left + "\t" + right +
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
    }
}
