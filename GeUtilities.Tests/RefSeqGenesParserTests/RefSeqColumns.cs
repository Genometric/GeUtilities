// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

namespace GeUtilities.Tests.RefSeqGenesParserTests
{
    public class RefSeqColumns
    {
        private byte _chrColumn = 0;
        public byte ChrColumn
        {
            get { return _chrColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_chrColumn);
                _chrColumn = value;
            }
        }

        private byte _leftColumn = 1;
        public byte LeftColumn
        {
            get { return _leftColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_leftColumn);
                _leftColumn = value;
            }
        }

        private sbyte _rightColumn = 2;
        public sbyte RightColumn
        {
            get { return _rightColumn; }
            set
            {
                Swap(value, _rightColumn);
                _rightColumn = value;
            }
        }

        private sbyte _refSeqIDColumn = 3;
        public sbyte RefSeqIDColumn
        {
            get { return _refSeqIDColumn; }
            set
            {
                Swap(value, _refSeqIDColumn);
                _refSeqIDColumn = value;
            }
        }

        private sbyte _officialGeneSymbolColumn = 4;
        public sbyte OfficialGeneSymbolColumn
        {
            get { return _officialGeneSymbolColumn; }
            set
            {
                Swap(value, _officialGeneSymbolColumn);
                _officialGeneSymbolColumn = value;
            }
        }

        private sbyte _strandColumn = -1;
        public sbyte StrandColumn
        {
            get { return _strandColumn; }
            set
            {
                Swap(value, _strandColumn);
                _strandColumn = value;
            }
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (_chrColumn == oldValue) _chrColumn = (byte)newValue;
            else if (_leftColumn == oldValue) _leftColumn = (byte)newValue;
            else if (_rightColumn == oldValue) _rightColumn = newValue;
            else if (_refSeqIDColumn == oldValue) _refSeqIDColumn = newValue;
            else if (_officialGeneSymbolColumn == oldValue) _officialGeneSymbolColumn = newValue;
            else if (_strandColumn == oldValue) _strandColumn = newValue;
        }

        public sbyte MaxColumnIndex()
        {
            return
                Math.Max((sbyte)ChrColumn,
                Math.Max((sbyte)LeftColumn,
                Math.Max(RightColumn,
                Math.Max(RefSeqIDColumn,
                Math.Max(OfficialGeneSymbolColumn, StrandColumn)))));
        }

        public void GetRefSeqLine(
            out string line,
            out string header,
            string chr = "chr1",
            string left = "10",
            string right = "20",
            string refSeqID = "refSeqID_01",
            string geneSymbol = "geneSymbol_01",
            string strand = "*")
        {
            line = "";
            header = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
            {
                if (ChrColumn == i)
                {
                    line += chr + "\t";
                    header += "chr\t";
                }
                else if (LeftColumn == i)
                {
                    line += left + "\t";
                    header += "Left\t";
                }
                else if (RightColumn == i)
                {
                    line += right + "\t";
                    header += "Right\t";
                }
                else if (RefSeqIDColumn == i)
                {
                    line += refSeqID + "\t";
                    header += "RefSeqID\t";
                }
                else if (OfficialGeneSymbolColumn == i)
                {
                    line += geneSymbol + "\t";
                    header += "GeneSymbol\t";
                }
                else if (StrandColumn == i)
                {
                    line += strand + "\t";
                    header += "Strand\t";
                }
                else
                {
                    line += "AbCd\t";
                    header += "aBcD\t";
                }
            }
        }
    }
}
