// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers;
using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using Xunit;

namespace GeUtilities.Tests.TStatus
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
            using (var testFile = new TBEDParser.TempFileCreator(new TBEDParser.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);

                // Assert
                Assert.True(parser.Status == "0");
            }
        }

        [Fact]
        public void CompletedBED()
        {
            // Arrange
            using (var testFile = new TBEDParser.TempFileCreator(new TBEDParser.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                parser.Parse();

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedGeneralFeature()
        {
            // Arrange
            using (var testFile = new TGTFParser.TempFileCreator(new TGTFParser.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                parser.Parse();

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedRefSeqGenes()
        {
            // Arrange
            using (var testFile = new TRefSeqParser.TempFileCreator(new TRefSeqParser.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                parser.Parse();

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedVCF()
        {
            // Arrange
            using (var testFile = new TVCFParser.TempFileCreator(new TVCFParser.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                parser.Parse();

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void Progress()
        {
            // Arrange
            _previousStatus = -1;
            using (var testFile = new TBEDParser.TempFileCreator(new TBEDParser.RegionGenerator(), peaksCount: 50))
            {
                // Act
                var bedParser = new BEDParser<ChIPSeqPeak>(testFile.TempFilePath);
                bedParser.StatusChanged += ParserStatusChanged;
                bedParser.Parse();

                // Asserted in 'ParserStatusChanged'
            }
        }
    }
}
