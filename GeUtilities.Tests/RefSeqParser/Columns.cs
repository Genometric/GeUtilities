// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;
using System.Text;

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

        public int Left { set; get; }

        public int Right { set; get; }

        public string RefSeqID { set; get; }

        public string GeneSymbol { set; get; }

        public char Strand { set; get; }

        public Gene Gene
        {
            get
            {
                return new Gene()
                {
                    Left = Left,
                    Right = Right,
                    RefSeqID = RefSeqID,
                    GeneSymbol = GeneSymbol,
                };
            }
        }

        public Columns() {
            Chr = "chr1";
            Left = 10;
            Right = 20;
            RefSeqID = "RefSeqID";
            GeneSymbol = "GeneSymbol";
            Strand = '*';
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
            var header = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header.Append("chr\t");
                else if (LeftColumn == i) header.Append("Left\t");
                else if (RightColumn == i) header.Append("Right\t");
                else if (RefSeqIDColumn == i) header.Append("RefSeqID\t");
                else if (GeneSymbolColumn == i) header.Append("GeneSymbol\t");
                else if (StrandColumn == i) header.Append("Strand\t");
                else header.Append("aBcD\t");

            return header.ToString();
        }

        public string GetSampleLine()
        {
            var lineBuilder = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) lineBuilder.Append(Chr + "\t");
                else if (LeftColumn == i) lineBuilder.Append(Left + "\t");
                else if (RightColumn == i) lineBuilder.Append(Right + "\t");
                else if (RefSeqIDColumn == i) lineBuilder.Append(RefSeqID + "\t");
                else if (GeneSymbolColumn == i) lineBuilder.Append(GeneSymbol + "\t");
                else if (StrandColumn == i) lineBuilder.Append(Strand + "\t");
                else lineBuilder.Append("AbCd\t");

            return lineBuilder.ToString();
        }
    }
}
