// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.TRefSeqParser
{
    public class DefaultArguments
    {
        [Fact]
        public void TestDefaultRefSeqGenesColumnOrder()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath);
                var parsedGene = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(columns.Gene) == 0);
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
            var columns = new Columns { Chr = chr };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath);
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
            var columns = new Columns { Chr = "chr1" };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath);
                var parsedData = parser.Parse();

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
            var columns = new Columns
            {
                Strand = strand,
                StrandColumn = 5
            };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser<Gene> genesParser = new RefSeqParser<Gene>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    refSeqIDColumn: columns.RefSeqIDColumn,
                    geneSymbolColumn: columns.GeneSymbolColumn,
                    strandColumn: columns.StrandColumn);
                var parsedData = genesParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands.ContainsKey(strand));
            }
        }

        [Fact]
        public void ReadRefSeqID()
        {
            // Arrange
            var columns = new Columns { RefSeqID = "RefSeqID_001" };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].RefSeqID == columns.RefSeqID);
            }
        }

        [Fact]
        public void FailReadRefSeqID()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10\t20\tRefSeq\tGeneSymbol"))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath, 0, 1, 2, 10, 4);
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ReadGeneSymbol()
        {
            // Arrange
            var columns = new Columns { GeneSymbol = "Symbol_001" };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].GeneSymbol == columns.GeneSymbol);
            }
        }

        [Fact]
        public void FailReadGeneSymbol()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10\t20\tRefSeq\tGeneSymbol"))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath, 0, 1, 2, 3, 10);
                var parsedData = parser.Parse();

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
            using (TempFileCreator testFile = new TempFileCreator(new Columns(), headerLineCount: headerCount))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath);
                parser.ReadOffset = readOffset;
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 1);
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
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].HashKey != 0);
            }
        }
    }
}
