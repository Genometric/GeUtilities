// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using Genometric.GeUtilities.ReferenceGenomes;

namespace Genometric.GeUtilities.Parsers
{
    public class GTFParser : GTFParser<GeneralFeature>
    {
        public GTFParser(
           string sourceFilePath,
           Assemblies assembly = Assemblies.Unknown,
           bool readOnlyValidChrs = true,
           byte startOffset = 0,
           uint maxLinesToRead = uint.MaxValue,
           HashFunction hashFunction = HashFunction.One_at_a_Time) :
           this(sourceFilePath: sourceFilePath,
               assembly: assembly,
               readOnlyValidChrs: readOnlyValidChrs,
               startOffset: startOffset,
               chrColumn: 0,
               sourceColumn: 1,
               featureColumn: 2,
               leftEndColumn: 3,
               rightEndColumn: 4,
               scoreColumn: 5,
               strandColumn: 6,
               frameColumn: 7,
               attributeColumn: 8,
               maxLinesToRead: maxLinesToRead,
               hashFunction: hashFunction)
        { }

        public GTFParser(
           string sourceFilePath,
           byte chrColumn,
           sbyte sourceColumn,
           sbyte featureColumn,
           byte leftEndColumn,
           sbyte rightEndColumn,
           sbyte scoreColumn,
           sbyte strandColumn,
           sbyte frameColumn,
           sbyte attributeColumn,
           Assemblies assembly = Assemblies.Unknown,
           bool readOnlyValidChrs = true,
           byte startOffset = 0,
           uint maxLinesToRead = uint.MaxValue,
           HashFunction hashFunction = HashFunction.One_at_a_Time) :
           base(
               sourceFilePath: sourceFilePath,
               chrColumn: chrColumn,
               sourceColumn: sourceColumn,
               featureColumn: featureColumn,
               leftEndColumn: leftEndColumn,
               rightEndColumn: rightEndColumn,
               scoreColumn: scoreColumn,
               strandColumn: strandColumn,
               frameColumn: frameColumn,
               attributeColumn: attributeColumn,
               assembly: assembly,
               readOnlyValidChrs: readOnlyValidChrs,
               maxLinesToRead: maxLinesToRead,
               startOffset: startOffset,
               hashFunction: hashFunction)
        { }
    }
}
