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

        [Theory]
        [InlineData(0, 1, 2, 3, 4)]
        [InlineData(1, 0, 2, 3, 4)]
        [InlineData(1, 2, 0, 3, 4)]
        [InlineData(1, 2, 3, 0, 4)]
        [InlineData(1, 2, 3, 4, 0)]
        [InlineData(4, 3, 2, 1, 0)]
        [InlineData(2, 1, 0, 4, 3)]
        [InlineData(5, 6, 7, 8, 9)]
        public void TestColumnsShuffle(sbyte chrColumn, sbyte leftColumn, sbyte rightColumn, byte nameColumn, byte valueColumn)
        {
            int left = 10, right = 20;
            string name = "GeUtilities_00", peak = "";
            double value = 100.0;
            for (int i = 0; i <= Math.Max(chrColumn, Math.Max(leftColumn, Math.Max(rightColumn, Math.Max(nameColumn, valueColumn)))); i++)
            {
                if (chrColumn == i) peak += _chr + "\t";
                else if (leftColumn == i) peak += left + "\t";
                else if (rightColumn == i) peak += right + "\t";
                else if (nameColumn == i) peak += name + "\t";
                else if (valueColumn == i) peak += value + "\t";
                else peak += "AbCd\t";
            }

            using (TestBEDFileCreator testFile = new TestBEDFileCreator(peak))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(
                    testFile.TestFilePath,
                    chrColumn: chrColumn,
                    leftEndColumn: leftColumn,
                    rightEndColumn: rightColumn,
                    nameColumn: nameColumn,
                    valueColumn: valueColumn,
                    strandColumn: -1);
                var parsedPeak = bedParser.Parse().Chromosomes[_chr].Strands['*'].Intervals[0];

                Assert.True(parsedPeak.Left == left && parsedPeak.Right == right && parsedPeak.Name == name && parsedPeak.Value == value);
            }
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(0, -1)]
        [InlineData(0, 99)]
        [InlineData(3, -1)]
        [InlineData(3, 99)]
        [InlineData(10, -1)]
        [InlineData(10, 99)]
        public void TestSummit(sbyte summitColumn, int summit)
        {
            int left = 10, right = 20;
            summit = summit == -1 ? left + ((right - left) / 2) : summit;
            string name = "GeUtilities_00", peak = "";
            double value = 100.0;
            sbyte chrColumn = -1, leftColumn = -1, rightColumn = -1, nameColumn = -1, valueColumn = -1;
            for (sbyte i = 0; i < Math.Max((sbyte)6, summitColumn); i++)
            {
                if (i == summitColumn) continue;
                else if (chrColumn == -1) chrColumn = i;
                else if (leftColumn == -1) leftColumn = i;
                else if (rightColumn == -1) rightColumn = i;
                else if (nameColumn == -1) nameColumn = i;
                else if (valueColumn == -1) valueColumn = i;
            }

            for (int i = 0; i <= Math.Max((sbyte)5, summitColumn); i++)
            {
                if (chrColumn == i) peak += _chr + "\t";
                else if (leftColumn == i) peak += left + "\t";
                else if (rightColumn == i) peak += right + "\t";
                else if (nameColumn == i) peak += name + "\t";
                else if (valueColumn == i) peak += value + "\t";
                else if (summitColumn == i) peak += summit + "\t";
                else peak += "AbCd\t";
            }

            using (TestBEDFileCreator testFile = new TestBEDFileCreator(peak))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(
                    testFile.TestFilePath,
                    chrColumn: chrColumn,
                    leftEndColumn: leftColumn,
                    rightEndColumn: rightColumn,
                    nameColumn: (byte)nameColumn,
                    valueColumn: (byte)valueColumn,
                    summitColumn: summitColumn,
                    strandColumn: -1);
                var parsedPeak = bedParser.Parse().Chromosomes[_chr].Strands['*'].Intervals[0];

                Assert.True(parsedPeak.Summit == summit);
            }
        }
    }
}
