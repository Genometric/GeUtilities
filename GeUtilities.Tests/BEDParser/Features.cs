﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Genometric.GeUtilities.ReferenceGenomes;
using System;
using System.IO;
using Xunit;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.TBEDParser
{
    public class Features
    {
        [Fact]
        public void InvalidFile()
        {
            // Arrange
            string fileName = "a_file_name_which_does_not_exist_1234567890";
            BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(fileName);

            // Act
            Exception exception = Assert.Throws<FileNotFoundException>(() => parser.Parse());

            // Assert
            Assert.False(String.IsNullOrEmpty(exception.Message));
            Assert.Equal(string.Format("The file `{0}` does not exist or is inaccessible.", fileName), exception.Message);
        }

        [Fact]
        public void DropPeaksHavingInvalidPValue()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, dropPeakIfInvalidValue: true);
                var parsedData = parser.Parse();

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
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, dropPeakIfInvalidValue: false, defaultValue: defaultValue);
                var parsedData = parser.Parse();

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
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: 9,
                    dropPeakIfInvalidValue: true);
                var parsedData = parser.Parse();

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
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: 9,
                    dropPeakIfInvalidValue: false);
                var parsedData = parser.Parse();

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
            var columns = new Columns { Value = convertedValue };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, pValueFormat: pvalueFormat);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].Value == originalValue);
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 1)]
        [InlineData(1, 4)]
        [InlineData(4, 4)]
        public void MaxLinesToRead(int numberOfPeaksToWrite, uint numberOfPeaksToRead)
        {
            // Arrange
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns, peaksCount: numberOfPeaksToWrite))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, maxLinesToRead: numberOfPeaksToRead);
                var parsedData = parser.Parse();

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
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, maxLinesToRead: 0);
                var parsedData = parser.Parse();

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
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: 12,
                    valueColumn: columns.ValueColumn,
                    dropPeakIfInvalidValue: false);
                var parsedData = parser.Parse();

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
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, hashFunction: hashFunction);
                var parsedData = parser.Parse();

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
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(
                    testFile.TempFilePath,
                    chrColumn: columns.ChrColumn,
                    leftEndColumn: columns.LeftColumn,
                    rightEndColumn: columns.RightColumn,
                    nameColumn: columns.NameColumn,
                    valueColumn: 9,
                    dropPeakIfInvalidValue: true);
                var parsedData = parser.Parse();

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
            var columns = new Columns { Chr = chr };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, assembly: Assemblies.hg19, readOnlyValidChrs: true);
                var parsedData = parser.Parse();

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
            var columns = new Columns { Chr = chr };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, assembly: Assemblies.hg19, readOnlyValidChrs: true);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }

        [Theory]
        [InlineData("chrZ")]
        [InlineData("chr99")]
        [InlineData("99")]
        public void TestExcessChrs(string chr)
        {
            // Arrange
            var columns = new Columns { Chr = chr };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, assembly: Assemblies.hg19, readOnlyValidChrs: false);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parser.ExcessChrs.Count == 1);
            }
        }

        [Fact]
        public void TestMissingChrs()
        {
            // Arrange
            var columns = new Columns { Chr = "chr1" };
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, assembly: Assemblies.hg19, readOnlyValidChrs: false);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parser.MissingChrs.Count == References.GetGenomeSizes(Assemblies.hg19).Count - 1);
            }
        }

        [Fact]
        public void BEDWithAllOptionalColumnsAndSomeExtraColumns()
        {
            // Arrange
            var columns = new Columns { SummitColumn = 10, StrandColumn = 11 };
            using (TempFileCreator testFile = new TempFileCreator(columns, headerLineCount: 2, peaksCount: 10))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals.Count == 10);
            }
        }

        [Fact]
        public void AvoidEmptyLines()
        {
            // Arrange
            using (TempFileCreator testFile = new TempFileCreator("             "))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, dropPeakIfInvalidValue: true);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }
    }
}
