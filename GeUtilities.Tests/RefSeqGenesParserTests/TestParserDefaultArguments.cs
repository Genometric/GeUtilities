// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using System;
using Xunit;

namespace GeUtilities.Tests.RefSeqGenesParserTests
{
    public class TestParserDefaultArguments
    {
        private string _chr = "chr1";

        [Fact]
        public void TestDefaultRefSeqGenesColumnOrder()
        {
            int left = 10, right = 20;
            string geneSymbol = "symbol_01", refSeqID = "id_01";
            string line = _chr + "\t" + left + "\t" + right + "\t" + refSeqID + "\t" + geneSymbol;
            using (TempRefSeqGenesFileCreator testFile = new TempRefSeqGenesFileCreator(line))
            {
                RefSeqGenesParser<Gene> genesParser = new RefSeqGenesParser<Gene>(testFile.TempFilePath);
                var parsedGene = genesParser.Parse().Chromosomes[_chr].Strands['*'].Intervals[0];

                Assert.True(parsedGene.Left == left && parsedGene.Right == right && parsedGene.GeneSymbol == geneSymbol && parsedGene.RefSeqID == refSeqID);
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
            using (TempRefSeqGenesFileCreator testFile = new TempRefSeqGenesFileCreator(chr: chr))
            {
                RefSeqGenesParser<Gene> genesParser = new RefSeqGenesParser<Gene>(testFile.TempFilePath);
                var parsedData = genesParser.Parse();

                Assert.True(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Theory]
        [InlineData("chr2")]
        [InlineData("chrX")]
        public void FailReadChr(string chr)
        {
            using (TempRefSeqGenesFileCreator testFile = new TempRefSeqGenesFileCreator(chr: "chr1"))
            {
                RefSeqGenesParser<Gene> genesParser = new RefSeqGenesParser<Gene>(testFile.TempFilePath);
                var parsedData = genesParser.Parse();

                Assert.False(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Theory]
        [InlineData('*')]
        [InlineData('+')]
        public void ReadStrand(char strand)
        {
            using (TempRefSeqGenesFileCreator testFile = new TempRefSeqGenesFileCreator(chr: _chr, strand: Convert.ToString(strand)))
            {
                RefSeqGenesParser<Gene> genesParser = new RefSeqGenesParser<Gene>(
                    testFile.TempFilePath,
                    chrColumn: 0,
                    leftEndColumn: 1,
                    rightEndColumn: 2,
                    refSeqIDColumn: 3,
                    officialGeneSymbolColumn: 4,
                    strandColumn: 5);
                var parsedData = genesParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands.ContainsKey(strand));
            }
        }

        [Fact]
        public void ReadRefSeqID()
        {
            string refSeqID = "ref_seq_id_001";
            using (TempRefSeqGenesFileCreator testFile = new TempRefSeqGenesFileCreator(chr: _chr, refSeqID: refSeqID))
            {
                RefSeqGenesParser<Gene> genesParser = new RefSeqGenesParser<Gene>(testFile.TempFilePath);
                var parsedData = genesParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].RefSeqID == refSeqID);
            }
        }

        [Fact]
        public void ReadGeneSymbol()
        {
            string geneSymbol = "symbol_001";
            using (TempRefSeqGenesFileCreator testFile = new TempRefSeqGenesFileCreator(chr: _chr, geneSymbol: geneSymbol))
            {
                RefSeqGenesParser<Gene> genesParser = new RefSeqGenesParser<Gene>(testFile.TempFilePath);
                var parsedData = genesParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].GeneSymbol == geneSymbol);
            }
        }

        [Fact]
        public void AssignHashKey()
        {
            using (TempRefSeqGenesFileCreator testFile = new TempRefSeqGenesFileCreator(chr: _chr))
            {
                RefSeqGenesParser<Gene> genesParser = new RefSeqGenesParser<Gene>(testFile.TempFilePath);
                var parsedData = genesParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].HashKey != 0);
            }
        }
    }
}
