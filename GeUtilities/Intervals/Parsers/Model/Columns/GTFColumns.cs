// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.Intervals.Parsers.Model
{
    public class GTFColumns : BaseColumns
    {
        public sbyte Source { set; get; }
        public sbyte Feature { set; get; }
        public sbyte Score { set; get; }
        public sbyte Frame { set; get; }
        public sbyte Attribute { set; get; }

        public GTFColumns()
        {
            Chr = 0;
            Source = 1;
            Feature = 2;
            Left = 3;
            Right = 4;
            Score = 5;
            Strand = 6;
            Frame = 7;
            Attribute = 8;
        }
    }
}
