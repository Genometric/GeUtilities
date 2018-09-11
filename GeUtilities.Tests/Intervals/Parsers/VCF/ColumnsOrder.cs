// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using Genometric.GeUtilities.Intervals.Parsers;
using Genometric.GeUtilities.Intervals.Parsers.Model;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.VCF
{
    public class ColumnsOrder
    {
        private VCF<Variant> ParseVCF(string filePath, RegionGenerator rg)
        {
            var parser = new VcfParser(rg.Columns);
            return parser.Parse(filePath);
        }

        [Fact]
        public void TestDefaultVCFColumnOrder()
        {
            // Arrange
            var rg = new RegionGenerator();
            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parser = new VcfParser();
                var parsedVariant = parser.Parse(file.TempFilePath).Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedVariant.CompareTo(rg.Variant) == 0);
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
            var rg = new RegionGenerator()
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

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);
                var parsedVariant = parsedVCF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedVariant.CompareTo(rg.Variant) == 0);
            }
        }

        [Fact]
        public void FailToReadPosition()
        {
            // Arrange
            var rg = new RegionGenerator() { PositionColumn = 20 };
            string line = "chr1\tXYZ\tAAC\tCCA\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadID()
        {
            // Arrange
            var rg = new RegionGenerator() { IDColumn = 20 };
            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadRefbpInvalidColumn()
        {
            // Arrange
            var rg = new RegionGenerator() { RefbColumn = 20 };
            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadRefbpInvalidValue()
        {
            // Arrange
            var rg = new RegionGenerator() { RefbColumn = 2 };
            string line = "chr1\t10\tXYZ\tCCA\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadAltbpInvalidColumn()
        {
            // Arrange
            var rg = new RegionGenerator() { AltbColumn = 20 };
            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadAltbpInvalidValue()
        {
            // Arrange
            var rg = new RegionGenerator() { AltbColumn = 3 };
            string line = "chr1\t10\tACC\tXYZ\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadQualityInvalidColumn()
        {
            // Arrange
            var rg = new RegionGenerator() { QualityColumn = 20 };
            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadQualityInvalidValue()
        {
            // Arrange
            var rg = new RegionGenerator() { QualityColumn = 4 };
            string line = "chr1\t10\tACC\tCCA\t12.3.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadFilter()
        {
            // Arrange
            var rg = new RegionGenerator() { FilterColumn = 20 };
            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadInfo()
        {
            // Arrange
            var rg = new RegionGenerator() { InfoColumn = 20 };
            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";

            using (var file = new TempFileCreator(line))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);

                // Assert
                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void ColumnsSetters()
        {
            // Arrange
            var rg = new RegionGenerator
            {
                ChrColumn = 2,
                PositionColumn = 2,
                IDColumn = 9,
                RefbColumn = 0,
                AltbColumn = 2,
                QualityColumn = 2,
                FilterColumn = 0,
                InfoColumn = 6,
                StrandColumn = 12
            };
            rg.InfoColumn = 12;

            using (var file = new TempFileCreator(rg))
            {
                // Act
                var parsedVCF = ParseVCF(file.TempFilePath, rg);
                var parsedPeak = parsedVCF.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals[0];

                // Assert
                Assert.True(parsedPeak.CompareTo(rg.Variant) == 0);
            }
        }
    }
}
