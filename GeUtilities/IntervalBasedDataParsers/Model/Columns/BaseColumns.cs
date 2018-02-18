// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.IntervalParsers
{
    public abstract class BaseColumns
    {
        public byte Chr { set; get; }
        public byte Left { set; get; }
        public sbyte Right { set; get; }
        public sbyte Strand { set; get; }

        protected BaseColumns()
        {
            Strand = -1;
        }
    }
}
