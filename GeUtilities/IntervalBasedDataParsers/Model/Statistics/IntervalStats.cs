// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Parsers
{
    public class IntervalStats : IStats<int>
    {
        private uint _sumWidth;
        private double _sumSqrdWidth;

        public int Count { private set; get; }
        public uint WidthMax { private set; get; }
        public uint WidthMin { private set; get; }
        public double WidthMean { private set; get; }
        public double WidthPSTDV { private set; get; }
        public float Coverage { private set; get; }

        public IntervalStats()
        {
            WidthMin = uint.MaxValue;
        }

        public void Update(IInterval<int> interval)
        {
            Count++;
            uint intervalWidth = (uint)(interval.Right - interval.Left);
            WidthMax = Math.Max(WidthMax, intervalWidth);
            WidthMin = Math.Min(WidthMin, intervalWidth);
            _sumWidth += intervalWidth;
            WidthMean = _sumWidth / (double)Count;
            _sumSqrdWidth += Math.Pow(intervalWidth, 2);
            WidthPSTDV = Math.Sqrt((_sumSqrdWidth / Count) - Math.Pow(_sumWidth / (double)Count, 2));
        }
    }
}
