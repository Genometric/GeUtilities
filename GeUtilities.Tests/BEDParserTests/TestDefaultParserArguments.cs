// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests
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
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(headerLineCount: headerCount))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath, startOffset: startOffset);
                var parsedData = bedParser.Parse();

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
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: chr))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes.ContainsKey(chr));
            }
        }


        [Theory]
        [InlineData("chr2")]
        [InlineData("chrX")]
        public void FailReadChr(string chr)
        {
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: "chr1"))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Fact]
        public void ReadStrand()
        {
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands.ContainsKey('*'));
            }
        }

        [Fact]
        public void ReadLeft()
        {
            int left = 10;
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, left: left.ToString()))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Left == left);
            }
        }

        [Fact]
        public void FailReadLeft()
        {
            string left = "10V";
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, left: left))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(_chr));
            }
        }

        [Fact]
        public void ReadRight()
        {
            int right = 20;
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, right: right.ToString()))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Right == right);
            }
        }

        [Fact]
        public void FailReadRight()
        {
            string right = "20V";
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, right: right.ToString()))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(_chr));
            }
        }


        [Fact]
        public void ReadName()
        {
            string name = "GeUtilities_01";
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, name: name))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Name == name);
            }
        }


        [Fact]
        public void ReadValue()
        {
            double value = 123.45;
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, value: value.ToString()))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].Value == value);
            }
        }


        [Fact]
        public void FailReadValue()
        {
            string value = "123..45";
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(chr: _chr, value: value.ToString()))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath);
                var parsedData = bedParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(_chr));
            }
        }
    }
}
