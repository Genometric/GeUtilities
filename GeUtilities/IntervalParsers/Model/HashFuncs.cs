// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.IntervalParsers
{
    public static class HashFuncs<I>
        where I : IInterval<int>
    {
        private const uint _FNVPrime_32 = 16777619;
        private const uint _FNVOffsetBasis_32 = 2166136261;

        /// <summary>
        /// Returns hash key based on One-at-a-Time method
        /// generated based on Dr. Dobb's left methods.
        /// </summary>
        /// <returns>Hash key of the interval.</returns>
        public static uint OneAtATimeHashFunction(uint fileHashKey, I readingPeak, uint lineNo)
        {
            string key = fileHashKey + "_" + readingPeak.Left.ToString() + "_" + readingPeak.Right.ToString() + "_" + lineNo.ToString();
            int l = key.Length;

            uint hashKey = 0;
            for (int i = 0; i < l; i++)
            {
                hashKey += key[i];
                hashKey += (hashKey << 10);
                hashKey ^= (hashKey >> 6);
            }

            hashKey += (hashKey << 3);
            hashKey ^= (hashKey >> 11);
            hashKey += (hashKey << 15);

            return hashKey;
        }
        public static uint FNVHashFunction(uint fileHashKey, I readingPeak, uint lineNo)
        {
            string key = fileHashKey + "_" + readingPeak.Left.ToString() + "_" + readingPeak.Right.ToString() + "_" + lineNo.ToString();
            uint hash = _FNVOffsetBasis_32;
            for (var i = 0; i < key.Length; i++)
            {
                hash = hash ^ key[i]; // exclusive OR
                hash *= _FNVPrime_32;
            }

            return hash;
        }
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
    }
}
