// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;
using System.Text;

namespace GeUtilities.Tests.TVCFParser
{
    public class Columns
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

        public string Chr { set; get; }

        public int Position { set; get; }

        public string Id { set; get; }

        public Base[] RefBase { set; get; }

        public Base[] AltBase { set; get; }

        public double Quality { set; get; }

        public string Filter { set; get; }

        public string Info { set; get; }

        public char Strand { set; get; }

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

        public Columns() {
            Chr = "chr1";
            Position = 10;
            Id = "id_001";
            RefBase = new Base[] { Base.A, Base.C, Base.G };
            AltBase = new Base[] { Base.U, Base.T, Base.N };
            Quality = 654.321;
            Filter= "filter_001";
            Info = "info_001";
            Strand = '*';
        }

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
            var header = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header.Append("chr\t");
                else if (PositionColumn == i) header.Append("Position\t");
                else if (IDColumn == i) header.Append("ID\t");
                else if (RefbColumn == i) header.Append("RefBase\t");
                else if (AltbColumn == i) header.Append("AltBase\t");
                else if (QualityColumn == i) header.Append("Quality\t");
                else if (FilterColumn == i) header.Append("Filter\t");
                else if (InfoColumn == i) header.Append("Info\t");
                else if (StrandColumn == i) header.Append("Strand\t");
                else header.Append("aBcD\t");

            return header.ToString();
        }

        public string GetSampleLine()
        {
            var lineBuilder = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) lineBuilder.Append(Chr + "\t");
                else if (PositionColumn == i) lineBuilder.Append(Position + "\t");
                else if (IDColumn == i) lineBuilder.Append(Id + "\t");
                else if (RefbColumn == i) lineBuilder.Append(string.Join("", RefBase) + "\t");
                else if (AltbColumn == i) lineBuilder.Append(string.Join("", AltBase) + "\t");
                else if (QualityColumn == i) lineBuilder.Append(Quality + "\t");
                else if (FilterColumn == i) lineBuilder.Append(Filter + "\t");
                else if (InfoColumn == i) lineBuilder.Append(Info + "\t");
                else if (StrandColumn == i) lineBuilder.Append(Strand + "\t");
                else lineBuilder.Append("AbCd\t");

            return lineBuilder.ToString();
        }
    }
}
