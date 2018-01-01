// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.Parsers
{
    public enum HashFunction
    {
        /// <summary>
        /// Calculates a hash key based on One-at-a-Time method
        /// generated based on Dr. Dobb's left methods.
        /// </summary>
        One_at_a_Time,

        /// <summary>
        /// Calculates a hash key based on FNV hash by 
        /// Fowler / Noll / Vo.
        /// </summary>
        FNV
    };
}
