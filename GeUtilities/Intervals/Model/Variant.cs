// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.Intervals.Model
{
    public class Variant : Interval, IVariant
    {
        public Variant(int left, int right, string id, Base[] refBase, Base[] altBase, double quality,
            string filter, string info, string hashSeed = "") :
            base(left, right, id + (refBase == null ? "" : refBase.ToString())
                + (altBase == null ? "" : altBase.ToString()) + quality.ToString() + filter + info + hashSeed)
        {
            ID = id;
            RefBase = refBase;
            AltBase = altBase;
            Quality = quality;
            Filter = filter;
            Info = info;
        }

        public string ID { get; }
        public Base[] RefBase { get; }
        public Base[] AltBase { get; }
        public double Quality { get; }
        public string Filter { get; }
        public string Info { get; }

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
            return CompareTo((Variant)obj);
        }

        public int CompareTo(IVariant other)
        {
            if (other == null) return 1;
            int compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            if (ID == null) return -1;
            if (other.ID == null) return 1;
            compareResult = ID.CompareTo(other.ID);
            if (compareResult != 0) return compareResult;
            compareResult = Quality.CompareTo(other.Quality);
            if (compareResult != 0) return compareResult;
            if (Filter == null) return -1;
            if (other.Filter == null) return 1;
            compareResult = Filter.CompareTo(other.Filter);
            if (compareResult != 0) return compareResult;
            if (Info == null) return -1;
            if (other.Info == null) return 1;
            compareResult = Info.CompareTo(other.Info);
            if (compareResult != 0) return compareResult;
            if (RefBase == null) return -1;
            if (other.RefBase == null) return 1;
            compareResult = (string.Join("", RefBase)).CompareTo(string.Join("", other.RefBase));
            if (compareResult != 0) return compareResult;
            if (AltBase == null) return -1;
            if (other.AltBase == null) return 1;
            return (string.Join("", AltBase)).CompareTo(string.Join("", other.AltBase));
        }
    }
}
