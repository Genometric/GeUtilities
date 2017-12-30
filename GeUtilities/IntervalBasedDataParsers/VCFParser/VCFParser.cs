// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class VCFParser<I> : Parser<I, IntervalStats>
        where I : IVCF, new()
    {
        #region .::.         private properties         .::.

        private byte _idColumn;
        private byte _refbpColumn;
        private byte _altbpColumn;
        private byte _qualityColumn;
        private byte _filterColumn;
        private byte _infoColumn;

        #endregion

        public VCFParser(
            string sourceFilePath,
            Assemblies assembly = Assemblies.Unknown,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue) :
            this(sourceFilePath: sourceFilePath,
                assembly: assembly,
                chrColumn: 0,
                positionColumn: 1,
                idColumn: 2,
                refbpColumn: 3,
                altbpColumn: 4,
                qualityColumn: 5,
                filterColumn: 6,
                infoColumn: 7,
                strandColumn: -1,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToBeRead: maxLinesToBeRead)
        { }

        public VCFParser(
            string sourceFilePath,
            byte chrColumn,
            byte positionColumn,
            byte idColumn,
            byte refbpColumn,
            byte altbpColumn,
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
            _refbpColumn = refbpColumn;
            _altbpColumn = altbpColumn;
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

            if (_refbpColumn < line.Length)
            {
                rtv.RefBase = ParseBasePairs(line[_refbpColumn]);
                if (rtv.RefBase == null)
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }

            if (_altbpColumn < line.Length)
            {
                rtv.AltBase = ParseBasePairs(line[_altbpColumn]);
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

        private BasePair[] ParseBasePairs(string field)
        {
            BasePair[] rtv = new BasePair[field.Length];
            for (int i = 0; i < field.Length; i++)
            {
                switch (field[i])
                {
                    case 'A': rtv[i] = BasePair.A; break;
                    case 'C': rtv[i] = BasePair.C; break;
                    case 'G': rtv[i] = BasePair.G; break;
                    case 'N': rtv[i] = BasePair.N; break;
                    case 'T': rtv[i] = BasePair.T; break;
                    case 'U': rtv[i] = BasePair.U; break;
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
