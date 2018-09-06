// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalParsers.Model.Columns;

namespace Genometric.GeUtilities.IntervalParsers
{
    public class VCFParser<I> : Parser<I, IntervalStats>
        where I : IVariant
    {
        #region .::.         private properties         .::.

        private readonly byte _idColumn;
        private readonly byte _refbColumn;
        private readonly byte _altbColumn;
        private readonly byte _qualityColumn;
        private readonly byte _filterColumn;
        private readonly byte _infoColumn;

        #endregion

        public VCFParser() : this(new VCFColumns())
        { }

        public VCFParser(VCFColumns columns) : base(columns)
        {
            _idColumn = columns.ID; ;
            _refbColumn = columns.RefBase;
            _altbColumn = columns.AltBase;
            _qualityColumn = columns.Quality;
            _filterColumn = columns.Filter;
            _infoColumn = columns.Info;
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
                if (rtv.RefBase.Length == 0)
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }

            if (_altbColumn < line.Length)
            {
                rtv.AltBase = ParseBases(line[_altbColumn]);
                if (rtv.AltBase.Length == 0)
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
                        return new Base[0];
                }
            }
            return rtv;
        }

        public VCF<I> Parse(string sourceFilePath)
        {
            var rtv = (VCF<I>)Parse(sourceFilePath, new VCF<I>());
            Status = "100";
            return rtv;
        }
    }
}
