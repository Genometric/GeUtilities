// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults
{
    public class Gene : IGene
    {
        public int Left { set; get; }
        public int Right { set; get; }
        public double Value { set; get; }
        public string RefSeqID { set; get; }
        public string GeneSymbol { set; get; }
        public uint HashKey { set; get; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Gene)
                return CompareTo(obj as Gene);
            else
                throw new NotImplementedException();
        }

        public int CompareTo(IGene other)
        {
            if (other == null) return 1;
            int compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            compareResult = Right.CompareTo(other.Right);
            if (compareResult != 0) return compareResult;
            compareResult = Value.CompareTo(other.Value);
            if (compareResult != 0) return compareResult;
            compareResult = RefSeqID.CompareTo(other.RefSeqID);
            if (compareResult != 0) return compareResult;
            return GeneSymbol.CompareTo(other.GeneSymbol);
        }
    }
}
