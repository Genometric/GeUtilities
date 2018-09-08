// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using System;
using Xunit;

namespace GeUtilities.Tests.IntervalParsers.ModelTests.Defaults
{
    public class TestGene
    {
        internal static Gene GetTempGene()
        {
            return new Gene(10, 20, "RefSeqID", "GeneSymbol");
        }

        [Theory]
        [InlineData(0, 10, 20, "RefSeqID", "GeneSymbol", 10, 20, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 8, 20, "RefSeqID", "GeneSymbol", 10, 20, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 10, 16, "RefSeqID", "GeneSymbol", 10, 20, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 10, 20, "RefSeq", "GeneSymbol", 10, 20, "RefSeqID", "GeneSymbol")]
        [InlineData(-1, 10, 20, "RefSeqID", "Gene", 10, 20, "RefSeqID", "GeneSymbol")]
        [InlineData(1, 10, 20, "RefSeqID", "GeneSymbol", 8, 20, "RefSeqID", "GeneSymbol")]
        [InlineData(1, 10, 20, "RefSeqID", "GeneSymbol", 10, 18, "RefSeqID", "GeneSymbol")]
        [InlineData(1, 10, 20, "RefSeqID", "GeneSymbol", 10, 20, "RefSeq", "GeneSymbol")]
        [InlineData(1, 10, 20, "RefSeqID", "GeneSymbol", 10, 20, "RefSeqID", "Gene")]
        public void ComparisonTest(
            int comparisonResult,
            int aLeft, int aRight, string aRefSeqID, string aGeneSymbol,
            int bLeft, int bRight, string bRefSeqID, string bGeneSymbol)
        {
            // Arrange
            var aGene = new Gene(aLeft, aRight, aRefSeqID, aGeneSymbol);
            var bGene = new Gene(bLeft, bRight, bRefSeqID, bGeneSymbol);

            // Act & Assert
            Assert.True(aGene.CompareTo(bGene) == comparisonResult);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            // Arrange
            var gene = GetTempGene();

            // Act & Assert
            Assert.True(gene.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            // Arrange
            var gene = GetTempGene();

            // Act & Assert
            Assert.True(gene.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            // Arrange
            var aGene = GetTempGene();
            var bGene = GetTempGene();

            // Act & Assert
            Assert.True(aGene.CompareTo((object)bGene) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            // Arrange
            var aGene = GetTempGene();
            var aPeak = TestChIPSeqPeak.GetPeak();

            // Act
            Exception exception = Assert.Throws<NotImplementedException>(() => aGene.CompareTo(aPeak));

            // Assert
            Assert.False(String.IsNullOrEmpty(exception.Message));
            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }

        [Theory]
        [InlineData("id", "id", 0)]
        [InlineData("id", null, 1)]
        [InlineData(null, "id", -1)]
        [InlineData(null, null, -1)]
        public void CompareTwoGenesWithNullRefSeqID(string aRefSeqID, string bRefSeqID, int expectedResult)
        {
            // Arrange
            var geneA = new Gene(10, 20, aRefSeqID, "symbol");
            var geneB = new Gene(10, 20, bRefSeqID, "symbol");

            // Act
            var comparison = geneA.CompareTo(geneB);

            // Assert
            Assert.Equal(expectedResult, comparison);
        }

        [Theory]
        [InlineData("symbol", "symbol", 0)]
        [InlineData("symbol", null, 1)]
        [InlineData(null, "symbol", -1)]
        [InlineData(null, null, -1)]
        public void CompareTwoGenesWithNullSymbol(string aSymbol, string bSymbol, int expectedResult)
        {
            // Arrange
            var geneA = new Gene(10, 20, "id", aSymbol);
            var geneB = new Gene(10, 20, "id", bSymbol);

            // Act
            var comparison = geneA.CompareTo(geneB);

            // Assert
            Assert.Equal(expectedResult, comparison);
        }
    }
}
