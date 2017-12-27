// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

namespace Genometric.GeUtilities.IGenomics
{
    public interface IVCF : IInterval<int>, IComparable<IVCF>
    {
        string ID { set; get; }
        BasePair[] RefBase { set; get; }
        BasePair[] AltBase { set; get; }

        /// <summary>
        /// Phred-scaled quality score for the assertion made in altered-base.
        /// </summary>
        double Quality { set; get; }

        string Filter { set; get; }

        string Info { set; get; }
        double Value { set; get; }
    }
}
