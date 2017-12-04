// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class VCFParser<I> : Parser<I, IntervalStats>
        where I : IVCF, new()
    {
        public VCFParser(
            string source,
            Genomes species,
            Assemblies assembly,
            uint maxLinesToBeRead = uint.MaxValue)
        {
            Source = source;
            Genome = species;
            Assembly = assembly;
            ChrColumn = 0;
            LeftColumn = 1;
            RightColumn = -1;
            _idColumn = 2;
            _refbpColumn = 3;
            _altbpColumn = 4;
            _qualityColumn = 5;
            _filterColumn = 6;
            _infoColumn = 7;
            maxLinesToBeRead = uint.MaxValue;
            ReadOnlyValidChrs = false;
            Initialize();
        }

        public VCFParser(
            string source,
            Genomes species,
            Assemblies assembly,
            sbyte chrColumn,
            sbyte positionColumn,
            sbyte idColumn,
            sbyte refbpColumn,
            sbyte altbpColumn,
            sbyte qualityColumn,
            sbyte filterColumn,
            sbyte infoColumn,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue)
        {
            Source = source;
            Genome = species;
            Assembly = assembly;
            ChrColumn = chrColumn;
            LeftColumn = positionColumn;
            RightColumn = -1;
            _idColumn = idColumn;
            _refbpColumn = refbpColumn;
            _altbpColumn = altbpColumn;
            _qualityColumn = qualityColumn;
            _filterColumn = filterColumn;
            _infoColumn = infoColumn;
            this.maxLinesToBeRead = maxLinesToBeRead;
            ReadOnlyValidChrs = readOnlyValidChrs;

            Initialize();
        }

        #region .::.         private Variables declaration               .::.

        private sbyte _idColumn { set; get; }
        private sbyte _refbpColumn { set; get; }
        private sbyte _altbpColumn { set; get; }
        private sbyte _qualityColumn { set; get; }
        private sbyte _filterColumn { set; get; }
        private sbyte _infoColumn { set; get; }

        #endregion

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter, out string intervalName)
        {
            I rtv = new I
            {
                Left = left,
                Right = left + 1
            };

            intervalName = "GeUtil_" + new Random().Next(10000, 100000).ToString();

            if(_idColumn < line.Length)
            {
                rtv.ID = line[_idColumn];
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ID column.");
            }

            if (_refbpColumn < line.Length)
            {
                rtv.RefBase = new BasePair[line[_refbpColumn].Length];
                for (int i = 0; i < line[_refbpColumn].Length; i++)
                {
                    switch (line[_refbpColumn][i])
                    {
                        case 'A': rtv.RefBase[i] = BasePair.A; break;
                        case 'C': rtv.RefBase[i] = BasePair.C; break;
                        case 'G': rtv.RefBase[i] = BasePair.G; break;
                        case 'N': rtv.RefBase[i] = BasePair.N; break;
                        case 'T': rtv.RefBase[i] = BasePair.T; break;
                        case 'U': rtv.RefBase[i] = BasePair.U; break;
                        default:
                            DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
                            break;
                    }
                }
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid REF column.");
            }


            if (_altbpColumn < line.Length)
            {
                rtv.AltBase = new BasePair[line[_altbpColumn].Length];
                for (int i = 0; i < line[_altbpColumn].Length; i++)
                {
                    switch (line[_altbpColumn][i])
                    {
                        case 'A': rtv.AltBase[i] = BasePair.A; break;
                        case 'C': rtv.AltBase[i] = BasePair.C; break;
                        case 'G': rtv.AltBase[i] = BasePair.G; break;
                        case 'N': rtv.AltBase[i] = BasePair.N; break;
                        case 'T': rtv.AltBase[i] = BasePair.T; break;
                        case 'U': rtv.AltBase[i] = BasePair.U; break;
                        default:
                            DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ALT column.");
                            break;
                    }
                }
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid ALT column.");
            }


            if(_qualityColumn < line.Length)
            {
                if (double.TryParse(line[_qualityColumn], out double quality))
                    rtv.Quality = quality;
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid quality column.");
            }


            if(_filterColumn < line.Length)
            {
                rtv.Filter = line[_filterColumn];
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid filter column.");
            }


            if (_infoColumn < line.Length)
            {
                rtv.Info = line[_infoColumn];
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid info column.");
            }

            return rtv;
        }

        public ParsedVariants<I> Parse()
        {
            var rtv = (ParsedVariants<I>)PARSE();
            Status = "100";
            return rtv;
        }
    }
}
