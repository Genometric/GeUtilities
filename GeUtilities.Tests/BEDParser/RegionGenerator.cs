// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;
using System.Text;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.TBEDParser
{
    public class RegionGenerator
    {
        // NOTE
        // The default column indexes (i.e., the values of properties such as
        // ChrColumn, LeftColumn, and etc.) must match the parsers defaults.

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

        private byte _nameColumn = 3;
        public byte NameColumn
        {
            get { return _nameColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_nameColumn);
                _nameColumn = value;
            }
        }

        private byte _valueColumn = 4;
        public byte ValueColumn
        {
            get { return _valueColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_valueColumn);
                _valueColumn = value;
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

        private sbyte _summitColumn = -1;
        public sbyte SummitColumn
        {
            get { return _summitColumn; }
            set
            {
                Swap(value, _summitColumn);
                _summitColumn = value;
            }
        }

        public string Chr { set; get; }

        public int Left { set; get; }

        public int Right { set; get; }

        public int Summit { set; get; }

        public string Name { set; get; }

        public double Value { set; get; }

        public char Strand { set; get; }

        public ChIPSeqPeak Peak
        {
            get
            {
                return new ChIPSeqPeak
                {
                    Left = Left,
                    Right = Right,
                    Summit = Summit,
                    Name = Name,
                    Value = Value
                };
            }
        }

        public RegionGenerator()
        {
            Chr = "chr1";
            Left = 10;
            Right = 20;
            Summit = 15;
            Name = "GeUtilities_01";
            Value = 123.45;
            Strand = '*';
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (newValue < 0)
                newValue = (sbyte)(MaxColumnIndex() + 1);

            if (_chrColumn == oldValue) _chrColumn = (byte)newValue;
            else if (_leftColumn == oldValue) _leftColumn = (byte)newValue;
            else if (_rightColumn == oldValue) _rightColumn = newValue;
            else if (_nameColumn == oldValue) _nameColumn = (byte)newValue;
            else if (_valueColumn == oldValue) _valueColumn = (byte)newValue;
            else if (_strandColumn == oldValue) _strandColumn = newValue;
            else if (_summitColumn == oldValue) _summitColumn = newValue;
        }

        public sbyte MaxColumnIndex()
        {
            return
                Math.Max((sbyte)ChrColumn,
                Math.Max((sbyte)LeftColumn,
                Math.Max(RightColumn,
                Math.Max((sbyte)NameColumn,
                Math.Max((sbyte)ValueColumn,
                Math.Max(StrandColumn, SummitColumn))))));
        }

        public string GetSampleHeader()
        {
            var header = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header.Append("chr\t");
                else if (LeftColumn == i) header.Append("Left\t");
                else if (RightColumn == i) header.Append("Right\t");
                else if (NameColumn == i) header.Append("Name\t");
                else if (ValueColumn == i) header.Append("Value\t");
                else if (StrandColumn == i) header.Append("Strand\t");
                else if (SummitColumn == i) header.Append("Summit\t");
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
                else if (NameColumn == i) lineBuilder.Append(Name + "\t");
                else if (ValueColumn == i) lineBuilder.Append(Value + "\t");
                else if (StrandColumn == i) lineBuilder.Append(Strand + "\t");
                else if (SummitColumn == i) lineBuilder.Append(Summit + "\t");
                else lineBuilder.Append("AbCd\t");

            return lineBuilder.ToString();
        }
    }
}
