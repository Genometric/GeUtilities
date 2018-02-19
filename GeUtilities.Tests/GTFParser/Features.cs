// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers;
using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using Xunit;

namespace GeUtilities.Tests.TGTFParser
{
    public class Features
    {
        internal static GTF<GeneralFeature> ParseGTF(string filePath, RegionGenerator rg)
        {
            var parser = new GTFParser<GeneralFeature>(filePath, rg.Columns);
            return parser.Parse();
        }

        [Fact]
        public void FeatureCount()
        {
            // Arrange
            var rg = new RegionGenerator { Feature = "feature" };
            int featureCount = 5;
            using (var testFile = new TempFileCreator(rg, featuresCount: featureCount))
            {
                // Act
                var parsedGTF = ParseGTF(testFile.TempFilePath, rg);

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
            using (var testFile = new TempFileCreator(rg, featuresCount: 10, headerLineCount: 2))
            {
                // Act
                var parsedData = ParseGTF(testFile.TempFilePath, rg);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.Count == 10);
            }
        }
    }
}
