// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers;
using Genometric.GeUtilities.IntervalParsers.Model.Columns;
using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using Genometric.GeUtilities.ReferenceGenomes;
using System;
using System.IO;
using Xunit;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.IntervalParsers.BED
{
    public class Features
    {
        [Fact]
        public void InvalidFile()
        {
            // Arrange
            string fileName = "a_file_name_which_does_not_exist_1234567890";
            var parser = new BEDParser<ChIPSeqPeak>(fileName);

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
            using (var testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    DropPeakIfInvalidValue = true
                };
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
            using (var testFile = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    DropPeakIfInvalidValue = false,
                    DefaultValue = defaultValue
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes["chr1"].Strands['*'].Intervals[0].Value == defaultValue);
            }
        }

        [Fact]
        public void DropPeakIfInvalidPValueColumn()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, new BEDColumns() { Value = 9 })
                {
                    DropPeakIfInvalidValue = true
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey(rg.Chr));
            }
        }

        [Fact]
        public void UseDefaultValueIfInvalidPValueColumn()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, new BEDColumns() { Value = 9 })
                {
                    DropPeakIfInvalidValue = false
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.ContainsKey(rg.Chr));
            }
        }

        [Theory]
        [InlineData(0.001, 0.001, PValueFormats.SameAsInput)]
        [InlineData(0.001, 3, PValueFormats.minus1_Log10_pValue)]
        [InlineData(0.001, 30, PValueFormats.minus10_Log10_pValue)]
        [InlineData(0.001, 300, PValueFormats.minus100_Log10_pValue)]
        public void PValueConversion(double originalValue, double convertedValue, PValueFormats pvalueFormat)
        {
            // Arrange
            var rg = new RegionGenerator { Value = convertedValue };
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    PValueFormat = pvalueFormat
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Value == originalValue);
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
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg, peaksCount: numberOfPeaksToWrite))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    MaxLinesToRead = numberOfPeaksToRead
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.Count == Math.Min(numberOfPeaksToWrite, numberOfPeaksToRead));
            }
        }

        [Fact]
        public void ReadNoPeak()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg, peaksCount: 4))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    MaxLinesToRead = 0
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }

        [Fact]
        public void InvalidNameColumn()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, new BEDColumns() { Name = 12 })
                {
                    DropPeakIfInvalidValue = false
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Name == null);
            }
        }

        [Theory]
        [InlineData(Genometric.GeUtilities.IntervalParsers.HashFunctions.FNV)]
        [InlineData(Genometric.GeUtilities.IntervalParsers.HashFunctions.One_at_a_Time)]
        public void HashFunctions(HashFunctions hashFunction)
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    HashFunction = hashFunction
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].HashKey != 0);
            }
        }

        [Fact]
        public void LogErrorIfFailedToReadALine()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, new BEDColumns() { Value = 9 })
                {
                    DropPeakIfInvalidValue = true
                };
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
            var rg = new RegionGenerator { Chr = chr };
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    Assembly = Assemblies.hg19,
                    ReadOnlyAssemblyChrs = true
                };
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
            var rg = new RegionGenerator { Chr = chr };
            using (TempFileCreator testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    Assembly = Assemblies.hg19,
                    ReadOnlyAssemblyChrs = true
                };
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
            var rg = new RegionGenerator { Chr = chr };
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    Assembly = Assemblies.hg19,
                    ReadOnlyAssemblyChrs = false
                };
                parser.Parse();

                // Assert
                Assert.True(parser.ExcessChrs.Count == 1);
            }
        }

        [Fact]
        public void TestMissingChrs()
        {
            // Arrange
            var rg = new RegionGenerator { Chr = "chr1" };
            using (var testFile = new TempFileCreator(rg))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    Assembly = Assemblies.hg19,
                    ReadOnlyAssemblyChrs = false
                };
                parser.Parse();

                // Assert
                Assert.True(parser.MissingChrs.Count == References.GetGenomeSizes(Assemblies.hg19).Count - 1);
            }
        }

        [Fact]
        public void BEDWithAllOptionalColumnsAndSomeExtraColumns()
        {
            // Arrange
            var rg = new RegionGenerator { SummitColumn = 10, StrandColumn = 11 };
            using (var testFile = new TempFileCreator(rg, headerLineCount: 2, peaksCount: 10))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.Count == 10);
            }
        }

        [Fact]
        public void AvoidEmptyLines()
        {
            // Arrange
            using (var testFile = new TempFileCreator("             "))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }

        [Fact]
        public void TestMaxValue()
        {
            // Arrange
            var p = new ChIPSeqPeak() { Left = 30, Right = 40, Summit = 35, Name = "GeUtilities_01", Value = 0.1 };
            string[] peaks = new string[]
            {
                "chr1\t10\t20\tGeUtilities_00\t0.01",
                "chr2\t" + p.Left + "\t" + p.Right + "\t" + p.Name + "\t" + p.Value,
                "chr3\t50\t60\tGeUtilities_02\t0.001",
                "chr4\t70\t80\tGeUtilities_03\t0.0001",
            };
            using (var testFile = new TempFileCreator(peaks))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.PValueMax.CompareTo(p) == 0);
            }
        }

        [Fact]
        public void TestMinValue()
        {
            // Arrange
            var p = new ChIPSeqPeak() { Left = 30, Right = 40, Summit = 35, Name = "GeUtilities_01", Value = 0.0 };
            string[] peaks = new string[]
            {
                "chr1\t10\t20\tGeUtilities_00\t0.1",
                "chr2\t" + p.Left + "\t" + p.Right + "\t" + p.Name + "\t" + p.Value,
                "chr3\t50\t60\tGeUtilities_02\t0.01",
                "chr4\t70\t80\tGeUtilities_03\t0.001",
            };
            using (var testFile = new TempFileCreator(peaks))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.PValueMin.CompareTo(p) == 0);
            }
        }

        [Fact]
        public void TestMeanValue()
        {
            // Arrange
            string[] peaks = new string[]
            {
                "chr1\t10\t20\tGeUtilities_00\t0.1",
                "chr1\t30\t40\tGeUtilities_01\t0.01",
                "chr3\t50\t60\tGeUtilities_02\t0.001",
                "chr4\t70\t80\tGeUtilities_03\t0.0001",
            };
            using (var testFile = new TempFileCreator(peaks))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.PValueMean == 0.027775);
            }
        }

        [Theory]
        [InlineData(' ')]
        [InlineData(',')]
        [InlineData(';')]
        [InlineData('\t')]
        public void TestDelimiter(char delimiter)
        {
            // Arrange
            string chr = "chr1";
            char strand = '*';
            var rg = new RegionGenerator { Chr = chr, Strand = strand };
            using (var testFile = new TempFileCreator(rg.GetSampleLine(delimiter)))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath)
                {
                    Delimiter = delimiter
                };
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[chr].Strands[strand].Intervals[0].CompareTo(rg.Peak) == 0);
            }
        }
    }
}
