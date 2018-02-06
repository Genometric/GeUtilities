// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;

namespace Genometric.GeUtilities.Parsers
{
    public class BEDParser : BEDParser<ChIPSeqPeak>
    {
        public BEDParser(
            string sourceFilePath,
            bool dropPeakIfInvalidValue = true) :
            this(
                sourceFilePath: sourceFilePath,
                chrColumn: 0,
                leftEndColumn: 1,
                rightEndColumn: 2,
                nameColumn: 3,
                valueColumn: 4,
                strandColumn: -1,
                summitColumn: -1)
        { }

        public BEDParser(
            string sourceFilePath,
            byte chrColumn,
            byte leftEndColumn,
            sbyte rightEndColumn,
            byte nameColumn,
            byte valueColumn,
            sbyte strandColumn = -1,
            sbyte summitColumn = -1) :
            base(
                sourceFilePath: sourceFilePath,
                chrColumn: chrColumn,
                leftEndColumn: leftEndColumn,
                rightEndColumn: rightEndColumn,
                nameColumn: nameColumn,
                valueColumn: valueColumn,
                strandColumn: strandColumn,
                summitColumn: summitColumn)
        { }
    }
}
