// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Parsers;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.GTF
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
                var parser = new GtfParser();
                var parsedFeature = parser.Parse(file.TempFilePath).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedFeature.CompareTo(rg.GFeature) == 0);
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
                var parser = new GtfParser(rg.Columns);
                var parsedFeature = parser.Parse(file.TempFilePath).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedFeature.CompareTo(rg.GFeature) == 0);
            }
        }
    }
}
