// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.Parsers
{
    public class ParsedChIPseqPeaks<C, I, M> : ParsedIntervals<C, I, M>
        where I : IInterval<C, M>, new()
        where M : IExtendedMetadata, new()
    {   
        public ParsedChIPseqPeaks()
        {
            pValueMax = new I() { Left = default(C), Right = default(C), Metadata = new M() };
            pValueMin = new I() { Left = default(C), Right = default(C), Metadata = new M() };
        }

        public I pValueMax;
        public I pValueMin;
        public double pValueMean;
    }
}
