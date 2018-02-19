// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers.Model.Defaults;

namespace Genometric.GeUtilities.IntervalParsers
{
    public class VCFParser : VCFParser<Variant>
    {
        public VCFParser(
            string sourceFilePath) :
            this(sourceFilePath,new VCFColumns())
        { }

        public VCFParser(
            string sourceFilePath, VCFColumns columns) :
            base(sourceFilePath, columns)
        { }
    }
}
