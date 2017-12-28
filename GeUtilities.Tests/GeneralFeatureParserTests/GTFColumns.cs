// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

namespace GeUtilities.Tests.GeneralFeatureParserTests
{
    public class GTFColumns
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

        private sbyte _sourceColumn = 1;
        public sbyte SourceColumn
        {
            get { return _sourceColumn; }
            set
            {
                Swap(value, _sourceColumn);
                _sourceColumn = value;
            }
        }

        private sbyte _featureColumn = 2;
        public sbyte FeatureColumn
        {
            get { return _featureColumn; }
            set
            {
                Swap(value, _featureColumn);
                _featureColumn = value;
            }
        }

        private sbyte _leftColumn = 3;
        public sbyte LeftColumn
        {
            get { return _leftColumn; }
            set
            {
                Swap(value, _leftColumn);
                _leftColumn = value;
            }
        }

        private sbyte _rightColumn = 4;
        public sbyte RightColumn
        {
            get { return _rightColumn; }
            set
            {
                Swap(value, _rightColumn);
                _rightColumn = value;
            }
        }

        private sbyte _scoreColumn = 5;
        public sbyte ScoreColumn
        {
            get { return _scoreColumn; }
            set
            {
                Swap(value, _scoreColumn);
                _scoreColumn = value;
            }
        }

        private sbyte _strandColumn = 6;
        public sbyte StrandColumn
        {
            get { return _strandColumn; }
            set
            {
                Swap(value, _strandColumn);
                _strandColumn = value;
            }
        }

        private sbyte _frameColumn = 7;
        public sbyte FrameColumn
        {
            get { return _frameColumn; }
            set
            {
                Swap(value, _frameColumn);
                _frameColumn = value;
            }
        }

        private sbyte _attributeColumn = 8;
        public sbyte AttributeColumn
        {
            get { return _attributeColumn; }
            set
            {
                Swap(value, _attributeColumn);
                _attributeColumn = value;
            }
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (_chrColumn == oldValue) _chrColumn = newValue;
            else if (_sourceColumn == oldValue) _sourceColumn = newValue;
            else if (_featureColumn == oldValue) _featureColumn = newValue;
            else if (_leftColumn == oldValue) _leftColumn = newValue;
            else if (_rightColumn == oldValue) _rightColumn = newValue;
            else if (_scoreColumn == oldValue) _scoreColumn = newValue;
            else if (_strandColumn == oldValue) _strandColumn = newValue;
            else if (_frameColumn == oldValue) _frameColumn = newValue;
            else if (_attributeColumn == oldValue) _attributeColumn = newValue;
        }

        public sbyte MaxColumnIndex()
        {
            return
                Math.Max(ChrColumn,
                Math.Max(SourceColumn,
                Math.Max(FeatureColumn,
                Math.Max(LeftColumn,
                Math.Max(RightColumn,
                Math.Max(ScoreColumn,
                Math.Max(StrandColumn,
                Math.Max(StrandColumn,
                Math.Max(FrameColumn, AttributeColumn)))))))));
        }
    }
}
