// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Intervals.Parsers.Model;

namespace Genometric.GeUtilities.Intervals.Parsers
{
    public class VcfParser<I> : Parser<I, IntervalStats>
        where I : IVariant
    {
        private readonly byte _idColumn;
        private readonly byte _refbColumn;
        private readonly byte _altbColumn;
        private readonly byte _qualityColumn;
        private readonly byte _filterColumn;
        private readonly byte _infoColumn;
        private readonly IVariantConstructor<I> _constructor;

        public VcfParser(VcfColumns columns, IVariantConstructor<I> constructor) : base(columns)
        {
            _idColumn = columns.ID; ;
            _refbColumn = columns.RefBase;
            _altbColumn = columns.AltBase;
            _qualityColumn = columns.Quality;
            _filterColumn = columns.Filter;
            _infoColumn = columns.Info;
            _constructor = constructor;
        }

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter, string hashSeed)
        {
            string id = null;
            if (_idColumn < line.Length)
                id = line[_idColumn];
            else
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ID column.");

            Base[] refBase = new Base[0];
            if (_refbColumn < line.Length)
            {
                refBase = ParseBases(line[_refbColumn]);
                if (refBase.Length == 0)
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }

            Base[] altBase = new Base[0];
            if (_altbColumn < line.Length)
            {
                altBase = ParseBases(line[_altbColumn]);
                if (altBase.Length == 0)
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ALT column.");
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ALT column.");
            }

            double quality = 0;
            if (!(_qualityColumn < line.Length && double.TryParse(line[_qualityColumn], out quality)))
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid quality column.");

            string filter = null;
            if (_filterColumn < line.Length)
                filter = line[_filterColumn];
            else
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid filter column.");

            string info = null;
            if (_infoColumn < line.Length)
                info = line[_infoColumn];
            else
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid info column.");

            I rtv = _constructor.Construct(left, right, id, refBase, altBase, quality, filter, info, hashSeed);

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
