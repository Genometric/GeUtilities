// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

namespace Genometric.GeUtilities.Parsers
{
    public class DeterminedFeature
    {
        public DeterminedFeature(string title, int count, byte code)
        {
            Title = title;
            Count = count;
            Code = code;
        }

        /// <summary>
        /// Gets the feature title.
        /// </summary>
        public string Title { private set; get; }
        /// <summary>
        /// Gets the number of determined intervals of the feature.
        /// </summary>
        public int Count { internal set; get; }
        /// <summary>
        /// Gets the conversion code of the feature.
        /// </summary>
        public byte Code { private set; get; }
    }
}
