// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Genometric.GeUtilities.ReferenceGenomes
{
    internal static class Hg19
    {
        private static readonly Dictionary<string, int> _data = new Dictionary<string, int>
            {
                { "chr1", 249250621 },
                { "chr2", 243199373 },
                { "chr3", 198022430 },
                { "chr4", 191154276 },
                { "chr5", 180915260 },
                { "chr6", 171115067 },
                { "chr7", 159138663 },
                { "chr8", 146364022 },
                { "chr9", 141213431 },
                { "chr10", 135534747 },
                { "chr11", 135006516 },
                { "chr12", 133851895 },
                { "chr13", 115169878 },
                { "chr14", 107349540 },
                { "chr15", 102531392 },
                { "chr16", 90354753 },
                { "chr17", 81195210 },
                { "chr18", 78077248 },
                { "chr19", 59128983 },
                { "chr20", 63025520 },
                { "chr21", 48129895 },
                { "chr22", 51304566 },
                { "chrX", 155270560 },
                { "chrY", 59373566 },
                { "chrM", 16569 }
            };

        internal static ReadOnlyDictionary<string, int> Data
        {
            get { return new ReadOnlyDictionary<string, int>(_data); }
        }
    }
}
