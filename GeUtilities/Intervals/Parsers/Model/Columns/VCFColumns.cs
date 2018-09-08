// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.Intervals.Parsers.Model
{
    public class VCFColumns : BaseColumns
    {
        public byte ID { set; get; }
        public byte RefBase { set; get; }
        public byte AltBase { set; get; }
        public byte Quality { set; get; }
        public byte Filter { set; get; }
        public byte Info { set; get; }

        public VCFColumns()
        {
            Chr = 0;
            Left = 1;
            Right = -1;
            ID = 2;
            RefBase = 3;
            AltBase = 4;
            Quality = 5;
            Filter = 6;
            Info = 7;
            Strand = -1;
        }
    }
}
