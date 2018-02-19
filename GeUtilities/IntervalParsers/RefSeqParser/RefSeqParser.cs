// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers.Model.Defaults;

namespace Genometric.GeUtilities.IntervalParsers
{
    public class RefSeqParser : RefSeqParser<Gene>
    {
        public RefSeqParser(string sourceFilePath) :
            this(sourceFilePath, new RefSeqColumns())
        { }

        public RefSeqParser(string sourceFilePath, RefSeqColumns columns) :
            base(sourceFilePath, columns)
        { }
    }
}
