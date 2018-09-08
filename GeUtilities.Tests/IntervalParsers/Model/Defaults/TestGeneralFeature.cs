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

        [Theory]
        [InlineData("source", "source", 0)]
        [InlineData("source", null, 1)]
        [InlineData(null, "source", -1)]
        [InlineData(null, null, -1)]
        public void CompareTwoFeaturesWithNullSource(string aSource, string bSource, int expectedResult)
        {
            // Arrange
            var featureA = new GeneralFeature(10, 20, aSource, "feature", 100, "frame", "attribute");
            var featureB = new GeneralFeature(10, 20, bSource, "feature", 100, "frame", "attribute");

            // Act
            var comparison = featureA.CompareTo(featureB);

            // Assert
            Assert.Equal(expectedResult, comparison);
        }

        [Theory]
        [InlineData("feature", "feature", 0)]
        [InlineData("feature", null, 1)]
        [InlineData(null, "feature", -1)]
        [InlineData(null, null, -1)]
        public void CompareTwoFeaturesWithNullFeature(string aFeature, string bFeature, int expectedResult)
        {
            // Arrange
            var featureA = new GeneralFeature(10, 20, "source", aFeature, 100, "frame", "attribute");
            var featureB = new GeneralFeature(10, 20, "source", bFeature, 100, "frame", "attribute");

            // Act
            var comparison = featureA.CompareTo(featureB);

            // Assert
            Assert.Equal(expectedResult, comparison);
        }

        [Theory]
        [InlineData("frame", "frame", 0)]
        [InlineData("frame", null, 1)]
        [InlineData(null, "frame", -1)]
        [InlineData(null, null, -1)]
        public void CompareTwoFeaturesWithNullFrame(string aFrame, string bFrame, int expectedResult)
        {
            // Arrange
            var featureA = new GeneralFeature(10, 20, "source", "feature", 100, aFrame, "attribute");
            var featureB = new GeneralFeature(10, 20, "source", "feature", 100, bFrame, "attribute");

            // Act
            var comparison = featureA.CompareTo(featureB);

            // Assert
            Assert.Equal(expectedResult, comparison);
        }

        [Theory]
        [InlineData("attribute", "attribute", 0)]
        [InlineData("attribute", null, 1)]
        [InlineData(null, "attribute", -1)]
        [InlineData(null, null, -1)]
        public void CompareTwoFeaturesWithNullAttribute(string aAttribute, string bAttribute, int expectedResult)
        {
            // Arrange
            var featureA = new GeneralFeature(10, 20, "source", "feature", 100, "frame", aAttribute);
            var featureB = new GeneralFeature(10, 20, "source", "feature", 100, "frame", bAttribute);

            // Act
            var comparison = featureA.CompareTo(featureB);

            // Assert
            Assert.Equal(expectedResult, comparison);
        }
    }
}
