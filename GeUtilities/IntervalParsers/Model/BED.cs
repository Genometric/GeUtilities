// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.IntervalParsers
{
    public class BED<I> : ParsedIntervals<I, BEDStats>
        where I : IChIPSeqPeak, new()
    {   
        public BED()
        {
            PValueMax = new I();
            PValueMin = new I();
        }

        public I PValueMax { set; get; }
        public I PValueMin { set; get; }
        public double PValueMean { set; get; }
    }
}
