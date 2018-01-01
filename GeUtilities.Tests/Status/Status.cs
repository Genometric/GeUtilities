// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.Parsers;
using Xunit;

namespace GeUtilities.Tests.Status
{
    public class Status
    {
        private double _previousStatus;
        private void ParserStatusChanged(object sender, ParserEventArgs e)
        {
            double.TryParse(e.Value, out double status);

            // Assert
            Assert.True(status > _previousStatus);

            _previousStatus = status;
        }

        [Fact]
        public void Initial()
        {
            // Arrange
            using (BEDParser.TempFileCreator testFile = new BEDParser.TempFileCreator(new BEDParser.Columns()))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);

                // Assert
                Assert.True(parser.Status == "0");
            }
        }

        [Fact]
        public void CompletedBED()
        {
            // Arrange
            using (BEDParser.TempFileCreator testFile = new BEDParser.TempFileCreator(new BEDParser.Columns()))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedBED = parser.Parse();

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedGeneralFeature()
        {
            // Arrange
            using (GeneralFeatureParser.TempFileCreator testFile = new GeneralFeatureParser.TempFileCreator(new GeneralFeatureParser.Columns()))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedBED = parser.Parse();

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedRefSeqGenes()
        {
            // Arrange
            using (RefSeqGenesParser.TempFileCreator testFile = new RefSeqGenesParser.TempFileCreator(new RefSeqGenesParser.Columns()))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedBED = parser.Parse();

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedVCF()
        {
            // Arrange
            using (VCFParser.TempFileCreator testFile = new VCFParser.TempFileCreator(new VCFParser.Columns()))
            {
                // Act
                BEDParser<ChIPSeqPeak> parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                var parsedBED = parser.Parse();

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void Progress()
        {
            // Arrange
            _previousStatus = -1;
            using (BEDParser.TempFileCreator testFile = new BEDParser.TempFileCreator(new BEDParser.Columns(), peaksCount: 50))
            {
                // Act
                BEDParser<ChIPSeqPeak> bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                bedParser.StatusChanged += ParserStatusChanged;
                var parsedBED = bedParser.Parse();

                // Asserted in 'ParserStatusChanged'
            }
        }
    }
}
