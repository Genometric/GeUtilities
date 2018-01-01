// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.ReferenceGenomes;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class VCFParser<I> : Parser<I, IntervalStats>
        where I : IVariant, new()
    {
        #region .::.         private properties         .::.

        private byte _idColumn;
        private byte _refbColumn;
        private byte _altbColumn;
        private byte _qualityColumn;
        private byte _filterColumn;
        private byte _infoColumn;

        #endregion

        public VCFParser(
            string sourceFilePath,
            Assemblies assembly = Assemblies.Unknown,
            byte startOffset = 0,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue) :
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
                maxLinesToBeRead: maxLinesToBeRead)
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
            byte startOffset = 0,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            base(sourceFilePath: sourceFilePath,
                assembly: assembly,
                startOffset: startOffset,
                chrColumn: chrColumn,
                leftEndColumn: positionColumn,
                rightEndColumn: -1,
                strandColumn: strandColumn,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToBeRead: maxLinesToBeRead,
                hashFunction: hashFunction,
                data: new ParsedVariants<I>())
        {
            _idColumn = idColumn;
            _refbColumn = refbColumn;
            _altbColumn = altbColumn;
            _qualityColumn = qualityColumn;
            _filterColumn = filterColumn;
            _infoColumn = infoColumn;
        }

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter)
        {
            I rtv = new I
            {
                Left = left,
                Right = left + 1
            };

            if (_idColumn < line.Length)
                rtv.ID = line[_idColumn];
            else
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ID column.");

            if (_refbColumn < line.Length)
            {
                rtv.RefBase = ParseBases(line[_refbColumn]);
                if (rtv.RefBase == null)
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }

            if (_altbColumn < line.Length)
            {
                rtv.AltBase = ParseBases(line[_altbColumn]);
                if (rtv.AltBase == null)
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ALT column.");
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ALT column.");
            }

            if (_qualityColumn < line.Length && double.TryParse(line[_qualityColumn], out double quality))
                rtv.Quality = quality;
            else
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid quality column.");

            if (_filterColumn < line.Length)
                rtv.Filter = line[_filterColumn];
            else
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid filter column.");

            if (_infoColumn < line.Length)
                rtv.Info = line[_infoColumn];
            else
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid info column.");

            return rtv;
        }

        private Base[] ParseBases(string field)
        {
            Base[] rtv = new Base[field.Length];
            for (int i = 0; i < field.Length; i++)
            {
                switch (field[i])
                {
                    case 'A': rtv[i] = Base.A; break;
                    case 'C': rtv[i] = Base.C; break;
                    case 'G': rtv[i] = Base.G; break;
                    case 'N': rtv[i] = Base.N; break;
                    case 'T': rtv[i] = Base.T; break;
                    case 'U': rtv[i] = Base.U; break;
                    default:
                        return null;
                }
            }
            return rtv;
        }

        public new ParsedVariants<I> Parse()
        {
            var rtv = (ParsedVariants<I>)base.Parse();
            Status = "100";
            return rtv;
        }
    }
}
