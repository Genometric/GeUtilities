// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using System;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Model
{
    public class TestGene
    {
        public enum Parameter { None, Left, Right, ID, Symbol };

        internal static Gene GetGene(Parameter param = Parameter.None, object value = null)
        {
            int left = 10;
            int right = 20;
            string id = "RefSeqID";
            string symbol = "GeneSymbol";

            switch (param)
            {
                case Parameter.Left: left = (int)value; break;
                case Parameter.Right: right = (int)value; break;
                case Parameter.ID: id = (string)value; break;
                case Parameter.Symbol: symbol = (string)value; break;
                default: break;
            }

            return new Gene(left, right, id, symbol);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            // Arrange
            var gene = GetGene();

            // Act & Assert
            Assert.True(gene.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            // Arrange
            var gene = GetGene();

            // Act & Assert
            Assert.True(gene.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            // Arrange
            var aGene = GetGene();
            var bGene = GetGene();

            // Act & Assert
            Assert.True(aGene.CompareTo((object)bGene) == 0);
        }

        [Fact]
        public void ThisProceedsDifferentType()
        {
            // Arrange
            var aGene = GetGene();
            var aPeak = TestChIPSeqPeak.GetPeak();

            // Act & Assert
            Assert.True(aGene.CompareTo(aPeak) == 1);
        }

        [Theory]
        [InlineData(Parameter.None, null, null, 0)]
        [InlineData(Parameter.Left, 10, 8, 1)]
        [InlineData(Parameter.Left, 8, 10, -1)]
        [InlineData(Parameter.Right, 20, 16, 1)]
        [InlineData(Parameter.Right, 16, 20, -1)]
        [InlineData(Parameter.Symbol, "GU", "G", 1)]
        [InlineData(Parameter.Symbol, "G", "GU", -1)]
        [InlineData(Parameter.Symbol, "GU", null, 1)]
        [InlineData(Parameter.Symbol, null, "GU", -1)]
        [InlineData(Parameter.Symbol, null, null, -1)]
        [InlineData(Parameter.ID, "GU", "G", 1)]
        [InlineData(Parameter.ID, "G", "GU", -1)]
        [InlineData(Parameter.ID, "GU", null, 1)]
        [InlineData(Parameter.ID, null, "GU", -1)]
        [InlineData(Parameter.ID, null, null, -1)]
        public void CompareTo(Parameter param, object v1, object v2, int expected)
        {
            // Arrange
            var a = GetGene(param, v1);
            var b = GetGene(param, v2);

            // Act
            var actual = a.CompareTo(b);
            var equal = expected == 0;

            // Assert
            Assert.Equal(expected, actual);
            Assert.True(a.Equals(b) == equal);
        }
    }
}
