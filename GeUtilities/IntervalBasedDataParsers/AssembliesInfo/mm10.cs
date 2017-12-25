// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Genometric.GeUtilities.Parsers
{
    internal static class mm10
    {
        private static Dictionary<string, int> _data = new Dictionary<string, int>
            {
                { "chr1", 195471971 },
                { "chr2", 182113224 },
                { "chr3", 160039680 },
                { "chr4", 156508116 },
                { "chr5", 151834684 },
                { "chr6", 149736546 },
                { "chr7", 145441459 },
                { "chr8", 129401213 },
                { "chr9", 124595110 },
                { "chr10", 130694993 },
                { "chr11", 122082543 },
                { "chr12", 120129022 },
                { "chr13", 120421639 },
                { "chr14", 124902244 },
                { "chr15", 104043685 },
                { "chr16", 98207768 },
                { "chr17", 94987271 },
                { "chr18", 90702639 },
                { "chr19", 90702639 },
                { "chr20", 61431566 },
                { "chrX", 171031299 },
                { "chrY", 91744698 },
                { "chrM", 16299 }
            };

        internal static ReadOnlyDictionary<string, int> Data
        {
            get { return new ReadOnlyDictionary<string, int>(_data); }
        }
    }
}
