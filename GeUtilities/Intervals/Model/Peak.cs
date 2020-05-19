// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Intervals.Functions;

namespace Genometric.GeUtilities.Intervals.Model
{
    public class Peak : Interval, IPeak
    {
        public Peak(int left, int right, double value, string name = null, int summit = -1, string hashSeed = "") :
            base(left, right, HashFunctions.GetHashSeed(value.ToString(), summit.ToString(), name, hashSeed))
        {
            Value = value;
            Summit = summit != -1 ? summit : (right - left) / 2;
            Name = name;
        }

        public double Value { get; }
        public int Summit { get; }
        public string Name { get; }

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
            return CompareTo((Peak)obj);
        }

        public int CompareTo(IPeak other)
        {
            if (other == null) return 1;
            int compareResult = base.CompareTo(other);
            if (compareResult != 0) return compareResult;
            compareResult = Value.CompareTo(other.Value);
            if (compareResult != 0) return compareResult;
            compareResult = Summit.CompareTo(other.Summit);
            if (compareResult != 0) return compareResult;
            if (Name == null && other.Name == null) return 0;
            if (Name == null) return -1;
            if (other.Name == null) return 1;
            return Name.CompareTo(other.Name);
        }
    }
}
