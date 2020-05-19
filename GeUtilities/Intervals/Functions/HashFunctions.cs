// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.Intervals.Functions
{
    public static class HashFunctions
    {
        private const uint _FNVPrime_32 = 16777619;
        private const uint _FNVOffsetBasis_32 = 2166136261;

        /// <summary>
        /// Sets and gets a string used as a delimiter separating 
        /// properties used as hash seed.
        /// </summary>
        public const string HashSeedDelimiter = ";;";

        public static uint FNVHashFunction(string bytes)
        {
            uint hash = _FNVOffsetBasis_32;
            for (var i = 0; i < bytes.Length; i++)
            {
                hash = hash ^ bytes[i]; // exclusive OR
                hash *= _FNVPrime_32;
            }

            return hash;
        }

        public static string GetHashSeed(params string[] properties)
        {
            return string.Join(HashSeedDelimiter, properties);
        }
    }
}
