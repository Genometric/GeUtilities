// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Genometric.GeUtilities.ReferenceGenomes
{
    public static class References
    {
        public static ReadOnlyDictionary<string, int> GetGenomeSizes(Assemblies assemblies)
        {
            switch (assemblies)
            {
                case Assemblies.hg19: return hg19.Data;
                case Assemblies.mm10: return mm10.Data;
                case Assemblies.Unknown:
                default:
                    return new ReadOnlyDictionary<string, int>(new Dictionary<string, int>());
            }
        }
    }
}
