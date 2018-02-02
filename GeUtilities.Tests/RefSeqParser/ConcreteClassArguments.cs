// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Parsers;
using Genometric.GeUtilities.ReferenceGenomes;
using Xunit;

namespace GeUtilities.Tests.TRefSeqParser
{
    public class ConcreteClassArguments
    {
        [Fact]
        public void AllDefaultArguments()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser parser = new RefSeqParser(testFile.TempFilePath);
                var parsedGene = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(columns.Gene) == 0);
            }
        }

        [Fact]
        public void PartiallySetArguments()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser parser = new RefSeqParser(
                    testFile.TempFilePath,
                    assembly: Assemblies.hg19);
                var parsedGene = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(columns.Gene) == 0);
            }
        }

        [Fact]
        public void FullySetArguments()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                RefSeqParser parser = new RefSeqParser(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    refSeqIDColumn: columns.RefSeqIDColumn,
                    geneSymbolColumn: columns.GeneSymbolColumn,
                    strandColumn: columns.StrandColumn,
                    assembly: Assemblies.hg19);
                var parsedGene = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(columns.Gene) == 0);
            }
        }
    }
}
