// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Genometric.GeUtilities.ReferenceGenomes;
using GeUtilities.Tests.BEDParserTests;
using System;
using System.IO;
using Xunit;

namespace GeUtilities.Tests.ModelTests
{
    public class ParsedIntervals
    {
        [Fact]
        public void TestFileName()
        {
            string peak = "chr1\t10\t20\tName\t100.0";
            using (TempBEDFileCreator testFile = new TempBEDFileCreator(peak))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedBED = bedParser.Parse();

                Assert.True(parsedBED.FileName == Path.GetFileName(testFile.TempFilePath));
            }
        }

        [Fact]
        public void TestFilePath()
        {
            string peak = "chr1\t10\t20\tName\t100.0";
            using (TempBEDFileCreator testFile = new TempBEDFileCreator(peak))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedBED = bedParser.Parse();

                Assert.True(parsedBED.FilePath == Path.GetFullPath(testFile.TempFilePath));
            }
        }

        [Theory]
        [InlineData(Assemblies.hg19)]
        [InlineData(Assemblies.mm10)]
        [InlineData(Assemblies.Unknown)]
        public void TestAssembly(Assemblies assembly)
        {
            string peak = "chr1\t10\t20\tName\t100.0";
            using (TempBEDFileCreator testFile = new TempBEDFileCreator(peak))
            {
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath, assembly: assembly);
                var parsedBED = bedParser.Parse();

                Assert.True(parsedBED.Assembly == assembly);
            }
        }
    }
}
