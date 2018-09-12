// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Intervals.Functions;
using System;

namespace Genometric.GeUtilities.Intervals.Model
{
    public class Interval : IInterval<int>
    {
        public Interval(int left, int right, string hashSeed = "")
        {
            Left = left;
            Right = right;

            unchecked
            {
                _hashKey = (int)HashFunctions.FNVHashFunction(left.ToString() + right.ToString() + hashSeed);
            }
        }

        public int Left { get; }
        public int Right { get; }


        private readonly int _hashKey;
        public override int GetHashCode()
        {
            return _hashKey;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;
            Interval other = (Interval)obj;
            return Left == other.Left && Right == other.Right;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException("Comparison with other object types is not implemented.");
        }
    }
}
