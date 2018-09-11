// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Parsers;
using Xunit;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace Genometric.GeUtilities.Tests.Intervals.Parsers.BED
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
                var parser = new BedParser();
                var parsedPeak = parser.Parse(file.Path).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(rg.Peak) == 0);
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
                var parser = new BedParser(rg.Columns);
                var parsedPeak = parser.Parse(file.Path).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(rg.Peak) == 0);
            }
        }
    }
}
