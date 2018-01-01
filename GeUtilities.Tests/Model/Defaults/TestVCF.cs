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
        internal static Variant GetTempVCF()
        {
            return new Variant()
            {
                Left = 10,
                ID = "ID",
                RefBase = ConvertStringToBasePair("ACGN"),
                AltBase = ConvertStringToBasePair("UGCA"),
                Quality = 123.4,
                Filter = "Filter",
                Info = "Info"
            };
        }

        private static Base[] ConvertStringToBasePair(string input)
        {
            var rtv = new Base[input.Length];
            for (int i = 0; i < input.Length; i++)
                switch (input[i])
                {
                    case 'A': rtv[i] = Base.A; break;
                    case 'C': rtv[i] = Base.C; break;
                    case 'G': rtv[i] = Base.G; break;
                    case 'N': rtv[i] = Base.N; break;
                    case 'T': rtv[i] = Base.T; break;
                    case 'U': rtv[i] = Base.U; break;
                    default: continue;
                }
            return rtv;
        }

        [Theory]
        [InlineData(0, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 8, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, "I", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, "ID", "A", "TGCA", 123.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, "ID", "ACGT", "A", 123.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, "ID", "ACGT", "TGCA", 23.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, "ID", "ACGT", "TGCA", 123.4, "F", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(-1, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "I", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 8, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "I", "ACGT", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "ID", "A", "TGCA", 123.4, "Filter", "Info")]
        [InlineData(1, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "ID", "ACGT", "A", 123.4, "Filter", "Info")]
        [InlineData(1, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 23.4, "Filter", "Info")]
        [InlineData(1, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "F", "Info")]
        [InlineData(1, 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "Info", 10, "ID", "ACGT", "TGCA", 123.4, "Filter", "I")]
        public void ComparisonTest(
            int comparisonResult,
            int aLeft, string aID, string aRefbp, string aAltbp, double aQuality, string aFilter, string aInfo,
            int bLeft, string bID, string bRefbp, string bAltbp, double bQuality, string bFilter, string bInfo)
        {
            var aVariant = new Variant()
            {
                Left = aLeft,
                ID = aID,
                RefBase = ConvertStringToBasePair(aRefbp),
                AltBase = ConvertStringToBasePair(aAltbp),
                Quality = aQuality,
                Filter = aFilter,
                Info = aInfo
            };

            var bVariant = new Variant()
            {
                Left = bLeft,
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
            var variant = GetTempVCF();

            Assert.True(variant.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            var variant = GetTempVCF();

            Assert.True(variant.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            var aVariant = GetTempVCF();
            var bVariant = GetTempVCF();

            Assert.True(aVariant.CompareTo((object)bVariant) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            var aVariant = GetTempVCF();
            var aPeak = TestChIPSeqPeak.GetTempChIPSeqPeak();

            Exception exception = Assert.Throws<NotImplementedException>(() => aVariant.CompareTo(aPeak));

            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }
    }
}
