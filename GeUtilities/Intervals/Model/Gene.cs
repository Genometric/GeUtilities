// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.Intervals.Model
{
    public class Gene : Interval, IRefSeqGene
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
            return CompareTo(obj) == 0;
        }

        public new int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return 1;
            return CompareTo((Gene)obj);
        }

        public int CompareTo(IRefSeqGene other)
        {
            if (RefSeqID == null ||
                GeneSymbol == null)
                return -1;

            if (other == null ||
                other.RefSeqID == null ||
                other.GeneSymbol == null)
                return 1;

            int compareResult = base.CompareTo(other);
            if (compareResult != 0) return compareResult;
            compareResult = RefSeqID.CompareTo(other.RefSeqID);
            if (compareResult != 0) return compareResult;
            return GeneSymbol.CompareTo(other.GeneSymbol);
        }
    }
}
