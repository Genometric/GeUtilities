// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Parsers;
using Genometric.GeUtilities.ReferenceGenomes;
using Genometric.GeUtilities.Tests.Intervals.Parsers.Bed;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers
{
    public class ParsedIntervals
    {
        [Fact]
        public void TestFileHashKey()
        {
            // Arrange
            string peak = "chr1\t10\t20\tName\t100.0";
            using (var file = new TempFileCreator(peak))
            {
                // Act
                var parser = new BedParser();
                var parsedBED = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedBED.FileHashKey != 0);
            }
        }

        [Fact]
        public void TestFileName()
        {
            // Arrange
            string peak = "chr1\t10\t20\tName\t100.0";
            using (var file = new TempFileCreator(peak))
            {
                // Act
                var parser = new BedParser();
                var parsedBED = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedBED.FileName == Path.GetFileName(file.Path));
            }
        }

        [Fact]
        public void TestFilePath()
        {
            // Arrange
            string peak = "chr1\t10\t20\tName\t100.0";
            using (var file = new TempFileCreator(peak))
            {
                // Act
                var parser = new BedParser();
                var parsedBED = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedBED.FilePath == Path.GetFullPath(file.Path));
            }
        }

        [Fact]
        public void TestNullFilePath()
        {
            // Arrange
            var parser = new BedParser();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }

        [Theory]
        [InlineData(Assemblies.hg19)]
        [InlineData(Assemblies.mm10)]
        [InlineData(Assemblies.Unknown)]
        public void TestAssembly(Assemblies assembly)
        {
            // Arrange
            string peak = "chr1\t10\t20\tName\t100.0";
            using (var file = new TempFileCreator(peak))
            {
                // Act
                var parser = new BedParser()
                {
                    Assembly = assembly
                };
                var parsedBED = parser.Parse(file.Path);

                // Assert
                Assert.True(parsedBED.Assembly == assembly);
            }
        }

        [Fact]
        public void CanTellIfStrandContainsAnInterval()
        {
            // Arrange
            using (var file = new TempFileCreator())
            {
                // Act
                var parser = new BedParser();
                var parsedBED = parser.Parse(file.Path);
                var interval = parsedBED.Chromosomes["chr1"].Strands['.'].Intervals.ToList()[0];

                // Assert
                Assert.True(parsedBED.Chromosomes["chr1"].Strands['.'].Contains(interval));
            }
        }

        [Fact]
        public void CanGetAnIntervalFromStrand()
        {
            // Arrange
            using (var file = new TempFileCreator())
            {
                // Act
                var parser = new BedParser();
                var parsedBED = parser.Parse(file.Path);
                var expected = parsedBED.Chromosomes["chr1"].Strands['.'].Intervals.ToList()[0];

                // Assert
                GeUtilities.Intervals.Model.Peak i;
                Assert.True(parsedBED.Chromosomes["chr1"].Strands['.'].TryGet(expected.GetHashCode(), out i));
                Assert.Equal(expected, i);
            }
        }
    }
}
