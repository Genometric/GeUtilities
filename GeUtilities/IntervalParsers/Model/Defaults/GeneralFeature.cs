// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.IntervalParsers.Model.Defaults
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

        public string Source { set; get; }
        public string Feature { set; get; }
        public double Score { set; get; }
        public string Frame { set; get; }
        public string Attribute { set; get; }

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
            int compareResult = Source.CompareTo(other.Source);
            if (compareResult != 0) return compareResult;
            compareResult = Feature.CompareTo(other.Feature);
            if (compareResult != 0) return compareResult;
            compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            compareResult = Right.CompareTo(other.Right);
            if (compareResult != 0) return compareResult;
            compareResult = Score.CompareTo(other.Score);
            if (compareResult != 0) return compareResult;
            compareResult = Frame.CompareTo(other.Frame);
            if (compareResult != 0) return compareResult;
            return Attribute.CompareTo(other.Attribute);
        }
    }
}
