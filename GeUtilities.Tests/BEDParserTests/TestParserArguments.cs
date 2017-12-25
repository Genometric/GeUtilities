// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using System;
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

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Value == defaultValue);
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

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Value == originalValue);
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 1)]
        [InlineData(1, 4)]
        [InlineData(4, 4)]
        public void MaxLinesToBeRead(int numberOfPeaksToWrite, uint numberOfPeaksToRead)
        {
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, peaksCount: numberOfPeaksToWrite))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath, maxLinesToBeRead: numberOfPeaksToRead);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals.Count == Math.Min(numberOfPeaksToWrite, numberOfPeaksToRead));
            }
        }

        [Fact]
        public void ReadNoPeak()
        {
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, peaksCount: 4))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath, maxLinesToBeRead: 0);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }

        [Theory]
        [InlineData(HashFunction.FNV)]
        [InlineData(HashFunction.One_at_a_Time)]
        public void HashFunctions(HashFunction hashFunction)
        {
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath, hashFunction: hashFunction);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].HashKey != 0);
            }
        }
    }
}
