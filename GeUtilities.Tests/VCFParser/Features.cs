// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.TVCFParser
{
    public class Features
    {
        [Fact]
        public void MultiVariantFile()
        {
            // Arrange
            var columns = new RegionGenerator
            {
                StrandColumn = 12
            };
            using (TempFileCreator testFile = new TempFileCreator(columns, variantsCount: 10, headerLineCount: 2))
            {
                // Act
                VCFParser<Variant> parser = new VCFParser<Variant>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals.Count == 10);
            }
        }

        /// <summary>
        /// All the parser of this package adhere to "interval" definition: 
        /// interval length is at least one base-pair. Intervals are left-end
        /// closed, and right-end open. Therefore, an interval of length 1, 
        /// is defined using a left-end inclusive, and right-end exclusive
        /// positions. Therefore, a variant position is defined inclusive
        /// left-end and exclusive right-end = left-end + 1. This test assesses
        /// if this condition is properly implemented in VCF parser.
        /// </summary>
        [Fact]
        public void OneBaseLenghtInterval()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                VCFParser<Variant> parser = new VCFParser<Variant>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Right == columns.Position + 1);
            }
        }
    }
}
