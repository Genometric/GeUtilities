// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;

namespace GeUtilities.Tests.TRefSeqParser
{
    public class Columns
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

        private byte _refSeqIDColumn = 3;
        public byte RefSeqIDColumn
        {
            get { return _refSeqIDColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_refSeqIDColumn);
                _refSeqIDColumn = value;
            }
        }

        private byte _geneSymbolColumn = 4;
        public byte GeneSymbolColumn
        {
            get { return _geneSymbolColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_geneSymbolColumn);
                _geneSymbolColumn = value;
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

        public string Chr { set; get; }
        public char Strand { set; get; }

        public Gene Gene { set; get; }

        public Columns(
            string chr = "chr1",
            int left = 10,
            int right = 20,
            string refSeqID = "RefSeqID",
            string geneSymbol = "GeneSymbol",
            char strand = '*',
            byte chrColumn = 0,
            byte leftColumn = 1,
            sbyte rightColumn = 2,
            byte refSeqIDColumn = 3,
            byte geneSymbolColumn = 4,
            sbyte strandColumn = -1)
        {
            Chr = chr;
            Strand = strand;
            Gene = new Gene()
            {
                Left = left,
                Right = right,
                RefSeqID = refSeqID,
                GeneSymbol = geneSymbol,
            };
            _chrColumn = chrColumn;
            _leftColumn = leftColumn;
            _rightColumn = rightColumn;
            _refSeqIDColumn = refSeqIDColumn;
            _geneSymbolColumn = geneSymbolColumn;
            _strandColumn = strandColumn;
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (newValue < 0)
                newValue = (sbyte)(MaxColumnIndex() + 1);

            if (_chrColumn == oldValue) _chrColumn = (byte)newValue;
            else if (_leftColumn == oldValue) _leftColumn = (byte)newValue;
            else if (_rightColumn == oldValue) _rightColumn = newValue;
            else if (_refSeqIDColumn == oldValue) _refSeqIDColumn = (byte)newValue;
            else if (_geneSymbolColumn == oldValue) _geneSymbolColumn = (byte)newValue;
            else if (_strandColumn == oldValue) _strandColumn = newValue;
        }

        public sbyte MaxColumnIndex()
        {
            return
                Math.Max((sbyte)ChrColumn,
                Math.Max((sbyte)LeftColumn,
                Math.Max(RightColumn,
                Math.Max((sbyte)RefSeqIDColumn,
                Math.Max((sbyte)GeneSymbolColumn, StrandColumn)))));
        }

        public string GetSampleHeader()
        {
            string header = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header += "chr\t";
                else if (LeftColumn == i) header += "Left\t";
                else if (RightColumn == i) header += "Right\t";
                else if (RefSeqIDColumn == i) header += "RefSeqID\t";
                else if (GeneSymbolColumn == i) header += "GeneSymbol\t";
                else if (StrandColumn == i) header += "Strand\t";
                else header += "aBcD\t";

            return header;
        }

        public string GetSampleLine()
        {
            string line = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) line += Chr + "\t";
                else if (LeftColumn == i) line += Gene.Left + "\t";
                else if (RightColumn == i) line += Gene.Right + "\t";
                else if (RefSeqIDColumn == i) line += Gene.RefSeqID + "\t";
                else if (GeneSymbolColumn == i) line += Gene.GeneSymbol + "\t";
                else if (StrandColumn == i) line += Strand + "\t";
                else line += "AbCd\t";

            return line;
        }
    }
}
