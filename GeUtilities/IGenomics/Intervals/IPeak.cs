﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

namespace Genometric.GeUtilities.IGenomics
{
    public interface IPeak : IInterval, IComparable<IPeak>
    {
        string Name { get; }
        int Summit { get; }
        double Value { get; }
    }
}
