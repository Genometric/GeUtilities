// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System.Collections.Generic;

namespace Genometric.GeUtilities.IntervalParsers
{
    public class Chromosome<I, S>
        where I : IInterval<int>
        where S : IStats<int>
    {
        public S Statistics { set; get; }
        public Dictionary<char, Strand<I>> Strands { set; get; }

        public Chromosome()
        {
            Statistics = new S();
            Strands = new Dictionary<char, Strand<I>>();
        }

        public void Add(I interval, char strand)
        {
            if (!Strands.ContainsKey(strand))
                Strands.Add(strand, new Strand<I>());
            Strands[strand].Add(interval);
            Statistics.Update(interval);
        }
    }
}
