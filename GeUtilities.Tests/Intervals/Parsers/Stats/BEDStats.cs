// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using Genometric.GeUtilities.Intervals.Parsers;
using Genometric.GeUtilities.Tests.Intervals.Parsers.BED;
using System;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.Stats
{
    public class BedStats
    {
        private ChIPSeqPeak[] CreatePeaks(double[] pValues)
        {
            var rtv = new ChIPSeqPeak[pValues.Length];
            for (int i = 0; i < pValues.Length; i++)
                rtv[i] = new ChIPSeqPeak(
                    left: 10 * i,
                    right: (10 * i) + 8,
                    value: pValues[i],
                    summit: (10 * i) + ((((10 * i) + 8) - (10 * i)) / 2),
                    name: "GeUtilities_" + i);
            return rtv;
        }

        [Theory]
        [InlineData(new double[] { 0.001 }, 0.001)]
        [InlineData(new double[] { 0.1, 0.01, 0.001, 0.0001 }, 0.1)]
        public void TestPValueHighest(double[] pValues, double pValueHighest)
        {
            var stats = new Genometric.GeUtilities.Intervals.Parsers.Model.BEDStats();
            foreach (var peak in CreatePeaks(pValues))
                stats.Update(peak);

            Assert.True(stats.PValueHighest == pValueHighest);
        }

        [Theory]
        [InlineData(new double[] { 0.001 }, 0.001)]
        [InlineData(new double[] { 0.1, 0.01, 0.001, 0.0001 }, 0.0001)]
        public void TestPValueLowest(double[] pValues, double pValueLowest)
        {
            var stats = new Genometric.GeUtilities.Intervals.Parsers.Model.BEDStats();
            foreach (var peak in CreatePeaks(pValues))
                stats.Update(peak);

            Assert.True(stats.PValueLowest == pValueLowest);
        }

        [Theory]
        [InlineData(new double[] { 0.001 }, 0.001)]
        [InlineData(new double[] { 0.1, 0.01, 0.001, 0.0001 }, 0.027775)]
        public void TestPValueMean(double[] pValues, double mean)
        {
            var stats = new Genometric.GeUtilities.Intervals.Parsers.Model.BEDStats();
            foreach (var peak in CreatePeaks(pValues))
                stats.Update(peak);

            Assert.True(stats.PValueMean == mean);
        }

        [Theory]
        [InlineData(new double[] { 0.001 }, 0.0)]
        [InlineData(new double[] { 0.1, 0.01, 0.001, 0.0001 }, 0.041878418)]
        public void TestPValueSTDV(double[] pValues, double stdv)
        {
            var stats = new Genometric.GeUtilities.Intervals.Parsers.Model.BEDStats();
            foreach (var peak in CreatePeaks(pValues))
                stats.Update(peak);

            Assert.True(Math.Round(stats.PValuePSTDV, 9) == stdv);
        }

        [Fact]
        public void TestPValueHighestInParsedDataPerChr()
        {
            string[] peaks = new string[]
            {
                "chr1\t10\t20\tGeUtilities_00\t0.1",
                "chr1\t30\t40\tGeUtilities_01\t0.01",
                "chr1\t50\t60\tGeUtilities_02\t0.001",
                "chr2\t10\t20\tGeUtilities_00\t0.2",
            };

            using (var file = new TempFileCreator(peaks))
            {
                var parser = new BedParser()
                {
                    PValueFormat = PValueFormats.SameAsInput
                };
                var parsedData = parser.Parse(file.Path);

                Assert.True(parsedData.Chromosomes["chr1"].Statistics.PValueHighest == 0.1);
            }
        }

        [Fact]
        public void TestPValueHighestInParsedData()
        {
            string[] peaks = new string[]
            {
                "chr1\t10\t20\tGeUtilities_00\t0.1",
                "chr1\t30\t40\tGeUtilities_01\t0.01",
                "chr1\t50\t60\tGeUtilities_02\t0.001",
                "chr2\t10\t20\tGeUtilities_00\t0.2",
            };

            using (var file = new TempFileCreator(peaks))
            {
                var parser = new BedParser()
                {
                    PValueFormat = PValueFormats.SameAsInput
                };
                var parsedData = parser.Parse(file.Path);

                Assert.True(parsedData.Statistics.PValueHighest == 0.2);
            }
        }
    }
}
