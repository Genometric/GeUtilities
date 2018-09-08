// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Interval.Model
{
    public class GeneralFeature : Interval, IGeneralFeature
    {
        public GeneralFeature(int left, int right, string source, string feature, double score,
            string frame, string attribute, string hashSeed = "") :
            base(left, right, source + feature + score.ToString() + frame + attribute + hashSeed)
        {
            Source = source;
            Feature = feature;
            Score = score;
            Frame = frame;
            Attribute = attribute;
        }

        public string Source { get; }
        public string Feature { get; }
        public double Score { get; }
        public string Frame { get; }
        public string Attribute { get; }

        public new int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is GeneralFeature)
                return CompareTo(obj as GeneralFeature);
            else
                throw new NotImplementedException("Comparison with other object types is not implemented.");
        }

        public int CompareTo(IGeneralFeature other)
        {
            if (other == null) return 1;
            if (Source == null) return -1;
            if (other.Source == null) return 1;
            int compareResult = Source.CompareTo(other.Source);
            if (compareResult != 0) return compareResult;
            if (Feature == null) return -1;
            if (other.Feature == null) return 1;
            compareResult = Feature.CompareTo(other.Feature);
            if (compareResult != 0) return compareResult;
            compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            compareResult = Right.CompareTo(other.Right);
            if (compareResult != 0) return compareResult;
            compareResult = Score.CompareTo(other.Score);
            if (compareResult != 0) return compareResult;
            if (Frame == null) return -1;
            if (other.Frame == null) return 1;
            compareResult = Frame.CompareTo(other.Frame);
            if (compareResult != 0) return compareResult;
            if (Attribute == null) return -1;
            if (other.Attribute == null) return 1;
            return Attribute.CompareTo(other.Attribute);
        }
    }
}
