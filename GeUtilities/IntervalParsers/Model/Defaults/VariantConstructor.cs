// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.IntervalParsers.Model.Defaults
{
    public class VariantConstructor : IVariantConstructor<Variant>
    {
        public Variant Construct(int left, int right, string id, Base[] refBase, Base[] altBase, double quality, string filter, string info, string hashSeed = "")
        {
            return new Variant(left, left + 1, id, refBase, altBase, quality, filter, info, hashSeed);
        }
    }
}
