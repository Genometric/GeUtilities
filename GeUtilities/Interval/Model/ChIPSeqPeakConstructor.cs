// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.Interval.Model
{
    public class ChIPSeqPeakConstructor : IChIPSeqPeakConstructor<ChIPSeqPeak>
    {
        public ChIPSeqPeak Construct(int left, int right, string name, int summit, double value, string hashSeed = "")
        {
            return new ChIPSeqPeak(left, right, value, summit, name, hashSeed);
        }
    }
}
