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
            Assemblies assembly = Assemblies.Unknown,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue) :
            this(source: source,
                assembly: assembly,
                chrColumn: 0,
                positionColumn: 1,
                idColumn: 2,
                refbpColumn: 3,
                altbpColumn: 4,
                qualityColumn: 5,
                filterColumn: 6,
                infoColumn: 7,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToBeRead: maxLinesToBeRead)
        { }

        public VCFParser(
            string source,            
            sbyte chrColumn,
            sbyte positionColumn,
            sbyte idColumn,
            sbyte refbpColumn,
            sbyte altbpColumn,
            sbyte qualityColumn,
            sbyte filterColumn,
            sbyte infoColumn,
            Assemblies assembly = Assemblies.Unknown,
            byte startOffset = 0,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            base(source: source,
                assembly: assembly,
                startOffset: startOffset,
                chrColumn: chrColumn,
                leftEndColumn: positionColumn,
                rightEndColumn: -1,
                strandColumn: -1,
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

        #region .::.         private Variables declaration               .::.

        private sbyte _idColumn { set; get; }
        private sbyte _refbpColumn { set; get; }
        private sbyte _altbpColumn { set; get; }
        private sbyte _qualityColumn { set; get; }
        private sbyte _filterColumn { set; get; }
        private sbyte _infoColumn { set; get; }

        #endregion

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter)
        {
            I rtv = new I
            {
                Left = left,
                Right = left + 1
            };

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

        public new ParsedVariants<I> Parse()
        {
            var rtv = (ParsedVariants<I>)base.Parse();
            Status = "100";
            return rtv;
        }
    }
}
