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
        public enum Parameter { None, Left, Right, ID, RefBase, AltBase, Quality, Filter, Info};

        internal static Variant GetVariant(Parameter param = Parameter.None, object value = null)
        {
            int left = 10;
            int right = 11;
            string id = "ID";
            Base[] refBase = ConvertStringToBasePair("ACGN");
            Base[] altBase = ConvertStringToBasePair("UGCA");
            double quality = 123.4;
            string filter = "Filter";
            string info = "Info";

            switch (param)
            {
                case Parameter.Left: left = (int)value; break;
                case Parameter.Right: right = (int)value; break;
                case Parameter.ID: id = (string)value; break;
                case Parameter.RefBase: refBase = (string)value == null ? null : ConvertStringToBasePair((string)value); break;
                case Parameter.AltBase: altBase = (string)value == null ? null : ConvertStringToBasePair((string)value); break;
                case Parameter.Quality: quality = (double)value; break;
                case Parameter.Filter: filter = (string)value; break;
                case Parameter.Info: info = (string)value; break;
                default: break;
            }

            return new Variant(left, right, id, refBase, altBase, quality, filter, info);
        }

        internal static Variant GetTempVCF(int left = 10, int right = 11, string id = "ID", string refBase = "ACGN", string altBase = "UGCA", double quality = 123.4, string filter = "Filter", string info = "Info")
        {
            var rBase = refBase == null ? null : ConvertStringToBasePair(refBase);
            var aBase = altBase == null ? null : ConvertStringToBasePair(altBase);
            return new Variant(left, right, id, rBase, aBase, quality, filter, info);
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

        [Fact]
        public void ConstructVariantWithNullAltBase()
        {
            // Arrange
            var constructor = new VariantConstructor();

            // Act
            var variant = constructor.Construct(10, 11, "ID", ConvertStringToBasePair("ACGN"), null, 123.4, "Filter", "Info");

            // Assert
            Assert.True(variant.GetHashCode() != 0);
        }

        [Fact]
        public void ConstructVariantWithNullRefBase()
        {
            // Arrange
            var constructor = new VariantConstructor();

            // Act
            var variant = constructor.Construct(10, 11, "ID", null, ConvertStringToBasePair("UGCA"), 123.4, "Filter", "Info");

            // Assert
            Assert.True(variant.GetHashCode() != 0);
        }


        [Theory]
        [InlineData(Parameter.ID, "GU", "GU", 0)]
        [InlineData(Parameter.ID, "GU", null, 1)]
        [InlineData(Parameter.ID, null, "GU", -1)]
        [InlineData(Parameter.ID, null, null, -1)]
        [InlineData(Parameter.Info, "GU", "GU", 0)]
        [InlineData(Parameter.Info, "GU", null, 1)]
        [InlineData(Parameter.Info, null, "GU", -1)]
        [InlineData(Parameter.Info, null, null, -1)]
        [InlineData(Parameter.Filter, "GU", "GU", 0)]
        [InlineData(Parameter.Filter, "GU", null, 1)]
        [InlineData(Parameter.Filter, null, "GU", -1)]
        [InlineData(Parameter.Filter, null, null, -1)]
        [InlineData(Parameter.RefBase, "GU", "GU", 0)]
        [InlineData(Parameter.RefBase, "GU", null, 1)]
        [InlineData(Parameter.RefBase, null, "GU", -1)]
        [InlineData(Parameter.RefBase, null, null, -1)]
        [InlineData(Parameter.AltBase, "GU", "GU", 0)]
        [InlineData(Parameter.AltBase, "GU", null, 1)]
        [InlineData(Parameter.AltBase, null, "GU", -1)]
        [InlineData(Parameter.AltBase, null, null, -1)]
        public void CompareTwoVariantsWithNullProperties(Parameter param, object v1, object v2, int expected)
        {
            // Arrange
            var a = GetVariant(param, v1);
            var b = GetVariant(param, v2);

            // Act
            var actual = a.CompareTo(b);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
