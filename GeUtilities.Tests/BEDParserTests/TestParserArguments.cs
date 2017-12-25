// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests
{
    public class TestParserArguments
    {
        private string _chr = "chr1";

        [Fact]
        public void DropPeaksHavingInvalidPValue()
        {
            string value = "123..45";
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, value: value))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath, dropPeakIfInvalidValue: true);
                var parsedData = bedParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(_chr));
            }
        }

        [Fact]
        public void DefaultPValue()
        {
            string value = "123..45";
            double defaultValue = 1122.33;
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, value: value))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath, dropPeakIfInvalidValue: false, defaultValue: defaultValue);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].intervals[0].Value == defaultValue);
            }
        }

        [Theory]
        [InlineData(0.001, 0.001, PValueFormat.SameAsInput)]
        [InlineData(0.001, 3, PValueFormat.minus1_Log10_pValue)]
        [InlineData(0.001, 30, PValueFormat.minus10_Log10_pValue)]
        [InlineData(0.001, 300, PValueFormat.minus100_Log10_pValue)]
        public void PValueConversion(double originalValue, double convertedValue, PValueFormat pvalueFormat)
        {
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, value: convertedValue.ToString()))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath, pValueFormat: pvalueFormat);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].intervals[0].Value == originalValue);
            }
        }
    }
}
