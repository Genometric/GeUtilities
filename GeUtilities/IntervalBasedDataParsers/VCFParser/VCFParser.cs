// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.ReferenceGenomes;

namespace Genometric.GeUtilities.Parsers
{
    public class VCFParser : VCFParser<Variant>
    {
        public VCFParser(
            string sourceFilePath,
            Assemblies assembly = Assemblies.Unknown,
            bool readOnlyValidChrs = true,
            byte startOffset = 0,
            uint maxLinesToRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            this(sourceFilePath: sourceFilePath,
                assembly: assembly,
                chrColumn: 0,
                positionColumn: 1,
                idColumn: 2,
                refbColumn: 3,
                altbColumn: 4,
                qualityColumn: 5,
                filterColumn: 6,
                infoColumn: 7,
                strandColumn: -1,
                startOffset: startOffset,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToRead: maxLinesToRead,
                hashFunction: hashFunction)
        { }

        public VCFParser(
            string sourceFilePath,
            byte chrColumn,
            byte positionColumn,
            byte idColumn,
            byte refbColumn,
            byte altbColumn,
            byte qualityColumn,
            byte filterColumn,
            byte infoColumn,
            sbyte strandColumn,
            Assemblies assembly = Assemblies.Unknown,
            bool readOnlyValidChrs = true,
            byte startOffset = 0,
            uint maxLinesToRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            base(
                sourceFilePath: sourceFilePath,
                chrColumn: chrColumn,
                positionColumn: positionColumn,
                idColumn: idColumn,
                refbColumn: refbColumn,
                altbColumn: altbColumn,
                qualityColumn: qualityColumn,
                filterColumn: filterColumn,
                infoColumn: infoColumn,
                strandColumn: strandColumn,
                assembly: assembly,
                startOffset: startOffset,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToRead: maxLinesToRead,
                hashFunction: hashFunction)
        { }
    }
}
