// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System.Collections.Generic;

namespace Genometric.GeUtilities.Intervals.Parsers.Model
{
    public class Gtf<I> : ParsedIntervals<I, IntervalStats>
        where I : IGeneralFeature
    {
        public Dictionary<string, int> DeterminedFeatures { set; get; }
    }
}
