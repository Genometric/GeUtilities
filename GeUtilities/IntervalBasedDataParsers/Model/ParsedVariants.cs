﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.Parsers
{
    public class ParsedVariants<I> : ParsedIntervals<I, IntervalStats>
        where I : IVariant, new()
    {
        public ParsedVariants()
        { }
    }
}
