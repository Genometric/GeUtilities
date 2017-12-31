// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.GeneralFeatureParser
{
    public class Features
    {
        internal static ParsedGeneralFeatures<GeneralFeature> ParseGTF(string filePath, Columns columns)
        {
            GeneralFeaturesParser<GeneralFeature> parser = new GeneralFeaturesParser<GeneralFeature>(
                    filePath,
                    chrColumn: columns.ChrColumn,
                    sourceColumn: columns.SourceColumn,
                    featureColumn: columns.FeatureColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    scoreColumn: columns.ScoreColumn,
                    strandColumn: columns.StrandColumn,
                    frameColumn: columns.FrameColumn,
                    attributeColumn: columns.AttributeColumn);
            return parser.Parse();
        }

        [Fact]
        public void FeatureCount()
        {
            // Arrange
            var columns = new Columns(feature: "feature");
            int featureCount = 5;
            using (TempFileCreator testFile = new TempFileCreator(columns, featuresCount: featureCount))
            {
                // Act
                var parsedGTF = ParseGTF(testFile.TempFilePath, columns);

                // Assert
                Assert.True(parsedGTF.DeterminedFeatures[columns.GFeature.Feature] == featureCount);
            }
        }
    }
}
