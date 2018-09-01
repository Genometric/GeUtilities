// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

namespace Genometric.GeUtilities.IGenomics
{
    public interface IInterval<C> : IComparable
    {
        /// <summary>
        /// Sets and gets the left-end of the interval.
        /// </summary>
        C Left { set; get; }

        /// <summary>
        /// Sets and gets the right-end of the interval.
        /// </summary>
        C Right { set; get; }

        /// <summary>
        /// Gets the hashKey of the interval. 
        /// 
        /// <para>
        /// The key must satisfy the following conditions:
        /// (1) Any two different intervals, 
        /// have different hash keys;
        /// 
        /// (2) Two equal hash keys imply the equality 
        /// of the intervals.
        /// </para>
        /// </summary>
        int GetHashCode();
    }
}
