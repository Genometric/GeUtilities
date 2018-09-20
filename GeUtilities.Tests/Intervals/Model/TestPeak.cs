// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Model
{
    public class TestPeak
    {
        public enum Parameter { None, Left, Right, Value, Summit, Name };

        internal static Peak GetPeak(Parameter param = Parameter.None, object value = null)
        {
            int left = 10;
            int right = 20;
            double pValue = 100.0;

            switch (param)
            {
                case Parameter.Left: left = (int)value; break;
                case Parameter.Right: right = (int)value; break;
                case Parameter.Value: pValue = (double)value; break;
                case Parameter.Summit: return new Peak(left, right, pValue, summit: (int)value);
                case Parameter.Name: return new Peak(left, right, pValue, name: (string)value);
                default: break;
            }

            return new Peak(left, right, pValue);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            // Arrange
            var peak = GetPeak();

            // Act & Assert
            Assert.True(peak.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            // Arrange
            var peak = GetPeak();

            // Act & Assert
            Assert.True(peak.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            // Arrange
            var aPeak = GetPeak();
            var bPeak = GetPeak();

            // Act & Assert
            Assert.True(aPeak.CompareTo((object)bPeak) == 0);
        }

        [Fact]
        public void ThisProceedsDifferentType()
        {
            // Arrange
            var aPeak = GetPeak();
            var aGene = TestRefSeqGene.GetGene();

            // Act & Assert
            Assert.True(aPeak.CompareTo(aGene) == 1);
        }

        [Theory]
        [InlineData(Parameter.None, null, null, 0)]
        [InlineData(Parameter.Left, 10, 8, 1)]
        [InlineData(Parameter.Left, 8, 10, -1)]
        [InlineData(Parameter.Right, 20, 16, 1)]
        [InlineData(Parameter.Right, 16, 20, -1)]
        [InlineData(Parameter.Value, 100.0, 90.0, 1)]
        [InlineData(Parameter.Value, 90.0, 100.0, -1)]
        [InlineData(Parameter.Summit, 15, 12, 1)]
        [InlineData(Parameter.Summit, 12, 15, -1)]
        [InlineData(Parameter.Name, "GU", "G", 1)]
        [InlineData(Parameter.Name, "G", "GU", -1)]
        [InlineData(Parameter.Name, "GU", null, 1)]
        [InlineData(Parameter.Name, null, "GU", -1)]
        [InlineData(Parameter.Name, null, null, 0)]
        public void CompareTo(Parameter param, object v1, object v2, int expected)
        {
            // Arrange
            var a = GetPeak(param, v1);
            var b = GetPeak(param, v2);

            // Act
            var actual = a.CompareTo(b);
            var equal = expected == 0;

            // Assert
            Assert.Equal(expected, actual);
            Assert.True(a.Equals(b) == equal);
        }
    }
}
