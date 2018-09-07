﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.IntervalParsers.Model.Defaults
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

        public string ID { set; get; }
        public Base[] RefBase { set; get; }
        public Base[] AltBase { set; get; }
        public double Quality { set; get; }
        public string Filter { set; get; }
        public string Info { set; get; }

        public new int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Variant)
                return CompareTo(obj as Variant);
            else
                throw new NotImplementedException("Comparison with other object types is not implemented.");
        }

        public int CompareTo(IVariant other)
        {
            if (other == null) return 1;
            int compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            compareResult = ID.CompareTo(other.ID);
            if (compareResult != 0) return compareResult;
            compareResult = Quality.CompareTo(other.Quality);
            if (compareResult != 0) return compareResult;
            compareResult = Filter.CompareTo(other.Filter);
            if (compareResult != 0) return compareResult;
            compareResult = Info.CompareTo(other.Info);
            if (compareResult != 0) return compareResult;
            compareResult = (string.Join("", RefBase)).CompareTo(string.Join("", other.RefBase));
            if (compareResult != 0) return compareResult;
            return (string.Join("", AltBase)).CompareTo(string.Join("", other.AltBase));
        }
    }
}
