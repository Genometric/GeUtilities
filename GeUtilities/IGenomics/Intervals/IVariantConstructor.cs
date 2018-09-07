// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.IGenomics
{
    public interface IVariantConstructor<out I>
        where I : IVariant
    {
        I Construct(int left, int right, string id, Base[] refBase, Base[] altBase, double quality,
            string filter, string info, string hashSeed = "");
    }
}
