// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.ReferenceGenomes;
using System;
using System.Collections.Generic;

namespace Genometric.GeUtilities.Parsers
{
    public abstract class ParsedIntervals<I, S>
        where I : IInterval<int>, new()
        where S : IStats<int>, new()
    {
        public string FileName { set; get; }
        public string FilePath { set; get; }
        public UInt32 FileHashKey { set; get; }
        public int IntervalsCount { set; get; }
        public List<string> Messages { set; get; }
        public Dictionary<string, Chromosome<I, S>> Chromosomes { set; get; }
        public Assemblies Assembly { set; get; }
        public S Statistics { set; get; }

        public ParsedIntervals()
        {
            Messages = new List<string>();
            Chromosomes = new Dictionary<string, Chromosome<I, S>>();
            Statistics = new S();
        }

        public void Add(I interval, string chr, char strand)
        {
            if (!Chromosomes.ContainsKey(chr))
                Chromosomes.Add(chr, new Chromosome<I, S>());
            Chromosomes[chr].Add(interval, strand);
            Statistics.Update(interval);
        }
    }
}
