﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults
{
    public class GeneralFeature : IGeneralFeature
    {
        public int Left { set; get; }
        public int Right { set; get; }
        public double Value { set; get; }
        public byte Feature { set; get; }
        public string Attribute { set; get; }
        public uint HashKey { set; get; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is GeneralFeature)
                return CompareTo(obj as GeneralFeature);
            else
                throw new System.NotImplementedException();
        }

        public int CompareTo(IGeneralFeature other)
        {
            int compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            compareResult = Right.CompareTo(other.Right);
            if (compareResult != 0) return compareResult;
            compareResult = Value.CompareTo(other.Value);
            if (compareResult != 0) return compareResult;
            compareResult = Feature.CompareTo(other.Feature);
            if (compareResult != 0) return compareResult;
            return Attribute.CompareTo(other.Attribute);
        }
    }
}
