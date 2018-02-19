// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.IntervalParsers
{
    public class RefSeqColumns : BaseColumns
    {
        public byte RefSeqID { set; get; }
        public byte GeneSeymbol { set; get; }

        public RefSeqColumns()
        {
            Chr = 0;
            Left = 1;
            Right = 2;
            RefSeqID = 3;
            GeneSeymbol = 4;
            Strand = -1;
        }
    }
}
