// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.ReferenceGenomes;

namespace Genometric.GeUtilities.Parsers
{
    public class RefSeqParser : RefSeqParser<Gene>
    {
        public RefSeqParser(
            string sourceFilePath,
            Assemblies assembly = Assemblies.Unknown,
            bool readOnlyValidChrs = true,
            byte startOffset = 0,
            uint maxLinesToBeRead = uint.MaxValue) :
            this(sourceFilePath: sourceFilePath,
                assembly: assembly,
                readOnlyValidChrs: readOnlyValidChrs,
                startOffset: startOffset,
                chrColumn: 0,
                leftEndColumn: 1,
                rightEndColumn: 2,
                refSeqIDColumn: 3,
                geneSymbolColumn: 4,
                strandColumn: -1,
                maxLinesToRead: maxLinesToBeRead,
                hashFunction: HashFunction.One_at_a_Time
                )
        { }

        public RefSeqParser(
            string sourceFilePath,
            byte chrColumn,
            byte leftEndColumn,
            sbyte rightEndColumn,
            byte refSeqIDColumn,
            byte geneSymbolColumn,
            sbyte strandColumn = -1,
            Assemblies assembly = Assemblies.Unknown,
            byte startOffset = 0,
            bool readOnlyValidChrs = true,
            uint maxLinesToRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            base(
                sourceFilePath: sourceFilePath,
                chrColumn: chrColumn,
                leftEndColumn: leftEndColumn,
                rightEndColumn: rightEndColumn,
                refSeqIDColumn: refSeqIDColumn,
                geneSymbolColumn: geneSymbolColumn,
                strandColumn: strandColumn,
                assembly: assembly,
                startOffset: startOffset,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToRead: maxLinesToRead,
                hashFunction: hashFunction)
        { }
    }
}
