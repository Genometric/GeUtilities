// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.VCFParser
{
    public class DefaultArguments
    {
        [Fact]
        public void AssignHashKey()
        {
            var columns = new Columns();
            using (TempFileCreator testFile = new TempFileCreator(columns))
            {
                VCFParser<Variant> parser = new VCFParser<Variant>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals[0].HashKey != 0);
            }
        }
    }
}