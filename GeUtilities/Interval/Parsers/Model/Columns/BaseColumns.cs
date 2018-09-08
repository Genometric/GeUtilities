// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.Interval.Parsers.Model
{
    public abstract class BaseColumns
    {
        /// <summary>
        /// Sets and gets the column number of a region's chromosome name.
        /// </summary>
        public byte Chr { set; get; }

        /// <summary>
        /// Sets and gets the column number of a region's left position.
        /// </summary>
        public byte Left { set; get; }

        /// <summary>
        /// Sets and gets the column number of a region's right position.
        /// </summary>
        public sbyte Right { set; get; }

        /// <summary>
        /// Sets and gets the column number of a regions' strand.
        /// </summary>
        public sbyte Strand { set; get; }

        protected BaseColumns()
        {
            Chr = 0;
            Left = 1;
            Right = 2;
            Strand = -1;
        }
    }
}
