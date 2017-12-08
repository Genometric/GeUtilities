// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults
{
    public class Gene : IGene
    {
        public int Left { set; get; }
        public int Right { set; get; }
        public double Value { set; get; }
        public char Strand { set; get; }
        public string RefSeqID { set; get; }
        public string GeneSymbol { set; get; }
        public uint HashKey { set; get; }
    }
}
