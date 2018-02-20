// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers;
using Xunit;

namespace GeUtilities.Tests.IntervalParsers.VCF
{
    public class ConcreteClassArguments
    {
        [Fact]
        public void AllDefaultArguments()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new VCFParser();
                var parsedVariant = parser.Parse(file.TempFilePath).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedVariant.CompareTo(rg.Variant) == 0);
            }
        }

        [Fact]
        public void FullySetArguments()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new VCFParser(rg.Columns);
                var parsedVariant = parser.Parse(file.TempFilePath).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedVariant.CompareTo(rg.Variant) == 0);
            }
        }
    }
}
