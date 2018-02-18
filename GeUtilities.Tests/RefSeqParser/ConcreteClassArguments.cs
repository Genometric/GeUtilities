// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.TRefSeqParser
{
    public class ConcreteClassArguments
    {
        [Fact]
        public void AllDefaultArguments()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new RefSeqParser(testFile.TempFilePath);
                var parsedGene = parser.Parse().Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(rg.Gene) == 0);
            }
        }

        [Fact]
        public void FullySetArguments()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new RefSeqParser(testFile.TempFilePath, rg.Columns);
                var parsedGene = parser.Parse().Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedGene.CompareTo(rg.Gene) == 0);
            }
        }
    }
}
