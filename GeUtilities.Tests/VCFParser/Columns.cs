// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;

namespace GeUtilities.Tests.TVCFParser
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

        private byte _positionColumn = 1;
        public byte PositionColumn
        {
            get { return _positionColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_positionColumn);
                _positionColumn = value;
            }
        }

        private byte _idColumn = 2;
        public byte IDColumn
        {
            get { return _idColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_idColumn);
                _idColumn = value;
            }
        }

        private byte _refbColumn = 3;
        public byte RefbColumn
        {
            get { return _refbColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_refbColumn);
                _refbColumn = value;
            }
        }

        private byte _altbColumn = 4;
        public byte AltbColumn
        {
            get { return _altbColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_altbColumn);
                _altbColumn = value;
            }
        }

        private byte _qualityColumn = 5;
        public byte QualityColumn
        {
            get { return _qualityColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_qualityColumn);
                _qualityColumn = value;
            }
        }

        private byte _filterColumn = 6;
        public byte FilterColumn
        {
            get { return _filterColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_filterColumn);
                _filterColumn = value;
            }
        }

        private byte _infoColumn = 7;
        public byte InfoColumn
        {
            get { return _infoColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_infoColumn);
                _infoColumn = value;
            }
        }

        private sbyte _strandColumn = 8;
        public sbyte StrandColumn
        {
            get { return _strandColumn; }
            set
            {
                Swap(value, _strandColumn);
                _strandColumn = value;
            }
        }

        private string _chr = "chr1";
        public string Chr
        {
            set { _chr = value; }
            get { return _chr; }
        }

        private int _position = 10;
        public int Position
        {
            set { _position = value; }
            get { return _position; }
        }

        private string _id = "id_001";
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }

        private Base[] _refBase = new Base[] { Base.A, Base.C, Base.G };
        public Base[] RefBase
        {
            set { _refBase = value; }
            get { return _refBase; }
        }

        private Base[] _altBase = new Base[] { Base.U, Base.T, Base.N };
        public Base[] AltBase
        {
            set { _altBase = value; }
            get { return _altBase; }
        }

        private double _quality = 654.321;
        public double Quality
        {
            set { _quality = value; }
            get { return _quality; }
        }

        private string _filter = "filter_001";
        public string Filter
        {
            set { _filter = value; }
            get { return _filter; }
        }

        private string _info = "info_001";
        public string Info
        {
            set { _info = value; }
            get { return _info; }
        }

        private char _strand = '*';
        public char Strand
        {
            set { _strand = value; }
            get { return _strand; }
        }

        public Variant Variant
        {
            get
            {
                return new Variant()
                {
                    Left = Position,
                    Right = Position + 1,
                    ID = Id,
                    Quality = Quality,
                    Filter = Filter,
                    Info = Info,
                    AltBase = AltBase ?? (new Base[] { Base.U, Base.T, Base.N }),
                    RefBase = RefBase ?? (new Base[] { Base.A, Base.C, Base.G })
                };
            }
        }

        /// <summary>
        /// NOTE: The only option for an array default value is 'null'.
        /// Therefore, a default value for null ref/alt base arrays is
        /// set in the constructor.
        /// and their value is 
        /// </summary>
        public Columns() { }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (newValue < 0)
                newValue = (sbyte)(MaxColumnIndex() + 1);

            if (_chrColumn == oldValue) _chrColumn = (byte)newValue;
            else if (_positionColumn == oldValue) _positionColumn = (byte)newValue;
            else if (_idColumn == oldValue) _idColumn = (byte)newValue;
            else if (_refbColumn == oldValue) _refbColumn = (byte)newValue;
            else if (_altbColumn == oldValue) _altbColumn = (byte)newValue;
            else if (_qualityColumn == oldValue) _qualityColumn = (byte)newValue;
            else if (_filterColumn == oldValue) _filterColumn = (byte)newValue;
            else if (_infoColumn == oldValue) _infoColumn = (byte)newValue;
            else if (_strandColumn == oldValue) _strandColumn = newValue;
        }

        public sbyte MaxColumnIndex()
        {
            return
                Math.Max((sbyte)ChrColumn,
                Math.Max((sbyte)PositionColumn,
                Math.Max((sbyte)IDColumn,
                Math.Max((sbyte)RefbColumn,
                Math.Max((sbyte)AltbColumn,
                Math.Max((sbyte)QualityColumn,
                Math.Max((sbyte)FilterColumn,
                Math.Max((sbyte)InfoColumn, StrandColumn))))))));
        }

        public string GetSampleHeader()
        {
            string header = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header += "chr\t";
                else if (PositionColumn == i) header += "Position\t";
                else if (IDColumn == i) header += "ID\t";
                else if (RefbColumn == i) header += "RefBase\t";
                else if (AltbColumn == i) header += "AltBase\t";
                else if (QualityColumn == i) header += "Quality\t";
                else if (FilterColumn == i) header += "Filter\t";
                else if (InfoColumn == i) header += "Info\t";
                else if (StrandColumn == i) header += "Strand\t";
                else header += "aBcD\t";

            return header;
        }

        public string GetSampleLine()
        {
            string line = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) line += Chr + "\t";
                else if (PositionColumn == i) line += Position + "\t";
                else if (IDColumn == i) line += Id + "\t";
                else if (RefbColumn == i) line += string.Join("", RefBase) + "\t";
                else if (AltbColumn == i) line += string.Join("", AltBase) + "\t";
                else if (QualityColumn == i) line += Quality + "\t";
                else if (FilterColumn == i) line += Filter + "\t";
                else if (InfoColumn == i) line += Info + "\t";
                else if (StrandColumn == i) line += Strand + "\t";
                else line += "AbCd\t";

            return line;
        }
    }
}
