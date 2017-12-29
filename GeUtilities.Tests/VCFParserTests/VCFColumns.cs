// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

namespace GeUtilities.Tests.VCFParserTests
{
    public class VCFColumns
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

        private byte _refbpColumn = 3;
        public byte RefbbpColumn
        {
            get { return _refbpColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_refbpColumn);
                _refbpColumn = value;
            }
        }

        private byte _altbpColumn = 4;
        public byte AltbpColumn
        {
            get { return _altbpColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_altbpColumn);
                _altbpColumn = value;
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

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (_chrColumn == oldValue) _chrColumn = (byte)newValue;
            else if (_positionColumn == oldValue) _positionColumn = (byte)newValue;
            else if (_idColumn == oldValue) _idColumn = (byte)newValue;
            else if (_refbpColumn == oldValue) _refbpColumn = (byte)newValue;
            else if (_altbpColumn == oldValue) _altbpColumn = (byte)newValue;
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
                Math.Max((sbyte)RefbbpColumn,
                Math.Max((sbyte)AltbpColumn,
                Math.Max((sbyte)QualityColumn,
                Math.Max((sbyte)FilterColumn,
                Math.Max((sbyte)InfoColumn, StrandColumn))))))));
        }
    }
}
