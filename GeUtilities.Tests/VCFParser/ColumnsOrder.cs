// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.TVCFParser
{
    public class ColumnsOrder
    {
        private VCF<Variant> ParseVCF(string filePath, RegionGenerator vcfColumns)
        {
            VCFParser<Variant> vcfParser = new VCFParser<Variant>(
                    filePath,
                    chrColumn: vcfColumns.ChrColumn,
                    positionColumn: vcfColumns.PositionColumn,
                    idColumn: vcfColumns.IDColumn,
                    refbColumn: vcfColumns.RefbColumn,
                    altbColumn: vcfColumns.AltbColumn,
                    qualityColumn: vcfColumns.QualityColumn,
                    filterColumn: vcfColumns.FilterColumn,
                    infoColumn: vcfColumns.InfoColumn,
                    strandColumn: vcfColumns.StrandColumn);
            return vcfParser.Parse();
        }

        [Fact]
        public void TestDefaultVCFColumnOrder()
        {
            // Arrange
            var columns = new RegionGenerator();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                VCFParser<Variant> parser = new VCFParser<Variant>(testFile.TempFilePath);
                var parsedVariant = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedVariant.CompareTo(columns.Variant) == 0);
            }
        }

        [Theory]
        [InlineData(0, 1, 2, 3, 4, 5, 6, 7, 8)]
        [InlineData(1, 0, 2, 3, 4, 5, 6, 7, 8)]
        [InlineData(1, 2, 0, 3, 4, 5, 6, 7, 8)]
        [InlineData(1, 2, 3, 0, 4, 5, 6, 7, 8)]
        [InlineData(8, 7, 6, 5, 4, 3, 2, 1, 0)]
        [InlineData(8, 7, 6, 5, 4, 0, 1, 2, 3)]
        [InlineData(5, 6, 8, 7, 0, 2, 1, 4, 3)]
        [InlineData(10, 11, 12, 13, 14, 15, 16, 17, 18)]
        public void TestColumnsShuffle(
            byte chrColumn, byte positionColumn, byte idColumn, byte refBPColumn, byte altBPColumn,
            byte qualityColumn, byte filterColumn, byte infoColumn, sbyte strandColumn)
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                ChrColumn = chrColumn,
                PositionColumn = positionColumn,
                IDColumn = idColumn,
                RefbColumn = refBPColumn,
                AltbColumn = altBPColumn,
                QualityColumn = qualityColumn,
                FilterColumn = filterColumn,
                InfoColumn = infoColumn,
                StrandColumn = strandColumn
            };

            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);
                var parsedVariant = parsedVCF.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedVariant.CompareTo(columns.Variant) == 0);
            }
        }

        [Fact]
        public void FailToReadPosition()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                PositionColumn = 20
            };

            string line = "chr1\tXYZ\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadID()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                IDColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadRefbpInvalidColumn()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                RefbColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadRefbpInvalidValue()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                RefbColumn = 2
            };

            string line = "chr1\t10\tXYZ\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadAltbpInvalidColumn()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                AltbColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadAltbpInvalidValue()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                AltbColumn = 3
            };

            string line = "chr1\t10\tACC\tXYZ\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadQualityInvalidColumn()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                QualityColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadQualityInvalidValue()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                QualityColumn = 4
            };

            string line = "chr1\t10\tACC\tCCA\t12.3.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadFilter()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                FilterColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadInfo()
        {
            // Arrange
            RegionGenerator columns = new RegionGenerator()
            {
                InfoColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ColumnsSetters()
        {
            // Arrange
            var columns = new RegionGenerator();
            columns.ChrColumn = 2;
            columns.PositionColumn = 2;
            columns.IDColumn = 9;
            columns.RefbColumn = 0;
            columns.AltbColumn = 2;
            columns.QualityColumn = 2;
            columns.FilterColumn = 0;
            columns.InfoColumn = 6;
            columns.StrandColumn = 12;
            columns.InfoColumn = 12;
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                // Act
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);
                var parsedPeak = parsedVCF.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(columns.Variant) == 0);
            }
        }
    }
}
