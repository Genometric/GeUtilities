// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;
using Xunit;

namespace GeUtilities.Tests.ModelTests.Defaults
{
    public class TestChIPSeqPeak
    {
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
            var aPeak = new ChIPSeqPeak()
            {
                Left = aLeft,
                Right = aRight,
                Value = aValue,
                Summit = aSummit,
                Name = aName
            };

            var bPeak = new ChIPSeqPeak()
            {
                Left = bLeft,
                Right = bRight,
                Value = bValue,
                Summit = bSummit,
                Name = bName
            };

            Assert.True(aPeak.CompareTo(bPeak) == comparisonResult);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            var peak = new ChIPSeqPeak()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                Summit = 15,
                Name = "GeUtilities"
            };

            Assert.True(peak.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            var peak = new ChIPSeqPeak()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                Summit = 15,
                Name = "GeUtilities"
            };

            Assert.True(peak.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            var aPeak = new ChIPSeqPeak()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                Summit = 15,
                Name = "GeUtilities"
            };

            var bPeak = new ChIPSeqPeak()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                Summit = 15,
                Name = "GeUtilities"
            };

            Assert.True(aPeak.CompareTo((object)bPeak) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            var aPeak = new ChIPSeqPeak()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                Summit = 15,
                Name = "GeUtilities"
            };

            var aGene = new Gene()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                RefSeqID = "RefSeqID",
                GeneSymbol = "GeneSymbol"
            };

            Exception exception = Assert.Throws<NotImplementedException>(() => aPeak.CompareTo(aGene));

            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }
    }
}
