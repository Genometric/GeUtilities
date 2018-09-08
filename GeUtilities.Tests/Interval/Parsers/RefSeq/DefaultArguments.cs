// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Parsers.Model;
using Xunit;

namespace GeUtilities.Tests.Interval.Parsers.RefSeq
{
    public class DefaultArguments
    {
        [Fact]
        public void TestDefaultRefSeqGenesColumnOrder()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new RefSeqParser();
                var parsedGene = parser.Parse(file.TempFilePath).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(rg.Gene) == 0);
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
            var rg = new RegionGenerator { Chr = chr };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new RefSeqParser();
                var parsedData = parser.Parse(file.TempFilePath);

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
            var rg = new RegionGenerator { Chr = "chr1" };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new RefSeqParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey(chr));
            }
        }

        [Theory]
        [InlineData('*')]
        [InlineData('+')]
        public void ReadStrand(char strand)
        {
            // Arrange
            var rg = new RegionGenerator
            {
                Strand = strand,
                StrandColumn = 5
            };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var genesParser = new RefSeqParser(rg.Columns);
                var parsedData = genesParser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands.ContainsKey(strand));
            }
        }

        [Fact]
        public void ReadRefSeqID()
        {
            // Arrange
            var rg = new RegionGenerator { RefSeqID = "RefSeqID_001" };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new RefSeqParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].RefSeqID == rg.RefSeqID);
            }
        }

        [Fact]
        public void FailReadRefSeqID()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\t10\t20\tRefSeq\tGeneSymbol"))
            {
                // Act
                var parser = new RefSeqParser(new RefSeqColumns() { RefSeqID = 10 });
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadGeneSymbol()
        {
            // Arrange
            var rg = new RegionGenerator { GeneSymbol = "Symbol_001" };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new RefSeqParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].GeneSymbol == rg.GeneSymbol);
            }
        }

        [Fact]
        public void FailReadGeneSymbol()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\t10\t20\tRefSeq\tGeneSymbol"))
            {
                // Act
                var parser = new RefSeqParser(new RefSeqColumns() { GeneSeymbol = 10 });
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
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
                var parser = new RefSeqParser()
                {
                    ReadOffset = readOffset
                };
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 1);
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
                var parser = new RefSeqParser();
                var parsedData = parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].GetHashCode() != 0);
            }
        }
    }
}
