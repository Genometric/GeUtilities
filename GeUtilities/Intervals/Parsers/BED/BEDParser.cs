// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using Genometric.GeUtilities.Intervals.Parsers.Model;

namespace Genometric.GeUtilities.Intervals.Parsers
{
    public class BedParser : BedParser<Peak>
    {
        public BedParser() : this(new BedColumns())
        { }

        public BedParser(BedColumns columns) : base(columns, new ChIPSeqPeakConstructor())
        { }
    }
}
