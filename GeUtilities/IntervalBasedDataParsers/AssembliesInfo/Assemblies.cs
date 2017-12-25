// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Genometric.GeUtilities.Parsers
{
    public static class GenomeAssemblies
    {
        internal static ReadOnlyDictionary<string, int> Assembly(Assemblies assembly)
        {
            switch (assembly)
            {
                case Assemblies.hg19: return hg19.Data();
                case Assemblies.mm10: return mm10.Data();
                case Assemblies.Unknown:
                default:
                    return new Dictionary<string, int>();
            }
        }
        public static Dictionary<Genomes, GenomeInfo> AllGenomeAssemblies()
        {
            Dictionary<Genomes, GenomeInfo> rtv = new Dictionary<Genomes, GenomeInfo>();

            GenomeInfo hs = new GenomeInfo
            {
                GenomeTitle = "Homo Sapiens",
                GenomeAssemblies = new Dictionary<Assemblies, string>()
            };
            hs.GenomeAssemblies.Add(Assemblies.hg19, "hg19 (GENECODE 19)");
            rtv.Add(Genomes.HomoSapiens, hs);


            GenomeInfo mm = new GenomeInfo
            {
                GenomeTitle = "Mus musculus",
                GenomeAssemblies = new Dictionary<Assemblies, string>()
            };
            mm.GenomeAssemblies.Add(Assemblies.mm10, "mm10 (GENCODE M2)");
            rtv.Add(Genomes.MusMusculus, mm);

            return rtv;
        }

        public class GenomeInfo
        {
            public string GenomeTitle { internal set; get; }
            public Dictionary<Assemblies, string> GenomeAssemblies { internal set; get; }
        }
    }
}
