// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Genometric.GeUtilities.Parsers
{
    public class Strand<I>
        where I : IInterval<int>, new()
    {
        public List<I> intervals;

        public Strand()
        {
            intervals = new List<I>();
        }

        public void Add(I interval)
        {
            intervals.Add(interval);
        }
    }
}
