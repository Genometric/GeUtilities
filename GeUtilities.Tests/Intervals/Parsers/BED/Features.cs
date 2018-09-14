// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using Genometric.GeUtilities.Intervals.Parsers;
using Genometric.GeUtilities.Intervals.Parsers.Model;
using Genometric.GeUtilities.ReferenceGenomes;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.Bed
{
    public class Features
    {
        [Fact]
        public void InvalidFile()
        {
            // Arrange
            string fileName = "a_file_name_which_does_not_exist_1234567890";
            var parser = new BedParser();

            // Act
            Exception exception = Assert.Throws<FileNotFoundException>(() => parser.Parse(fileName));

            // Assert
            Assert.False(string.IsNullOrEmpty(exception.Message));
            Assert.Equal(string.Format("The file `{0}` does not exist or is inaccessible.", fileName), exception.Message);
        }

        [Fact]
        public void DropPeaksHavingInvalidPValue()
        {
            // Arrange
            using (var file = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                var parser = new BedParser()
                {
                    DropPeakIfInvalidValue = true
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void UseDefaultPValueForPeaksWithInvalidPValue()
        {
            // Arrange
            double defaultValue = 0.112233;
            using (var file = new TempFileCreator("chr1\t10\t20\tGeUtilities_01\t123..45"))
            {
                // Act
                var parser = new BedParser()
                {
                    DropPeakIfInvalidValue = false,
                    DefaultValue = defaultValue
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes["chr1"].Strands['*'].Intervals[0].Value == defaultValue);
            }
        }

        [Fact]
        public void DropPeakIfInvalidPValueColumn()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser(new BedColumns() { Value = 9 })
                {
                    DropPeakIfInvalidValue = true
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.False(parsedData.Chromosomes.ContainsKey(rg.Chr));
            }
        }

        [Fact]
        public void UseDefaultValueIfInvalidPValueColumn()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser(new BedColumns() { Value = 9 })
                {
                    DropPeakIfInvalidValue = false
                };
                var parsedData = parser.Parse(file.Path);

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
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser()
                {
                    PValueFormat = pvalueFormat
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Value == originalValue);
            }
        }

        [Theory]
        [InlineData(-1, PValueFormats.SameAsInput, true, false)]
        [InlineData(-1, PValueFormats.SameAsInput, false, true)]
        [InlineData(10000, PValueFormats.SameAsInput, true, false)]
        [InlineData(10000, PValueFormats.SameAsInput, false, true)]
        [InlineData(-1, PValueFormats.minus1_Log10_pValue, true, false)]
        [InlineData(-1, PValueFormats.minus1_Log10_pValue, false, true)]
        public void PValueAssertion(double value, PValueFormats pvalueFormat, bool validate, bool expected)
        {
            // Arrange
            var rg = new RegionGenerator { Value = value };

            // Act
            using (var file = new TempFileCreator(rg))
            {
                var parser = new BedParser()
                {
                    PValueFormat = pvalueFormat,
                    ValidatePValue = validate
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes.Any() == expected);
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
            using (var file = new TempFileCreator(rg, peaksCount: numberOfPeaksToWrite))
            {
                // Act
                var parser = new BedParser()
                {
                    MaxLinesToRead = numberOfPeaksToRead
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.Count == Math.Min(numberOfPeaksToWrite, numberOfPeaksToRead));
            }
        }

        [Fact]
        public void ReadNoPeak()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg, peaksCount: 4))
            {
                // Act
                var parser = new BedParser()
                {
                    MaxLinesToRead = 0
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }

        [Fact]
        public void InvalidNameColumn()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser(new BedColumns() { Name = 12 })
                {
                    DropPeakIfInvalidValue = false
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0].Name == null);
            }
        }

        [Fact]
        public void LogErrorIfFailedToReadALine()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser(new BedColumns() { Value = 9 })
                {
                    DropPeakIfInvalidValue = true
                };
                var parsedData = parser.Parse(file.Path);

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
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser()
                {
                    Assembly = Assemblies.hg19,
                    ReadOnlyAssemblyChrs = true
                };
                var parsedData = parser.Parse(file.Path);

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
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser()
                {
                    Assembly = Assemblies.hg19,
                    ReadOnlyAssemblyChrs = true
                };
                var parsedData = parser.Parse(file.Path);

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
            using (var file = new TempFileCreator(rg))
            {
                // Act
                BedParser<Peak> parser = new BedParser()
                {
                    Assembly = Assemblies.hg19,
                    ReadOnlyAssemblyChrs = false
                };
                parser.Parse(file.Path);

                // Assert
                Assert.True(parser.ExcessChrs.Count == 1);
            }
        }

        [Fact]
        public void TestMissingChrs()
        {
            // Arrange
            var rg = new RegionGenerator { Chr = "chr1" };
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new BedParser()
                {
                    Assembly = Assemblies.hg19,
                    ReadOnlyAssemblyChrs = false
                };
                parser.Parse(file.Path);

                // Assert
                Assert.True(parser.MissingChrs.Count == References.GetGenomeSizes(Assemblies.hg19).Count - 1);
            }
        }

        [Fact]
        public void BEDWithAllOptionalColumnsAndSomeExtraColumns()
        {
            // Arrange
            var rg = new RegionGenerator { SummitColumn = 10, StrandColumn = 11 };
            using (var file = new TempFileCreator(rg, headerLineCount: 2, peaksCount: 10))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.Count == 10);
            }
        }

        [Fact]
        public void AvoidEmptyLines()
        {
            // Arrange
            using (var file = new TempFileCreator("             "))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes.Count == 0);
            }
        }

        [Fact]
        public void TestMaxValue()
        {
            // Arrange
            var p = new Peak(30, 40, 0.1, 35, "GeUtilities_01"); ;
            string[] peaks = new string[]
            {
                "chr1\t10\t20\tGeUtilities_00\t0.01",
                "chr2\t" + p.Left + "\t" + p.Right + "\t" + p.Name + "\t" + p.Value,
                "chr3\t50\t60\tGeUtilities_02\t0.001",
                "chr4\t70\t80\tGeUtilities_03\t0.0001",
            };
            using (var file = new TempFileCreator(peaks))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.PValueMax.CompareTo(p) == 0);
            }
        }

        [Fact]
        public void TestMinValue()
        {
            // Arrange
            var p = new Peak(30, 40, 0.0, 35, "GeUtilities_01");
            string[] peaks = new string[]
            {
                "chr1\t10\t20\tGeUtilities_00\t0.1",
                "chr2\t" + p.Left + "\t" + p.Right + "\t" + p.Name + "\t" + p.Value,
                "chr3\t50\t60\tGeUtilities_02\t0.01",
                "chr4\t70\t80\tGeUtilities_03\t0.001",
            };
            using (var file = new TempFileCreator(peaks))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

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
            using (var file = new TempFileCreator(peaks))
            {
                // Act
                var parser = new BedParser();
                var parsedData = parser.Parse(file.Path);

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
            using (var file = new TempFileCreator(rg.GetSampleLine(delimiter)))
            {
                // Act
                var parser = new BedParser()
                {
                    Delimiter = delimiter
                };
                var parsedData = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedData.Chromosomes[chr].Strands[strand].Intervals[0].CompareTo(rg.Peak) == 0);
            }
        }
    }
}
