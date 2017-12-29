// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;
using Xunit;

namespace GeUtilities.Tests.ModelTests.Defaults
{
    public class TestVCF
    {
        private BasePair[] ConvertStringToBasePair(string input)
        {
            var rtv = new BasePair[input.Length];
            for (int i = 0; i < input.Length; i++)
                switch (input[i])
                {
                    case 'A': rtv[i] = BasePair.A; break;
                    case 'C': rtv[i] = BasePair.C; break;
                    case 'G': rtv[i] = BasePair.G; break;
                    case 'N': rtv[i] = BasePair.N; break;
                    case 'T': rtv[i] = BasePair.T; break;
                    case 'U': rtv[i] = BasePair.U; break;
                    default: continue;
                }
            return rtv;
        }

        [Theory]
        [InlineData(0, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 8, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, 15, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, 20, 90, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, 20, 100.0, "I", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, 20, 100.0, "ID", "A", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, 20, 100.0, "ID", "ACGT", "A", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 23.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "F", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "I", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 8, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 15, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 90.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "I", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "A", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "A", 123.4, "Filter", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 23.4, "Filter", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "F", "Info")]
        [InlineData(1, 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, 20, 100.0, "ID", "ACGT", "TGCA", 123.4, "Filter", "I")]
        public void ComparisonTest(
            int comparisonResult,
            int aLeft, int aRight, double aValue, string aID, string aRefbp, string aAltbp, double aQuality, string aFilter, string aInfo,
            int bLeft, int bRight, double bValue, string bID, string bRefbp, string bAltbp, double bQuality, string bFilter, string bInfo)
        {
            var aVariant = new VCF()
            {
                Left = aLeft,
                Right = aRight,
                Value = aValue,
                ID = aID,
                RefBase = ConvertStringToBasePair(aRefbp),
                AltBase = ConvertStringToBasePair(aAltbp),
                Quality = aQuality,
                Filter = aFilter,
                Info = aInfo
            };

            var bVariant = new VCF()
            {
                Left = bLeft,
                Right = bRight,
                Value = bValue,
                ID = bID,
                RefBase = ConvertStringToBasePair(bRefbp),
                AltBase = ConvertStringToBasePair(bAltbp),
                Quality = bQuality,
                Filter = bFilter,
                Info = bInfo
            };

            Assert.True(aVariant.CompareTo(bVariant) == comparisonResult);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            var variant = new VCF()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                ID = "ID",
                RefBase = ConvertStringToBasePair("ACG"),
                AltBase = ConvertStringToBasePair("GCA"),
                Quality = 123.4,
                Filter = "Filter",
                Info = "Info"
            };

            Assert.True(variant.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            var variant = new VCF()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                ID = "ID",
                RefBase = ConvertStringToBasePair("ACG"),
                AltBase = ConvertStringToBasePair("GCA"),
                Quality = 123.4,
                Filter = "Filter",
                Info = "Info"
            };

            Assert.True(variant.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            var aVariant = new VCF()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                ID = "ID",
                RefBase = ConvertStringToBasePair("ACG"),
                AltBase = ConvertStringToBasePair("GCA"),
                Quality = 123.4,
                Filter = "Filter",
                Info = "Info"
            };

            var bVariant = new VCF()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                ID = "ID",
                RefBase = ConvertStringToBasePair("ACG"),
                AltBase = ConvertStringToBasePair("GCA"),
                Quality = 123.4,
                Filter = "Filter",
                Info = "Info"
            };

            Assert.True(aVariant.CompareTo((object)bVariant) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            var aVariant = new VCF()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                ID = "ID",
                RefBase = ConvertStringToBasePair("ACG"),
                AltBase = ConvertStringToBasePair("GCA"),
                Quality = 123.4,
                Filter = "Filter",
                Info = "Info"
            };

            var aPeak = new ChIPSeqPeak()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                Summit = 15,
                Name = "GeUtilities"
            };

            Exception exception = Assert.Throws<NotImplementedException>(() => aVariant.CompareTo(aPeak));

            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }
    }
}
