// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers;
using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using Xunit;

namespace GeUtilities.Tests.TRefSeqParser
{
    public class Features
    {
        [Fact]
        public void MultiFeatureFile()
        {
            // Arrange
            var rg = new RegionGenerator { StrandColumn = 12 };
            using (var testFile = new TempFileCreator(rg, genesCount: 10, headerLineCount: 2))
            {
                // Act
                var parser = new RefSeqParser<Gene>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[rg.Chr].Strands[rg.Strand].Intervals.Count == 10);
            }
        }
    }
}
