// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class BEDStats : IntervalStats
    {
        private double _pValueSum;
        private double _pValueSTDVTemp;

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
                _pValueSum += peak.Value;
                PValueHighest = Math.Max(PValueHighest, peak.Value);
                PValueLowest = Math.Min(PValueLowest, peak.Value);
                PValueMean = _pValueSum / Count;
                _pValueSTDVTemp += Math.Pow(peak.Value - PValueMean, 2.0);
                PValueSTDV = Math.Sqrt(_pValueSTDVTemp / Count);
            }
        }
    }
}
