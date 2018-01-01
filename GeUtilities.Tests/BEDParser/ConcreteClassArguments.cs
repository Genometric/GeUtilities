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
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser parser = new BEDParser(testFile.TempFilePath);
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
                BEDParser parser = new BEDParser(
                    testFile.TempFilePath,
                    assembly: Assemblies.hg19,
                    defaultValue: 1E-8,
                    pValueFormat: PValueFormat.SameAsInput,
                    dropPeakIfInvalidValue: true,
                    startOffset: 0,
                    readOnlyValidChrs: true,
                    maxLinesToBeRead: 1,
                    hashFunction: HashFunction.FNV);
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
                BEDParser parser = new BEDParser(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: columns.ValueColumn,
                    strandColumn: columns.StrandColumn,
                    summitColumn: columns.SummitColumn,
                    assembly: Assemblies.hg19,
                    defaultValue: 1E-8,
                    pValueFormat: PValueFormat.SameAsInput,
                    dropPeakIfInvalidValue: true,
                    startOffset: 0,
                    readOnlyValidChrs: true,
                    maxLinesToBeRead: 1,
                    hashFunction: HashFunction.FNV);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.ContainsKey(columns.Chr));
            }
        }
    }
}
