// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Genometric.GeUtilities.IntervalParsers
{
    public class Strand<I>
        where I : IInterval<int>, new()
    {
        private readonly List<I> _intervals;
        public ReadOnlyCollection<I> Intervals
        {
            get { return _intervals.AsReadOnly(); }
        }

        public Strand()
        {
            _intervals = new List<I>();
        }

        public void Add(I interval)
        {
            _intervals.Add(interval);
        }
    }
}
