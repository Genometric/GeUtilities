// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.Parsers
{
    public abstract class ParsedIntervals<C, I, M>
        where I : IInterval<C, M>, new()
        where M : IExtendedMetadata, new()
    {
        public string fileName;
        public string filePath;
        public UInt32 fileHashKey;
        public int intervalsCount;
        public List<string> messages;
        public Dictionary<string, Dictionary<char, List<I>>> intervals;
        public Dictionary<string, ChrStatistics> chrStatistics;
        public Genomes genome;
        public Assemblies assembly;

        public ParsedIntervals()
        {   
            messages = new List<string>();
            intervals = new Dictionary<string, Dictionary<char, List<I>>>();
            chrStatistics = new Dictionary<string, ChrStatistics>();
        }
    }
}
