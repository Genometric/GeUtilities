﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers;
using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using Xunit;

namespace GeUtilities.Tests.IntervalParsers.TStatus
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
            using (var file = new BED.TempFileCreator(new BED.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser();

                // Assert
                Assert.True(parser.Status == "0");
            }
        }

        [Fact]
        public void CompletedBED()
        {
            // Arrange
            using (var file = new BED.TempFileCreator(new BED.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser();
                parser.Parse(file.Path);

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedGeneralFeature()
        {
            // Arrange
            using (var file = new GTF.TempFileCreator(new GTF.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser();
                parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedRefSeqGenes()
        {
            // Arrange
            using (var file = new RefSeq.TempFileCreator(new RefSeq.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser();
                parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void CompletedVCF()
        {
            // Arrange
            using (var file = new VCF.TempFileCreator(new VCF.RegionGenerator()))
            {
                // Act
                var parser = new BEDParser();
                parser.Parse(file.TempFilePath);

                // Assert
                Assert.True(parser.Status == "100");
            }
        }

        [Fact]
        public void Progress()
        {
            // Arrange
            _previousStatus = -1;
            using (var file = new BED.TempFileCreator(new BED.RegionGenerator(), peaksCount: 50))
            {
                // Act
                var bedParser = new BEDParser();
                bedParser.StatusChanged += ParserStatusChanged;
                bedParser.Parse(file.Path);

                // Asserted in 'ParserStatusChanged'
            }
        }
    }
}
