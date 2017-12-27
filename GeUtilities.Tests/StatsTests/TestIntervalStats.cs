﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.StatsTests
{
    public class TestIntervalStats
    {
        private ChIPSeqPeak[] CreatePeaks(int[] width)
        {
            var rtv = new ChIPSeqPeak[width.Length / 2];
            for (int i = 0; i < width.Length; i += 2)
                rtv[i / 2] = new ChIPSeqPeak()
                {
                    HashKey = (uint)i,
                    Left = width[i],
                    Right = width[i + 1],
                    Name = "GeUtilities_" + i,
                    Summit = width[i] + ((width[i + 1] - width[i])) / 2,
                    Value = 100.0
                };
            return rtv;
        }

        [Theory]
        [InlineData(new int[0])]
        [InlineData(new int[] { 10, 20 })]
        [InlineData(new int[] { 10, 20, 30, 32 })]
        [InlineData(new int[] { 10, 20, 30, 32, 40, 80 })]
        public void TestCount(int[] width)
        {
            var stats = new BEDStats();
            foreach (var peak in CreatePeaks(width))
                stats.Update(peak);

            Assert.True(stats.Count == width.Length / 2);
        }

        [Theory]
        [InlineData(new int[0], 0)]
        [InlineData(new int[] { 10, 20 }, 10)]
        [InlineData(new int[] { 10, 20, 30, 32 }, 10)]
        [InlineData(new int[] { 10, 20, 30, 32, 40, 80 }, 40)]
        public void TestWidthMax(int[] width, int maxWidth)
        {
            var stats = new BEDStats();
            foreach (var peak in CreatePeaks(width))
                stats.Update(peak);

            Assert.True(stats.WidthMax == maxWidth);
        }
    }
}
