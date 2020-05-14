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

        /// <summary>
        /// The hash key of a peak should be computed considering 
        /// its properties such as value, name, and summit, in 
        /// addition to the interval properties (i.e,. left and right).
        /// According, two peaks with same coordinates but different
        /// properties should not have same hash key. 
        /// </summary>
        [Theory]
        [InlineData(1, "a", 10, 1, "a", 10, true)]
        [InlineData(1, "a", 10, 1, "b", 10, false)]
        [InlineData(1, "a", 10, 1, "a", 99, false)]
        [InlineData(1, "a", 10, 9, "a", 10, false)]
        [InlineData(1, "a", 10, 1, "a", -1, false)]
        [InlineData(2, "a", 10, 3, "a", 10, false)]
        [InlineData(1, "a", -1, 1, "a", -1, true)]
        [InlineData(1, null, -1, 1, null, -1, true)]
        public void PeakHashKeyFuncOfPeakProperties(
            double aValue, string aName, int aSummit, 
            double bValue, string bName, int bSummit,
            bool equalHashKey)
        {
            // Arrange
            var peakA = new Peak(10, 20, aValue, aName, aSummit);
            var peakB = new Peak(10, 20, bValue, bName, bSummit);

            // Act
            var peakAKey = peakA.GetHashCode();
            var peakBKey = peakB.GetHashCode();
            var equality = peakAKey == peakBKey;

            // Assert
            Assert.True(equality == equalHashKey);
        }
    }
}
