﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.IntervalParsers
{
    public class BEDColumns : BaseColumns
    {
        public byte Name { set; get; }
        public byte Value { set; get; }
        public sbyte Summit { set; get; }

        public BEDColumns() : base()
        {
            Name = 3;
            Value = 4;
            Summit = -1;
        }
    }
}
