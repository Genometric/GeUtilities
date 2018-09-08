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

        internal Variant GetVariant(Parameter param = Parameter.None, object value = null)
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

        private Base[] ConvertStringToBasePair(string input)
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

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            // Arrange
            var variant = GetVariant();

            // Act & Assert
            Assert.True(variant.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            // Arrange
            var variant = GetVariant();

            // Act & Assert
            Assert.True(variant.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            // Arrange
            var aVariant = GetVariant();
            var bVariant = GetVariant();

            // Act & Assert
            Assert.True(aVariant.CompareTo((object)bVariant) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            // Arrange
            var aVariant = GetVariant();
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
        [InlineData(Parameter.None, null, null, 0)]
        [InlineData(Parameter.Left, 8, 10, -1)]
        [InlineData(Parameter.Left, 10, 8, 1)]
        [InlineData(Parameter.ID, "GU", null, 1)]
        [InlineData(Parameter.ID, null, "GU", -1)]
        [InlineData(Parameter.ID, null, null, -1)]
        [InlineData(Parameter.ID, "GU", "G", 1)]
        [InlineData(Parameter.ID, "G", "GU", -1)]
        [InlineData(Parameter.Info, "GU", null, 1)]
        [InlineData(Parameter.Info, null, "GU", -1)]
        [InlineData(Parameter.Info, null, null, -1)]
        [InlineData(Parameter.Info, "GU", "G", 1)]
        [InlineData(Parameter.Info, "G", "GU", -1)]
        [InlineData(Parameter.Filter, "GU", null, 1)]
        [InlineData(Parameter.Filter, null, "GU", -1)]
        [InlineData(Parameter.Filter, null, null, -1)]
        [InlineData(Parameter.Filter, "GU", "G", 1)]
        [InlineData(Parameter.Filter, "G", "GU", -1)]
        [InlineData(Parameter.RefBase, "GU", null, 1)]
        [InlineData(Parameter.RefBase, null, "GU", -1)]
        [InlineData(Parameter.RefBase, null, null, -1)]
        [InlineData(Parameter.RefBase, "GU", "G", 1)]
        [InlineData(Parameter.RefBase, "G", "GU", -1)]
        [InlineData(Parameter.AltBase, "GU", null, 1)]
        [InlineData(Parameter.AltBase, null, "GU", -1)]
        [InlineData(Parameter.AltBase, null, null, -1)]
        [InlineData(Parameter.AltBase, "GU", "G", 1)]
        [InlineData(Parameter.AltBase, "G", "GU", -1)]
        [InlineData(Parameter.Quality, 123.4, 23.4, 1)]
        [InlineData(Parameter.Quality, 23.4, 123.4, -1)]
        public void CompareTo(Parameter param, object v1, object v2, int expected)
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
