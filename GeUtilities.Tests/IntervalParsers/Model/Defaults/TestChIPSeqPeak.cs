﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using System;
using Xunit;

namespace GeUtilities.Tests.IntervalParsers.ModelTests.Defaults
{
    public class TestChIPSeqPeak
    {
        internal static ChIPSeqPeak GetTempChIPSeqPeak()
        {
            return new ChIPSeqPeak(10, 20, 100.0, 15, "GeUtilities");
        }

        [Theory]
        [InlineData(0, 10, 20, 100.0, 15, "GeUtilities", 10, 20, 100.0, 15, "GeUtilities")]
        [InlineData(-1, 8, 20, 100.0, 15, "GeUtilities", 10, 20, 100.0, 15, "GeUtilities")]
        [InlineData(-1, 10, 16, 100.0, 15, "GeUtilities", 10, 20, 100.0, 15, "GeUtilities")]
        [InlineData(-1, 10, 20, 90.0, 15, "GeUtilities", 10, 20, 100.0, 15, "GeUtilities")]
        [InlineData(-1, 10, 20, 100.0, 12, "GeUtilities", 10, 20, 100.0, 15, "GeUtilities")]
        [InlineData(-1, 10, 20, 100.0, 15, "GeU", 10, 20, 100.0, 15, "GeUtilities")]
        [InlineData(1, 10, 20, 100.0, 15, "GeUtilities", 8, 20, 100.0, 15, "GeUtilities")]
        [InlineData(1, 10, 20, 100.0, 15, "GeUtilities", 10, 18, 100.0, 15, "GeUtilities")]
        [InlineData(1, 10, 20, 100.0, 15, "GeUtilities", 10, 20, 90.0, 15, "GeUtilities")]
        [InlineData(1, 10, 20, 100.0, 15, "GeUtilities", 10, 20, 100.0, 12, "GeUtilities")]
        [InlineData(1, 10, 20, 100.0, 15, "GeUtilities", 10, 20, 100.0, 15, "GeU")]
        public void ComparisonTest(
            int comparisonResult,
            int aLeft, int aRight, double aValue, int aSummit, string aName,
            int bLeft, int bRight, double bValue, int bSummit, string bName)
        {
            // Arrange
            var aPeak = new ChIPSeqPeak(aLeft, aRight, aValue, aSummit, aName);
            var bPeak = new ChIPSeqPeak(bLeft, bRight, bValue, bSummit, bName);

            // Act & Assert
            Assert.True(aPeak.CompareTo(bPeak) == comparisonResult);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            // Arrange
            var peak = GetTempChIPSeqPeak();

            // Act & Assert
            Assert.True(peak.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            // Arrange
            var peak = GetTempChIPSeqPeak();

            // Act & Assert
            Assert.True(peak.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            // Arrange
            var aPeak = GetTempChIPSeqPeak();
            var bPeak = GetTempChIPSeqPeak();

            // Act & Assert
            Assert.True(aPeak.CompareTo((object)bPeak) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            // Arrange
            var aPeak = GetTempChIPSeqPeak();
            var aGene = TestGene.GetTempGene();

            // Act
            Exception exception = Assert.Throws<NotImplementedException>(() => aPeak.CompareTo(aGene));

            // Assert
            Assert.False(String.IsNullOrEmpty(exception.Message));
            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }

        [Theory]
        [InlineData("name", "name", 0)]
        [InlineData("name", null, 1)]
        [InlineData(null, "name", -1)]
        [InlineData(null, null, -1)]
        public void CompareTwoPeaksWithNullName(string aName, string bName, int expectedResult)
        {
            // Arrange
            var peakA = new ChIPSeqPeak(10, 20, 100, 15, aName);
            var peakB = new ChIPSeqPeak(10, 20, 100, 15, bName);

            // Act
            var comparison = peakA.CompareTo(peakB);

            // Assert
            Assert.Equal(expectedResult, comparison);
        }
    }
}
