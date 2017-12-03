using Genometric.GeUtilities.IGenomics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Genometric.GeUtilities.Parsers
{
    public class Strand<I>
        where I : IInterval<int>, new()
    {
        public List<I> intervals;

        public Strand()
        {
            intervals = new List<I>();
        }

        public void Add(I interval)
        {
            intervals.Add(interval);
        }
    }
}
