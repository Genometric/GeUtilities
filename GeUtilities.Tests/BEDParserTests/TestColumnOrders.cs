// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using System;
using Xunit;

namespace GeUtilities.Tests
{
    public class TestColumnOrders
    {
        private string _chr = "chr1";

        [Fact]
        public void TestDefaultBEDColumnOrder()
        {
            int left = 10, right = 20;
            string name = "GeUtilities_00";
            double value = 100.0;
            string peak = _chr + "\t" + left + "\t" + right + "\t" + name + "\t" + value;
            using (TestBEDFileCreator testFile = new TestBEDFileCreator(peak))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TestFilePath, dropPeakIfInvalidValue: true);
                var parsedPeak = bedParser.Parse().Chromosomes[_chr].Strands['*'].Intervals[0];

                Assert.True(parsedPeak.Left == left && parsedPeak.Right == right && parsedPeak.Name == name && parsedPeak.Value == value);
            }
        }
    }
}
