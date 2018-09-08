// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using System;
using Xunit;

namespace GeUtilities.Tests.Interval.Model
{
    public class TestInterval
    {
        public enum Parameter { None, Left, Right, Seed };

        internal static Genometric.GeUtilities.Intervals.Model.Interval GetInterval(Parameter param = Parameter.None, object value = null)
        {
            int left = 10;
            int right = 20;
            string seed = "";

            switch (param)
            {
                case Parameter.Left: left = (int)value; break;
                case Parameter.Right: right = (int)value; break;
                case Parameter.Seed: seed = (string)value; break;
                default: break;
            }

            return new Genometric.GeUtilities.Intervals.Model.Interval(left, right, seed);
        }

        [Fact]
        public void ConstructInterval()
        {
            // Arrange
            var constructor = new IntervalConstructor();

            // Act
            var interval = constructor.Construct(10, 20);

            // Assert
            Assert.True(interval.Left == 10 && interval.Right == 20);
        }

        [Fact]
        public void IntervalNotEqualNullObject()
        {
            // Arrange
            var constructor = new IntervalConstructor();
            var interval = constructor.Construct(10, 20);

            // Act
            var comparison = interval.Equals(null);

            // Assert
            Assert.False(comparison);
        }

        [Fact]
        public void IntervalNotEqualOtherType()
        {
            // Arrange
            var constructor = new IntervalConstructor();
            var interval = constructor.Construct(10, 20);

            // Act
            var comparison = interval.Equals("Genometric");

            // Assert
            Assert.False(comparison);
        }

        [Fact]
        public void NotImplementedCompareTo()
        {
            // Arrange
            var constructor = new IntervalConstructor();
            var intA = constructor.Construct(10, 20);
            var intB = constructor.Construct(30, 40);

            // Act & Assert
            Exception exception = Assert.Throws<NotImplementedException>(() => intA.CompareTo(intB));

            // Act & Assert
            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }

        [Theory]
        [InlineData(Parameter.None, null, null, true)]
        [InlineData(Parameter.Left, 10, 8, false)]
        [InlineData(Parameter.Left, 8, 10, false)]
        [InlineData(Parameter.Right, 20, 18, false)]
        [InlineData(Parameter.Right, 18, 20, false)]
        [InlineData(Parameter.Seed, "GU", "G", false)]
        [InlineData(Parameter.Seed, "G", "GU", false)]
        public void Equal(Parameter param, object v1, object v2, bool expected)
        {
            // Arrange
            var a = GetInterval(param, v1);
            var b = GetInterval(param, v2);

            // Act
            var actual = a.Equals(b);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
