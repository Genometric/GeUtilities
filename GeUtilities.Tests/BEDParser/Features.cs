// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Genometric.GeUtilities.ReferenceGenomes;
using System;
using Xunit;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.BEDParser
{
    public class Features
    {
        [Fact]
        public void DropPeaksHavingInvalidPValue()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, dropPeakIfInvalidValue: true);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void UseDefaultPValueForPeaksWithInvalidPValue()
        {
            // Arrange
            double defaultValue = 1122.33;
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, dropPeakIfInvalidValue: false, defaultValue: defaultValue);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes["chr1"].Strands['*'].Intervals[0].Value == defaultValue);
            }
        }

        [Fact]
        public void DropPeakIfInvalidPValueColumn()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: 9,
                    dropPeakIfInvalidValue: true);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey(columns.Chr));
            }
        }

        [Fact]
        public void UseDefaultValueIfInvalidPValueColumn()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: 9,
                    dropPeakIfInvalidValue: false);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.ContainsKey(columns.Chr));
            }
        }

        [Theory]
        [InlineData(0.001, 0.001, PValueFormat.SameAsInput)]
        [InlineData(0.001, 3, PValueFormat.minus1_Log10_pValue)]
        [InlineData(0.001, 30, PValueFormat.minus10_Log10_pValue)]
        [InlineData(0.001, 300, PValueFormat.minus100_Log10_pValue)]
        public void PValueConversion(double originalValue, double convertedValue, PValueFormat pvalueFormat)
        {
            // Arrange
            var columns = new Columns(value: convertedValue);
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, pValueFormat: pvalueFormat);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Value == originalValue);
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 1)]
        [InlineData(1, 4)]
        [InlineData(4, 4)]
        public void MaxLinesToBeRead(int numberOfPeaksToWrite, uint numberOfPeaksToRead)
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns, peaksCount: numberOfPeaksToWrite))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, maxLinesToBeRead: numberOfPeaksToRead);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals.Count == Math.Min(numberOfPeaksToWrite, numberOfPeaksToRead));
            }
        }

        [Fact]
        public void ReadNoPeak()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns, peaksCount: 4))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, maxLinesToBeRead: 0);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }

        [Fact]
        public void InvalidNameColumn()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: 12,
                    valueColumn: columns.ValueColumn,
                    dropPeakIfInvalidValue: false);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Name == null);
            }
        }

        [Theory]
        [InlineData(HashFunction.FNV)]
        [InlineData(HashFunction.One_at_a_Time)]
        public void HashFunctions(HashFunction hashFunction)
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, hashFunction: hashFunction);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].HashKey != 0);
            }
        }

        [Fact]
        public void LogErrorIfFailedToReadALine()
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: 9,
                    dropPeakIfInvalidValue: true);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Messages.Count > 0);
            }
        }

        [Theory]
        [InlineData("1")]
        [InlineData("X")]
        public void ParseIntervalChrWhenChrPrefixIsMissing(string chr)
        {
            // Arrange
            var columns = new Columns(chr: chr);
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, assembly: Assemblies.hg19, readOnlyValidChrs: true);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.ContainsKey("chr" + chr));
            }
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("A")]
        public void DropLineIfInvalidChr(string chr)
        {
            // Arrange
            var columns = new Columns(chr: chr);
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, assembly: Assemblies.hg19, readOnlyValidChrs: true);
                var parsedData = bedParser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }
    }
}
