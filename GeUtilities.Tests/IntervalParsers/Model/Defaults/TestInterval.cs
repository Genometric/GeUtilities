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
    }
}
