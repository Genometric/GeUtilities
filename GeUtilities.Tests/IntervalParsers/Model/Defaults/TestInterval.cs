// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using Xunit;

namespace GeUtilities.Tests.IntervalParsers.ModelTests.Defaults
{
    public class TestInterval
    {
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

        [Theory]
        [InlineData(10, 20, 10, 20, true)]
        [InlineData(10, 20, 10, 30, false)]
        [InlineData(10, 20, 20, 10, false)]
        [InlineData(10, 20, 30, 40, false)]
        public void AssertEquality(int aLeft, int aRight, int bLeft, int bRight, bool expectedResult)
        {
            // Arrange
            var constructor = new IntervalConstructor();
            var intA = constructor.Construct(aLeft, aRight);
            var intB = constructor.Construct(bLeft, bRight);

            // Act
            var comparison = intA.Equals(intB);

            // Assert
            Assert.Equal(expectedResult, comparison);
        }
    }
}
