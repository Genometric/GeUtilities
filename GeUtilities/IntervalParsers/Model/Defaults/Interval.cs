// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.IntervalParsers.Model.Defaults
{
    public class Interval : IInterval<int>
    {
        public Interval(int left, int right, string hashSeed)
        {
            Left = left;
            Right = right;

            unchecked
            {
                _hashKey = (int)HashFuncs.FNVHashFunction(left.ToString() + right.ToString() + hashSeed);
            }
        }

        public int Left { private set; get; }
        public int Right { private set; get; }

        private readonly int _hashKey;
        public override int GetHashCode()
        {
            return _hashKey;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Interval)) return false;
            return _hashKey == obj.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException("Comparison with other object types is not implemented.");
        }
    }
}
