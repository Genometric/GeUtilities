// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using System;
using Xunit;

namespace GeUtilities.Tests.IntervalParsers.ModelTests.Defaults
{
    public class TestVCF
    {
        internal static Variant GetTempVCF()
        {
            return new Variant(10, 11, "ID", ConvertStringToBasePair("ACGN"), ConvertStringToBasePair("UGCA"), 123.4, "Filter", "Info");
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
                    default: rtv[i] = Base.U; break;
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
            // Arrange
            var aVariant = new Variant(
                left: aLeft,
                right: aLeft + 1,
                id: aID,
                refBase: ConvertStringToBasePair(aRefbp),
                altBase: ConvertStringToBasePair(aAltbp),
                quality: aQuality,
                filter: aFilter,
                info: aInfo);

            var bVariant = new Variant(
                left: bLeft,
                right: bLeft + 1,
                id: bID,
                refBase: ConvertStringToBasePair(bRefbp),
                altBase: ConvertStringToBasePair(bAltbp),
                quality: bQuality,
                filter: bFilter,
                info: bInfo);


            // Act & Assert
            Assert.True(aVariant.CompareTo(bVariant) == comparisonResult);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            // Arrange
            var variant = GetTempVCF();

            // Act & Assert
            Assert.True(variant.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            // Arrange
            var variant = GetTempVCF();

            // Act & Assert
            Assert.True(variant.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            // Arrange
            var aVariant = GetTempVCF();
            var bVariant = GetTempVCF();

            // Act & Assert
            Assert.True(aVariant.CompareTo((object)bVariant) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            // Arrange
            var aVariant = GetTempVCF();
            var aPeak = TestChIPSeqPeak.GetTempChIPSeqPeak();

            // Act & Assert
            Exception exception = Assert.Throws<NotImplementedException>(() => aVariant.CompareTo(aPeak));

            // Act & Assert
            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }
    }
}
