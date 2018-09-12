﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Intervals.Model
{
    public class ChIPSeqPeak : Interval, IChIPSeqPeak
    {
        public ChIPSeqPeak(int left, int right, double value, int summit, string name, string hashSeed = "") :
            base(left, right, value.ToString() + summit.ToString() + name + hashSeed)
        {
            Value = value;
            Summit = summit;
            Name = name;
        }

        public double Value { get; }
        public int Summit { get; }
        public string Name { get; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public new int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return 1;
            return CompareTo((ChIPSeqPeak)obj);
        }

        public int CompareTo(IChIPSeqPeak other)
        {
            if (other == null) return 1;
            int compareResult = Left.CompareTo(other.Left);
            if (compareResult != 0) return compareResult;
            compareResult = Right.CompareTo(other.Right);
            if (compareResult != 0) return compareResult;
            compareResult = Value.CompareTo(other.Value);
            if (compareResult != 0) return compareResult;
            compareResult = Summit.CompareTo(other.Summit);
            if (compareResult != 0) return compareResult;
            if (Name == null) return -1;
            if (other.Name == null) return 1;
            return Name.CompareTo(other.Name);
        }
    }
}
