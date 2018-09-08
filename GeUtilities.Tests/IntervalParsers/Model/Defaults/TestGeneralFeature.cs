// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using System;
using Xunit;

namespace GeUtilities.Tests.IntervalParsers.ModelTests.Defaults
{
    public class TestGeneralFeature
    {
        internal static GeneralFeature GetTempGeneralFeature()
        {
            return new GeneralFeature(10, 20, "Source", "Feature", 100.0, "Frame", "Attribute");
        }

        [Theory]
        [InlineData(0, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "S", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "F", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 8, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 10, 15, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 10, 20, 90.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 10, 20, 100.0, "F", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 10, 20, 100.0, "Fr", "A", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "S", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "F", 10, 20, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 8, 20, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 15, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 90.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "F", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "A")]
        public void ComparisonTest(
            int comparisonResult,
            string aSource, string aFeature, int aLeft, int aRight, double aScore, string aFrame, string aAttribute,
            string bSource, string bFeature, int bLeft, int bRight, double bScore, string bFrame, string bAttribute)
        {
            // Arrange
            var aGF = new GeneralFeature(aLeft, aRight, aSource, aFeature, aScore, aFrame, aAttribute);
            var bGF = new GeneralFeature(bLeft, bRight, bSource, bFeature, bScore, bFrame, bAttribute);

            // Act & Assert
            Assert.True(aGF.CompareTo(bGF) == comparisonResult);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            // Arrange
            var gf = GetTempGeneralFeature();

            // Act & Assert
            Assert.True(gf.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            // Arrange
            var gf = GetTempGeneralFeature();

            // Act & Assert
            Assert.True(gf.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            // Arrange
            var aGF = GetTempGeneralFeature();
            var bGF = GetTempGeneralFeature();

            // Act & Assert
            Assert.True(aGF.CompareTo((object)bGF) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            // Arrange
            var aGF = GetTempGeneralFeature();
            var aPeak = TestChIPSeqPeak.GetTempChIPSeqPeak();

            // Act & Assert
            Exception exception = Assert.Throws<NotImplementedException>((() => aGF.CompareTo(aPeak)));

            // Act & Assert
            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }
    }
}
