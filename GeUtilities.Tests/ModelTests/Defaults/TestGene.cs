// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;
using Xunit;

namespace GeUtilities.Tests.ModelTests.Defaults
{
    public class TestGene
    {
        internal static Gene GetTempGene()
        {
            return new Gene()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                RefSeqID = "RefSeqID",
                GeneSymbol = "GeneSymbol"
            };
        }

        [Theory]
        [InlineData(0, 10, 20, 100.0, "RefSeqID", "GeneSymbol", 10, 20, 100.0, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 8, 20, 100.0, "RefSeqID", "GeneSymbol", 10, 20, 100.0, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 10, 16, 100.0, "RefSeqID", "GeneSymbol", 10, 20, 100.0, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 10, 20, 90.0, "RefSeqID", "GeneSymbol", 10, 20, 100.0, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 10, 20, 100.0, "RefSeq", "GeneSymbol", 10, 20, 100.0, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 10, 20, 100.0, "RefSeqID", "Gene", 10, 20, 100.0, "RefSeqID", "GeneSymbol")]
        [InlineData(1, 10, 20, 100.0, "RefSeqID", "GeneSymbol", 8, 20, 100.0, "RefSeqID", "GeneSymbol")]
        [InlineData(1, 10, 20, 100.0, "RefSeqID", "GeneSymbol", 10, 18, 100.0, "RefSeqID", "GeneSymbol")]
        [InlineData(1, 10, 20, 100.0, "RefSeqID", "GeneSymbol", 10, 20, 90.0, "RefSeqID", "GeneSymbol")]
        [InlineData(1, 10, 20, 100.0, "RefSeqID", "GeneSymbol", 10, 20, 100.0, "RefSeq", "GeneSymbol")]
        [InlineData(1, 10, 20, 100.0, "RefSeqID", "GeneSymbol", 10, 20, 100.0, "RefSeqID", "Gene")]
        public void ComparisonTest(
            int comparisonResult,
            int aLeft, int aRight, double aValue, string aRefSeqID, string aGeneSymbol,
            int bLeft, int bRight, double bValue, string bRefSeqID, string bGeneSymbol)
        {
            var aGene = new Gene()
            {
                Left = aLeft,
                Right = aRight,
                Value = aValue,
                RefSeqID = aRefSeqID,
                GeneSymbol = aGeneSymbol
            };

            var bGene = new Gene()
            {
                Left = bLeft,
                Right = bRight,
                Value = bValue,
                RefSeqID = bRefSeqID,
                GeneSymbol = bGeneSymbol
            };

            Assert.True(aGene.CompareTo(bGene) == comparisonResult);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            var gene = GetTempGene();

            Assert.True(gene.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            var gene = GetTempGene();

            Assert.True(gene.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            var aGene = GetTempGene();
            var bGene = GetTempGene();

            Assert.True(aGene.CompareTo((object)bGene) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            var aGene = GetTempGene();
            var aPeak = TestChIPSeqPeak.GetTempChIPSeqPeak();

            Exception exception = Assert.Throws<NotImplementedException>(() => aGene.CompareTo(aPeak));

            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }
    }
}
