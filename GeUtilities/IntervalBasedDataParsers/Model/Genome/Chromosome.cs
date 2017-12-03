using Genometric.GeUtilities.IGenomics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Genometric.GeUtilities.Parsers
{
    public class Chromosome<I, S>
        where I : IInterval<int>, new()
        where S : IStats<int>, new()
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
