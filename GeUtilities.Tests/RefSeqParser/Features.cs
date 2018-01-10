﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.TRefSeqParser
{
    public class Features
    {
        [Fact]
        public void MultiFeatureFile()
        {
            // Arrange
            var columns = new Columns
            {
                StrandColumn = 12
            };
            using (TempFileCreator testFile = new TempFileCreator(columns, genesCount: 10, headerLineCount: 2))
            {
                // Act
                RefSeqParser<Gene> parser = new RefSeqParser<Gene>(testFile.TempFilePath);
                var parsedData = parser.Parse();

                // Assert
                Assert.True(parsedData.Chromosomes[columns.Chr].Strands[columns.Strand].Intervals.Count == 10);
            }
        }
    }
}