// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class BEDStats : IntervalStats
    {
        private double _sumPValue;
        private double _sumSqrdPValue;

        public double PValueHighest { private set; get; }
        public double PValueLowest { private set; get; }
        public double PValueMean { private set; get; }
        public double PValueSTDV { private set; get; }

        public BEDStats() : base()
        {
            PValueLowest = 1;
        }

        public new void Update(IInterval<int> interval)
        {
            base.Update(interval);

            // Can use `as` for safe casting as the following:
            // IChIPSeqPeak peak = interval as IChIPSeqPeak;
            // however, casting errors must not occur here unless
            // there is an issue initializing these classes.
            // Therefore, an "unsafe" casting might help  
            // spotting such issues with initializations.
            IChIPSeqPeak peak = (IChIPSeqPeak)interval;

            if (!double.IsNaN(peak.Value))
            {
                PValueHighest = Math.Max(PValueHighest, peak.Value);
                PValueLowest = Math.Min(PValueLowest, peak.Value);
                _sumPValue += peak.Value;
                PValueMean = _sumPValue / Count;
                _sumSqrdPValue += Math.Pow(peak.Value, 2);
                PValueSTDV = Math.Sqrt((_sumSqrdPValue / Count) - Math.Pow(_sumPValue / Count, 2));
            }
        }
    }
}
