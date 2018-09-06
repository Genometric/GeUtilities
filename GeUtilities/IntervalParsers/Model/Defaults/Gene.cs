// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.IntervalParsers.Model.Defaults
{
    public class Gene : Interval, IRefSeq
    {
        public Gene(int left, int right, string refSeqID, string geneSymbol, string hashSeed = "") :
            base(left, right, refSeqID + geneSymbol + hashSeed)
        {
            RefSeqID = refSeqID;
            GeneSymbol = geneSymbol;
        }

        public string RefSeqID { set; get; }
        public string GeneSymbol { set; get; }

        public new int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Gene)
                return CompareTo(obj as Gene);
            else
                throw new NotImplementedException("Comparison with other object types is not implemented.");
        }

        public int CompareTo(IRefSeq other)
        {
            if (other == null) return 1;
            int compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            compareResult = Right.CompareTo(other.Right);
            if (compareResult != 0) return compareResult;
            compareResult = RefSeqID.CompareTo(other.RefSeqID);
            if (compareResult != 0) return compareResult;
            return GeneSymbol.CompareTo(other.GeneSymbol);
        }
    }
}
