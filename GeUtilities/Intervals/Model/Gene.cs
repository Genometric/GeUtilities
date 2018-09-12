// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Intervals.Model
{
    public class Gene : Interval, IRefSeq
    {
        public Gene(int left, int right, string refSeqID, string geneSymbol, string hashSeed = "") :
            base(left, right, refSeqID + geneSymbol + hashSeed)
        {
            RefSeqID = refSeqID;
            GeneSymbol = geneSymbol;
        }

        public string RefSeqID { get; }
        public string GeneSymbol { get; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;
            if (!base.Equals(obj)) return false;
            var other = (Gene)obj;
            return
                RefSeqID == other.RefSeqID &&
                GeneSymbol == other.GeneSymbol;
        }

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
            if (RefSeqID == null) return -1;
            if (other.RefSeqID == null) return 1;
            compareResult = RefSeqID.CompareTo(other.RefSeqID);
            if (compareResult != 0) return compareResult;
            if (GeneSymbol == null) return -1;
            if (other.GeneSymbol == null) return 1;
            return GeneSymbol.CompareTo(other.GeneSymbol);
        }
    }
}
