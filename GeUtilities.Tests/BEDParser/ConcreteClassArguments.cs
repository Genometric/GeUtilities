// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Parsers;
using Genometric.GeUtilities.ReferenceGenomes;
using Xunit;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.TBEDParser
{
    public class ConcreteClassArguments
    {
        [Fact]
        public void AllDefaultArguments()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser parser = new BEDParser(testFile.TempFilePath);
                var parsedPeak = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(columns.Peak) == 0);
            }
        }

        [Fact]
        public void PartiallySetArguments()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser parser = new BEDParser(
                    testFile.TempFilePath,
                    dropPeakIfInvalidValue: true);
                var parsedPeak = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(columns.Peak) == 0);
            }
        }

        [Fact]
        public void FullySetArguments()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser parser = new BEDParser(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: columns.ValueColumn,
                    strandColumn: columns.StrandColumn,
                    summitColumn: columns.SummitColumn);
                var parsedPeak = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(columns.Peak) == 0);
            }
        }
    }
}
