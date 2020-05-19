// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Intervals.Functions;

namespace Genometric.GeUtilities.Intervals.Model
{
    public class GeneralFeature : Interval, IGeneralFeature
    {
        public GeneralFeature(int left, int right, string source, string feature, double score,
            string frame, string attribute, string hashSeed = "") :
            base(left, right, HashFunctions.GetHashSeed(source, feature, score.ToString(), frame, attribute, hashSeed))
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public new int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return 1;
            return CompareTo((GeneralFeature)obj);
        }

        public int CompareTo(IGeneralFeature other)
        {
            if (Frame == null ||
                Source == null ||
                Feature == null ||
                Attribute == null)
                return -1;

            if (other == null ||
                other.Frame == null ||
                other.Source == null ||
                other.Feature == null ||
                other.Attribute == null)
                return 1;

            int compareResult = base.CompareTo(other);
            if (compareResult != 0) return compareResult;
            compareResult = Source.CompareTo(other.Source);
            if (compareResult != 0) return compareResult;
            compareResult = Feature.CompareTo(other.Feature);
            if (compareResult != 0) return compareResult;
            compareResult = Score.CompareTo(other.Score);
            if (compareResult != 0) return compareResult;
            compareResult = Frame.CompareTo(other.Frame);
            if (compareResult != 0) return compareResult;
            return Attribute.CompareTo(other.Attribute);
        }
    }
}
