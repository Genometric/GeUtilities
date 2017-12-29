﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using System.Linq;
using Xunit;

namespace GeUtilities.Tests.VCFParserTests
{
    public class TestColumnOrders
    {
        private string _chr = "chr1";

        private ParsedVariants<VCF> ParseVCF(string filePath, VCFColumns vcfColumns)
        {
            VCFParser<VCF> vcfParser = new VCFParser<VCF>(
                    filePath,
                    chrColumn: vcfColumns.ChrColumn,
                    positionColumn: vcfColumns.PositionColumn,
                    idColumn: vcfColumns.IDColumn,
                    refbpColumn: vcfColumns.RefbbpColumn,
                    altbpColumn: vcfColumns.AltbpColumn,
                    qualityColumn: vcfColumns.QualityColumn,
                    filterColumn: vcfColumns.FilterColumn,
                    infoColumn: vcfColumns.InfoColumn,
                    strandColumn: vcfColumns.StrandColumn);
            return vcfParser.Parse();
        }

        [Fact]
        public void TestDefaultVCFColumnOrder()
        {
            int position = 10;
            BasePair[] refBP = new BasePair[] { BasePair.A, BasePair.C, BasePair.G, BasePair.N, BasePair.T, BasePair.U };
            BasePair[] altBP = new BasePair[] { BasePair.U, BasePair.T, BasePair.N, BasePair.G, BasePair.C, BasePair.A };
            string id = "id_01", filter = "filter", info = "info";
            double quality = 123.456;
            char strand = '*';
            string line =
                _chr + "\t" + position + "\t" + id + "\t" + string.Join("", refBP) + "\t" +
                string.Join("", altBP) + "\t" + quality + "\t" + filter + "\t" + info + "\t" + strand.ToString();

            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                VCFParser<VCF> vcfParser = new VCFParser<VCF>(testFile.TempFilePath);
                var parsedVCF = vcfParser.Parse().Chromosomes[_chr].Strands[strand].Intervals[0];

                Assert.True(
                    parsedVCF.Left == position &&
                    parsedVCF.ID == id &&
                    parsedVCF.RefBase.SequenceEqual(refBP) &&
                    parsedVCF.AltBase.SequenceEqual(altBP) &&
                    parsedVCF.Quality == quality &&
                    parsedVCF.Filter == filter &&
                    parsedVCF.Info == info);
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
            VCFColumns vcfColumns = new VCFColumns()
            {
                ChrColumn = chrColumn,
                PositionColumn = positionColumn,
                IDColumn = idColumn,
                RefbbpColumn = refBPColumn,
                AltbpColumn = altBPColumn,
                QualityColumn = qualityColumn,
                FilterColumn = filterColumn,
                InfoColumn = infoColumn,
                StrandColumn = strandColumn
            };

            int position = 10;
            BasePair[] refBP = new BasePair[] { BasePair.A, BasePair.C, BasePair.G, BasePair.N, BasePair.T, BasePair.U };
            BasePair[] altBP = new BasePair[] { BasePair.U, BasePair.T, BasePair.N, BasePair.G, BasePair.C, BasePair.A };
            string id = "id_01", filter = "filter", info = "info";
            double quality = 123.456;
            char strand = '*';

            using (TempVCFFileCreator testFile = new TempVCFFileCreator(
                vcfColumns: vcfColumns, chr: _chr, position: position.ToString(), id: id, refBP: string.Join("", refBP),
                altBP: string.Join("", altBP), quality: quality.ToString(), filter: filter, info: info, strand: strand.ToString()))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);
                var parsedVariant = parsedVCF.Chromosomes[_chr].Strands[strand].Intervals[0];

                Assert.True(
                    parsedVariant.Left == position &&
                    parsedVariant.ID == id &&
                    parsedVariant.RefBase.SequenceEqual(refBP) &&
                    parsedVariant.AltBase.SequenceEqual(altBP) &&
                    parsedVariant.Quality == quality &&
                    parsedVariant.Filter == filter &&
                    parsedVariant.Info == info);
            }
        }

        [Fact]
        public void FailToReadPosition()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                PositionColumn = 20
            };

            string line = "chr1\tXYZ\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadID()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                IDColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadRefbpInvalidColumn()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                RefbbpColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadRefbpInvalidValue()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                RefbbpColumn = 2
            };

            string line = "chr1\t10\tXYZ\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadAltbpInvalidColumn()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                AltbpColumn = 20
            };

            string line = "chr1\t10\tAAC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadAltbpInvalidValue()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                AltbpColumn = 3
            };

            string line = "chr1\t10\tACC\tXYZ\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadQualityInvalidColumn()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                QualityColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadQualityInvalidValue()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                QualityColumn = 4
            };

            string line = "chr1\t10\tACC\tCCA\t12.3.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadFilter()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                FilterColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }

        [Fact]
        public void FailToReadInfo()
        {
            VCFColumns vcfColumns = new VCFColumns()
            {
                InfoColumn = 20
            };

            string line = "chr1\t10\tACC\tCCA\t123.456\tFilter\tInfo\t*";
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(line))
            {
                var parsedVCF = ParseVCF(testFile.TempFilePath, vcfColumns);

                Assert.False(parsedVCF.Chromosomes.ContainsKey("chr1"));
            }
        }
    }
}