// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Intervals.Functions;

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
                _hashKey = (int)HashFunctions.FNVHashFunction(HashFunctions.GetHashSeed(left.ToString(), right.ToString(), hashSeed));
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
            return CompareTo(obj) == 0;
        }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return 1;
            return CompareTo((Interval)obj);
        }

        public int CompareTo(IInterval<int> other)
        {
            int compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            return Right.CompareTo(other.Right);
        }
    }
}
