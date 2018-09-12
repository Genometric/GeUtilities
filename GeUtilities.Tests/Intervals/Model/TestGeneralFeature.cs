// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using System;
using Xunit;

namespace Genometric.GeUtilities.Tests.Intervals.Model
{
    public class TestGeneralFeature
    {
        public enum Parameter { None, Left, Right, Source, Feature, Score, Frame, Attribute };

        internal static GeneralFeature GetFeature(Parameter param = Parameter.None, object value = null)
        {
            int left = 10;
            int right = 20;
            string source = "Source";
            string feature = "Feature";
            double score = 100.0;
            string frame = "Frame";
            string attribute = "Attribute";

            switch (param)
            {
                case Parameter.Left: left = (int)value; break;
                case Parameter.Right: right = (int)value; break;
                case Parameter.Source: source = (string)value; break;
                case Parameter.Feature: feature = (string)value; break;
                case Parameter.Score: score = (double)value; break;
                case Parameter.Frame: frame = (string)value; break;
                case Parameter.Attribute: attribute = (string)value; break;
                default: break;
            }

            return new GeneralFeature(left, right, source, feature, score, frame, attribute);
        }

        [Fact]
        public void ComparisonTestWithNullObject()
        {
            // Arrange
            var gf = GetFeature();

            // Act & Assert
            Assert.True(gf.CompareTo(null) == 1);
        }

        [Fact]
        public void ComparisonTestWithNullObject2()
        {
            // Arrange
            var gf = GetFeature();

            // Act & Assert
            Assert.True(gf.CompareTo((object)null) == 1);
        }

        [Fact]
        public void ComparisonTestWithAPeakAsObject()
        {
            // Arrange
            var aGF = GetFeature();
            var bGF = GetFeature();

            // Act & Assert
            Assert.True(aGF.CompareTo((object)bGF) == 0);
        }

        [Fact]
        public void ThisProceedsDifferentType()
        {
            // Arrange
            var aVariant = GetFeature();
            var aPeak = TestChIPSeqPeak.GetPeak();

            // Act & Assert
            Assert.True(aVariant.CompareTo(aPeak) == 1);
        }

        [Theory]
        [InlineData(Parameter.None, null, null, 0)]
        [InlineData(Parameter.Left, 10, 8, 1)]
        [InlineData(Parameter.Left, 8, 10, -1)]

        [InlineData(Parameter.Right, 10, 8, 1)]
        [InlineData(Parameter.Right, 8, 10, -1)]

        [InlineData(Parameter.Score, 100.0, 80.0, 1)]
        [InlineData(Parameter.Score, 80.0, 100.0, -1)]

        [InlineData(Parameter.Source, "GU", "G", 1)]
        [InlineData(Parameter.Source, "G", "GU", -1)]
        [InlineData(Parameter.Source, "GU", null, 1)]
        [InlineData(Parameter.Source, null, "GU", -1)]
        [InlineData(Parameter.Source, null, null, -1)]

        [InlineData(Parameter.Feature, "GU", "G", 1)]
        [InlineData(Parameter.Feature, "G", "GU", -1)]
        [InlineData(Parameter.Feature, "GU", null, 1)]
        [InlineData(Parameter.Feature, null, "GU", -1)]
        [InlineData(Parameter.Feature, null, null, -1)]

        [InlineData(Parameter.Frame, "GU", "G", 1)]
        [InlineData(Parameter.Frame, "G", "GU", -1)]
        [InlineData(Parameter.Frame, "GU", null, 1)]
        [InlineData(Parameter.Frame, null, "GU", -1)]
        [InlineData(Parameter.Frame, null, null, -1)]

        [InlineData(Parameter.Attribute, "GU", "G", 1)]
        [InlineData(Parameter.Attribute, "G", "GU", -1)]
        [InlineData(Parameter.Attribute, "GU", null, 1)]
        [InlineData(Parameter.Attribute, null, "GU", -1)]
        [InlineData(Parameter.Attribute, null, null, -1)]
        public void CompareTo(Parameter param, object v1, object v2, int expected)
        {
            // Arrange
            var a = GetFeature(param, v1);
            var b = GetFeature(param, v2);

            // Act
            var actual = a.CompareTo(b);
            var equal = expected == 0;

            // Assert
            Assert.Equal(expected, actual);
            Assert.True(a.Equals(b) == equal);
        }
    }
}
