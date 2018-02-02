// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Parsers;
using Genometric.GeUtilities.ReferenceGenomes;
using Xunit;

namespace GeUtilities.Tests.TVCFParser
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
                VCFParser parser = new VCFParser(testFile.TempFilePath);
                var parsedVariant = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedVariant.CompareTo(columns.Variant) == 0);
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
                VCFParser parser = new VCFParser(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    positionColumn: columns.PositionColumn,
                    idColumn: columns.IDColumn,
                    refbColumn: columns.RefbColumn,
                    altbColumn: columns.AltbColumn,
                    qualityColumn: columns.QualityColumn,
                    filterColumn: columns.FilterColumn,
                    infoColumn: columns.InfoColumn,
                    strandColumn: columns.StrandColumn);
                var parsedVariant = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedVariant.CompareTo(columns.Variant) == 0);
            }
        }
    }
}
