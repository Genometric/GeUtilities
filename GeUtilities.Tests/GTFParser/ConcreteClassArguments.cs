// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Genometric.GeUtilities.ReferenceGenomes;
using Xunit;

namespace GeUtilities.Tests.TGTFParser
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
                GTFParser parser = new GTFParser(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.ContainsKey(columns.Chr));
            }
        }

        [Fact]
        public void PartiallySetArguments()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                GTFParser parser = new GTFParser(
                    testFile.TempFilePath,
                    assembly: Assemblies.hg19,
                    readOnlyValidChrs: true,
                    maxLinesToBeRead: 1,
                    startOffset: 0,
                    hashFunction: HashFunction.One_at_a_Time);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.ContainsKey(columns.Chr));
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
                GTFParser parser = new GTFParser(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    sourceColumn: columns.SourceColumn,
                    featureColumn: columns.FeatureColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    scoreColumn: columns.ScoreColumn,
                    strandColumn: columns.StrandColumn,
                    frameColumn: columns.FrameColumn,
                    attributeColumn: columns.AttributeColumn,
                    assembly: Assemblies.hg19,
                    readOnlyValidChrs: true,
                    maxLinesToBeRead: 1,
                    startOffset: 0,
                    hashFunction: HashFunction.One_at_a_Time);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.ContainsKey(columns.Chr));
            }
        }
    }
}
