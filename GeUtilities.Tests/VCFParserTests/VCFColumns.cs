// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

namespace GeUtilities.Tests.VCFParserTests
{
    public class VCFColumns
    {
        private sbyte _chrColumn = 0;
        public sbyte ChrColumn
        {
            get { return _chrColumn; }
            set
            {
                Swap(value, _chrColumn);
                _chrColumn = value;
            }
        }

        private sbyte _positionColumn = 1;
        public sbyte PositionColumn
        {
            get { return _positionColumn; }
            set
            {
                Swap(value, _positionColumn);
                _positionColumn = value;
            }
        }

        private sbyte _idColumn = 2;
        public sbyte IDColumn
        {
            get { return _idColumn; }
            set
            {
                Swap(value, _idColumn);
                _idColumn = value;
            }
        }

        private sbyte _refbpColumn = 3;
        public sbyte RefbbpColumn
        {
            get { return _refbpColumn; }
            set
            {
                Swap(value, _refbpColumn);
                _refbpColumn = value;
            }
        }

        private sbyte _altbpColumn = 4;
        public sbyte AltbpColumn
        {
            get { return _altbpColumn; }
            set
            {
                Swap(value, _altbpColumn);
                _altbpColumn = value;
            }
        }

        private sbyte _qualityColumn = 5;
        public sbyte QualityColumn
        {
            get { return _qualityColumn; }
            set
            {
                Swap(value, _qualityColumn);
                _qualityColumn = value;
            }
        }

        private sbyte _filterColumn = 6;
        public sbyte FilterColumn
        {
            get { return _filterColumn; }
            set
            {
                Swap(value, _filterColumn);
                _filterColumn = value;
            }
        }

        private sbyte _infoColumn = 7;
        public sbyte InfoColumn
        {
            get { return _infoColumn; }
            set
            {
                Swap(value, _infoColumn);
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
            if (_chrColumn == oldValue) _chrColumn = newValue;
            else if (_positionColumn == oldValue) _positionColumn = newValue;
            else if (_idColumn == oldValue) _idColumn = newValue;
            else if (_refbpColumn == oldValue) _refbpColumn = newValue;
            else if (_altbpColumn == oldValue) _altbpColumn = newValue;
            else if (_qualityColumn == oldValue) _qualityColumn = newValue;
            else if (_filterColumn == oldValue) _filterColumn = newValue;
            else if (_infoColumn == oldValue) _infoColumn = newValue;
            else if (_strandColumn == oldValue) _strandColumn = newValue;
        }

        public sbyte MaxColumnIndex()
        {
            return
                Math.Max(ChrColumn,
                Math.Max(PositionColumn,
                Math.Max(IDColumn,
                Math.Max(RefbbpColumn,
                Math.Max(AltbpColumn,
                Math.Max(QualityColumn,
                Math.Max(FilterColumn,
                Math.Max(InfoColumn, StrandColumn))))))));
        }
    }
}
