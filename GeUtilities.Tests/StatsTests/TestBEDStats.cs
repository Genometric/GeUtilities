// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests
{
    public class TestBEDStats
    {
        private ChIPSeqPeak[] CreatePeaks(double[] pValues)
        {
            var rtv = new ChIPSeqPeak[pValues.Length];
            for (int i = 0; i < pValues.Length; i++)
                rtv[i] = new ChIPSeqPeak()
                {
                    HashKey = (uint)i,
                    Left = 10 * i,
                    Right = (10 * i) + 8,
                    Name = "GeUtilities_" + i,
                    Summit = (10 * i) + ((((10 * i) + 8) - (10 * i)) / 2),
                    Value = pValues[i]
                };
            return rtv;
        }

        [Theory]
        [InlineData(new double[] { 0.001 }, 0.001)]
        [InlineData(new double[] { 0.1, 0.001, 0.0001}, 0.1)]
        public void TestPValueHighest(double[] pValues, double pValueHighest)
        {
            var stats = new BEDStats();
            foreach (var peak in CreatePeaks(pValues))
                stats.Update(peak);

            Assert.True(stats.PValueHighest == pValueHighest);
        }

        [Theory]
        [InlineData(new double[] { 0.001 }, 0.001)]
        [InlineData(new double[] { 0.1, 0.001, 0.0001 }, 0.0001)]
        public void TestPValueLowest(double[] pValues, double pValueLowest)
        {
            var stats = new BEDStats();
            foreach (var peak in CreatePeaks(pValues))
                stats.Update(peak);

            Assert.True(stats.PValueLowest == pValueLowest);
        }
    }
}
