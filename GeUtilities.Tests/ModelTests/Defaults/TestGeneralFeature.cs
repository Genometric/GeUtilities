﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;
using Xunit;

namespace GeUtilities.Tests.ModelTests.Defaults
{
    public class TestGeneralFeature
    {
        [Theory]
        [InlineData(0, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "S", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "F", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 8, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 10, 15, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 10, 20, 90.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 10, 20, 100.0, "F", "At", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(-1, "So", "Fe", 10, 20, 100.0, "Fr", "A", "So", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "S", "Fe", 10, 20, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "F", 10, 20, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 8, 20, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 15, 100.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 90.0, "Fr", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "F", "At")]
        [InlineData(1, "So", "Fe", 10, 20, 100.0, "Fr", "At", "So", "Fe", 10, 20, 100.0, "Fr", "A")]
        public void ComparisonTest(
            int comparisonResult,
            string aSource, string aFeature, int aLeft, int aRight, double aScore, string aFrame, string aAttribute,
            string bSource, string bFeature, int bLeft, int bRight, double bScore, string bFrame, string bAttribute)
        {
            var aGF = new GeneralFeature()
            {
                Source = aSource,
                Feature = aFeature,
                Left = aLeft,
                Right = aRight,
                Score = aScore,
                Frame = aFrame,
                Attribute = aAttribute
            };

            var bGF = new GeneralFeature()
            {
                Source = bSource,
                Feature = bFeature,
                Left = bLeft,
                Right = bRight,
                Score = bScore,
                Frame = bFrame,
                Attribute = bAttribute
            };

            Assert.True(aGF.CompareTo(bGF) == comparisonResult);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            var gf = new GeneralFeature()
            {
                Source = "Source",
                Feature = "Feature",
                Left = 10,
                Right = 20,
                Score = 100.0,
                Frame = "Frame",
                Attribute = "Attribute"
            };

            Assert.True(gf.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            var gf = new GeneralFeature()
            {
                Source = "Source",
                Feature = "Feature",
                Left = 10,
                Right = 20,
                Score = 100.0,
                Frame = "Frame",
                Attribute = "Attribute"
            };

            Assert.True(gf.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            var aGF = new GeneralFeature()
            {
                Source = "Source",
                Feature = "Feature",
                Left = 10,
                Right = 20,
                Score = 100.0,
                Frame = "Frame",
                Attribute = "Attribute"
            };

            var bGF = new GeneralFeature()
            {
                Source = "Source",
                Feature = "Feature",
                Left = 10,
                Right = 20,
                Score = 100.0,
                Frame = "Frame",
                Attribute = "Attribute"
            };

            Assert.True(aGF.CompareTo((object)bGF) == 0);
        }

        [Fact]
        public void CheckNotImplementedComparison()
        {
            var aGF = new GeneralFeature()
            {
                Source = "Source",
                Feature = "Feature",
                Left = 10,
                Right = 20,
                Score = 100.0,
                Frame = "Frame",
                Attribute = "Attribute"
            };

            var aPeak = new ChIPSeqPeak()
            {
                Left = 10,
                Right = 20,
                Value = 100.0,
                Summit = 15,
                Name = "GeUtilities"
            };

            Exception exception = Assert.Throws<NotImplementedException>(() => aGF.CompareTo(aPeak));

            Assert.Equal("Comparison with other object types is not implemented.", exception.Message);
        }
    }
}