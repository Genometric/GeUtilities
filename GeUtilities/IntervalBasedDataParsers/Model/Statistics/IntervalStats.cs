// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Genometric.GeUtilities.Parsers
{
    public class IntervalStats : IStats<int>
    {
        private byte _decimalPlaces = 6;
        private uint _widthSum;
        private double _withSTDVTemp;
        public int Count { private set; get; }
        public uint WidthMax { private set; get; }
        public uint WidthMin { private set; get; }
        public double WidthMean { private set; get; }
        public double WidthSTDV { private set; get; }
        public float Coverage { private set; get; }

        public IntervalStats()
        {
            WidthMin = uint.MaxValue;
        }

        public void Update(IInterval<int> interval)
        {
            Count++;
            uint intervalWidth = (uint)(interval.Right - interval.Left);
            _widthSum += intervalWidth;
            WidthMax = Math.Max(WidthMax, intervalWidth);
            WidthMin = Math.Min(WidthMin, intervalWidth);
            WidthMean = Math.Round(_widthSum / (double)Count, _decimalPlaces);
            _withSTDVTemp += Math.Pow(intervalWidth - WidthMean, 2.0);
            WidthSTDV = Math.Sqrt(_withSTDVTemp / Count);
        }
    }
}
