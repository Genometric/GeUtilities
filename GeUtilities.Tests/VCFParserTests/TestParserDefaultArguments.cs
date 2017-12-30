// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using System.Linq;
using Xunit;

namespace GeUtilities.Tests.VCFParserTests
{
    public class TestParserDefaultArguments
    {
        private string _chr = "chr1";

        [Fact]
        public void AssignHashKey()
        {
            using (TempVCFFileCreator testFile = new TempVCFFileCreator(new VCFColumns(), chr: _chr))
            {
                VCFParser<VCF> vcfParser = new VCFParser<VCF>(testFile.TempFilePath);
                var parsedData = vcfParser.Parse();

                Assert.True(parsedData.Chromosomes[_chr].Strands['*'].Intervals[0].HashKey != 0);
            }
        }
    }
}