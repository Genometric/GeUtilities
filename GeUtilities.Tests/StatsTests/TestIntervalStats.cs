// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.StatsTests
{
    public class TestIntervalStats
    {
        private ChIPSeqPeak[] CreatePeaks(int count)
        {
            var rtv = new ChIPSeqPeak[count];
            for (int i = 0; i < count; i++)
                rtv[i] = new ChIPSeqPeak()
                {
                    HashKey = (uint)i,
                    Left = 10 * i,
                    Right = (10 * i) + 8,
                    Name = "GeUtilities_" + i,
                    Summit = (10 * i) + ((((10 * i) + 8) - (10 * i)) / 2),
                    Value = 100.0
                };
            return rtv;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(9)]
        public void TestCount(int count)
        {
            var stats = new BEDStats();
            foreach (var peak in CreatePeaks(count))
                stats.Update(peak);

            Assert.True(stats.Count == count);
        }
    }
}
