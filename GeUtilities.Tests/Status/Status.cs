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
            using (TBEDParser.TempFileCreator testFile = new TBEDParser.TempFileCreator(new TBEDParser.Columns()))
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
            using (TBEDParser.TempFileCreator testFile = new TBEDParser.TempFileCreator(new TBEDParser.Columns()))
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
            using (TGTFParser.TempFileCreator testFile = new TGTFParser.TempFileCreator(new TGTFParser.Columns()))
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
            using (TRefSeqParser.TempFileCreator testFile = new TRefSeqParser.TempFileCreator(new TRefSeqParser.Columns()))
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
            using (TVCFParser.TempFileCreator testFile = new TVCFParser.TempFileCreator(new TVCFParser.Columns()))
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
            using (TBEDParser.TempFileCreator testFile = new TBEDParser.TempFileCreator(new TBEDParser.Columns(), peaksCount: 50))
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
