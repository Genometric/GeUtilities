// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.GeneralFeatureParserTests
{
    public class GeneralTests
    {
        internal static ParsedGeneralFeatures<GeneralFeature> ParseGTF(string filePath, GTFColumns gtfColumns)
        {
            GeneralFeaturesParser<GeneralFeature> gtfParser = new GeneralFeaturesParser<GeneralFeature>(
                    filePath,
                    chrColumn: gtfColumns.ChrColumn,
                    sourceColumn: gtfColumns.SourceColumn,
                    featureColumn: gtfColumns.FeatureColumn,
                    leftEndColumn: gtfColumns.LeftColumn,
                    rightEndColumn: gtfColumns.RightColumn,
                    scoreColumn: gtfColumns.ScoreColumn,
                    strandColumn: gtfColumns.StrandColumn,
                    frameColumn: gtfColumns.FrameColumn,
                    attributeColumn: gtfColumns.AttributeColumn);
            return gtfParser.Parse();
        }

        [Fact]
        public void FeatureCount()
        {
            var gtfColumns = new GTFColumns();
            string feature = "feature";
            int featureCount = 5;
            using (TempGeneralFeatureFileCreator testFile = new TempGeneralFeatureFileCreator(gtfColumns: gtfColumns, feature: feature, featuresCount: featureCount))
            {
                var parsedGTF = ParseGTF(testFile.TempFilePath, gtfColumns);

                Assert.True(parsedGTF.DeterminedFeatures[feature] == featureCount);
            }
        }
    }
}
