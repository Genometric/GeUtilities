// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Interval.Model;
using Genometric.GeUtilities.Interval.Parsers;
using Genometric.GeUtilities.Interval.Parsers.Model;
using Xunit;

namespace GeUtilities.Tests.Interval.Parsers.GTF
{
    public class Features
    {
        internal static GTF<GeneralFeature> ParseGTF(string filePath, RegionGenerator rg)
        {
            var parser = new GTFParser(rg.Columns);
            return parser.Parse(filePath);
        }

        [Fact]
        public void FeatureCount()
        {
            // Arrange
            var rg = new RegionGenerator { Feature = "feature" };
            int featureCount = 5;
            using (var file = new TempFileCreator(rg, featuresCount: featureCount))
            {
                // Act
                var parsedGTF = ParseGTF(file.TempFilePath, rg);

                // Assert
                Assert.True(parsedGTF.DeterminedFeatures[rg.Feature] == featureCount);
            }
        }

        [Fact]
        public void MultiFeatureFile()
        {
            // Arrange
            var rg = new RegionGenerator
            {
                StrandColumn = 12
            };
            using (var file = new TempFileCreator(rg, featuresCount: 10, headerLineCount: 2))
            {
                // Act
                var parsedData = ParseGTF(file.TempFilePath, rg);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.Count == 10);
            }
        }
    }
}
