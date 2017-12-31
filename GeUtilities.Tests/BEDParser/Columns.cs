// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.BEDParser
{
    public class Columns
    {
        private byte _chrColumn;
        public byte ChrColumn
        {
            get { return _chrColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_chrColumn);
                _chrColumn = value;
            }
        }

        private byte _leftColumn;
        public byte LeftColumn
        {
            get { return _leftColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_leftColumn);
                _leftColumn = value;
            }
        }

        private sbyte _rightColumn;
        public sbyte RightColumn
        {
            get { return _rightColumn; }
            set
            {
                Swap(value, _rightColumn);
                _rightColumn = value;
            }
        }

        private byte _nameColumn;
        public byte NameColumn
        {
            get { return _nameColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_nameColumn);
                _nameColumn = value;
            }
        }

        private byte _valueColumn;
        public byte ValueColumn
        {
            get { return _valueColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_valueColumn);
                _valueColumn = value;
            }
        }

        private sbyte _strandColumn;
        public sbyte StrandColumn
        {
            get { return _strandColumn; }
            set
            {
                Swap(value, _strandColumn);
                _strandColumn = value;
            }
        }

        private sbyte _summitColumn;
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

        /// <summary>
        /// <para>NOTE 1: the default values of these columns must match the 
        /// default columns order of the parser. </para>
        ///
        /// <para>Note 2: the order of columns received in the constructor
        /// are used/applied as given. Therefore, they are prone to 
        /// overlap. This is intentional. To avoid column number overlapping
        /// assign their values using set/get accessors of the properties. </para>
        /// </summary>
        public Columns(
            string chr = "chr1",
            int left = 10,
            int right = 20,
            int summit = 15,
            string name = "GeUtilities_01",
            double value = 123.45,
            char strand = '*',
            byte chrColumn = 0,
            byte leftColumn = 1,
            sbyte rightColumn = 2,
            byte nameColumn = 3,
            byte valueColumn = 4,
            sbyte strandColumn = -1,
            sbyte summitColumn = -1)
        {
            Chr = chr;
            Left = left;
            Right = right;
            Summit = summit;
            Name = name;
            Value = value;
            Strand = strand;
            _chrColumn = chrColumn;
            _leftColumn = leftColumn;
            _rightColumn = rightColumn;
            _nameColumn = nameColumn;
            _valueColumn = valueColumn;
            _strandColumn = strandColumn;
            _summitColumn = summitColumn;
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
            string header = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header += "chr\t";
                else if (LeftColumn == i) header += "Left\t";
                else if (RightColumn == i) header += "Right\t";
                else if (NameColumn == i) header += "Name\t";
                else if (ValueColumn == i) header += "Value\t";
                else if (StrandColumn == i) header += "Strand\t";
                else if (SummitColumn == i) header += "Summit\t";
                else header += "aBcD\t";

            return header;
        }

        public string GetSampleLine()
        {
            string line = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) line += Chr + "\t";
                else if (LeftColumn == i) line += Left + "\t";
                else if (RightColumn == i) line += Right + "\t";
                else if (NameColumn == i) line += Name + "\t";
                else if (ValueColumn == i) line += Value + "\t";
                else if (StrandColumn == i) line += Strand + "\t";
                else if (SummitColumn == i) line += Summit + "\t";
                else line += "AbCd\t";

            return line;
        }
    }
}
