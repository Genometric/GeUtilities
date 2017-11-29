// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.Parsers
{
    public sealed class ChrStatistics
    {
        public string chrTitle;
        public int count;
        public string percentage;

        public uint peakWidthMax;
        public uint peakWidthMin;
        public double peakWidthMean;
        public double peakWidthSTDV;

        public double pValueMax;
        public double pValueMin;
        public double pValueMean;
        public double pValueSTDV;

        public float coverage;
    }
}
