// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Intervals.Parsers.Model
{
    public sealed class BedStats : IntervalStats
    {
        private double _sumPValue;
        private double _sumSqrdPValue;

        public double PValueHighest { private set; get; }
        public double PValueLowest { private set; get; }
        public double PValueMean { private set; get; }

        /// <summary>
        /// Gets the population standard deviation of p-values.
        /// <para />
        /// The value is computed as a new p-value is given 
        /// (aka, streaming). Due to overflow/rounding at each
        /// step, there could be an epsilon difference between 
        /// this value, and a population standard deviation 
        /// computed by averaging all the data first and then
        /// subtracting average from each p-value.
        /// </summary>
        public double PValuePSTDV { private set; get; }

        public BedStats() : base()
        {
            PValueLowest = 1;
        }

        public override void Update(IInterval<int> interval)
        {
            base.Update(interval);

            // Can use `as` for safe casting as the following:
            /// IPeak peak = interval as IPeak;
            // however, casting errors must not occur here unless
            // there is an issue initializing these classes.
            // Therefore, an "unsafe" casting might help  
            // spotting such issues with initializations.
            IPeak peak = (IPeak)interval;

            if (!double.IsNaN(peak.Value))
            {
                PValueHighest = Math.Max(PValueHighest, peak.Value);
                PValueLowest = Math.Min(PValueLowest, peak.Value);
                _sumPValue += peak.Value;
                PValueMean = _sumPValue / Count;
                _sumSqrdPValue += Math.Pow(peak.Value, 2);
                PValuePSTDV = Math.Sqrt((_sumSqrdPValue / Count) - Math.Pow(_sumPValue / Count, 2));
            }
        }
    }
}
