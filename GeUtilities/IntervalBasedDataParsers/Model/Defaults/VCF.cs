// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults
{
    public class VCF : IVCF
    {
        public int Left { set; get; }
        public int Right { set; get; }
        public double Value { set; get; }
        public string ID { set; get; }
        public BasePair[] RefBase { set; get; }
        public BasePair[] AltBase { set; get; }
        public double Quality { set; get; }
        public string Filter { set; get; }
        public string Info { set; get; }
        public uint HashKey { set; get; }
    }
}
