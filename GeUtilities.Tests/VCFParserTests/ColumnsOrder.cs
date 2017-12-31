// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.VCFParser
{
    public class ColumnsOrder
    {
        private ParsedVariants<Variant> ParseVCF(string filePath, Columns vcfColumns)
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
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                VCFParser<Variant> parser = new VCFParser<Variant>(testFile.TempFilePath);
                var parsedVariant = parser.Parse().Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                Assert.True(
                    parsedVariant.Left == columns.Position &&
                    parsedVariant.ID == columns.ID &&
                    string.Equals(string.Join("", parsedVariant.RefBase), string.Join("", columns.RefBase)) &&
                    string.Equals(string.Join("", parsedVariant.AltBase), string.Join("", columns.AltBase)) &&
                    parsedVariant.Quality == columns.Quality &&
                    parsedVariant.Filter == columns.Filter &&
                    parsedVariant.Info == columns.Info);
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
            Columns columns = new Columns()
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
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);
                var parsedVariant = parsedVCF.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0];

                Assert.True(
                    parsedVariant.Left == columns.Position &&
                    parsedVariant.ID == columns.ID &&
                    string.Equals(string.Join("", parsedVariant.RefBase), string.Join("", columns.RefBase)) &&
                    string.Equals(string.Join("", parsedVariant.AltBase), string.Join("", columns.AltBase)) &&
                    parsedVariant.Quality == columns.Quality &&
                    parsedVariant.Filter == columns.Filter &&
                    parsedVariant.Info == columns.Info);
            }
        }

        [Fact]
        public void FailToReadPosition()
        {
            Columns columns = new Columns()
            {
                PositionColumn = 20
            };

            string line = "chr1\tXYZ\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadID()
        {
            Columns columns = new Columns()
            {
                IDColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadRefbpInvalidColumn()
        {
            Columns columns = new Columns()
            {
                RefbColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadRefbpInvalidValue()
        {
            Columns columns = new Columns()
            {
                RefbColumn = 2
            };

            string line = "chr1\t10\tXYZ\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadAltbpInvalidColumn()
        {
            Columns columns = new Columns()
            {
                AltbColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadAltbpInvalidValue()
        {
            Columns columns = new Columns()
            {
                AltbColumn = 3
            };

            string line = "chr1\t10\tACC\tXYZ\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadQualityInvalidColumn()
        {
            Columns columns = new Columns()
            {
                QualityColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadQualityInvalidValue()
        {
            Columns columns = new Columns()
            {
                QualityColumn = 4
            };

            string line = "chr1\t10\tACC\tCCA\t12.3.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadFilter()
        {
            Columns columns = new Columns()
            {
                FilterColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadInfo()
        {
            Columns columns = new Columns()
            {
                InfoColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempFileCreator testFile = new TempFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, columns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }
    }
}
