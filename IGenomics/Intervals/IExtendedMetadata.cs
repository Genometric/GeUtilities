// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.IGenomics
{
    public interface IExtendedMetadata : IMetaData
    {
        /// <summary>
        /// Sets and gets value.
        /// </summary>
        double Value { set; get; }
    }
}
