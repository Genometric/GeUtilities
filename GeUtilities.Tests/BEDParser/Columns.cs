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

        private string _chr = "chr1";
        public string Chr
        {
            set { _chr = value; }
            get { return _chr; }
        }

        private int _left = 10;
        public int Left
        {
            set { _left = value; }
            get { return _left; }
        }

        private int _right = 20;
        public int Right
        {
            set { _right = value; }
            get { return _right; }
        }

        private int _summit = 15;
        public int Summit
        {
            set { _summit = value; }
            get { return _summit; }
        }

        private string _name = "GeUtilities_01";
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        private double _value = 123.45;
        public double Value
        {
            set { _value = value; }
            get { return _value; }
        }

        private char _strand = '*';
        public char Strand
        {
            set { _strand = value; }
            get { return _strand; }
        }

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

        /// <summary>
        /// <para>NOTE 1: the default values of the columns must match the 
        /// default columns order of the parser. </para>
        ///
        /// <para>Note 2: the order of columns received in the constructor
        /// are used/applied as given. Therefore, they are prone to 
        /// overlap. This is intentional. To avoid column number overlapping
        /// assign their values using set/get accessors of the properties. </para>
        /// </summary>
        public Columns() { }

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
