// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;

namespace GeUtilities.Tests.GeneralFeatureParser
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

        private sbyte _sourceColumn;
        public sbyte SourceColumn
        {
            get { return _sourceColumn; }
            set
            {
                Swap(value, _sourceColumn);
                _sourceColumn = value;
            }
        }

        private sbyte _featureColumn;
        public sbyte FeatureColumn
        {
            get { return _featureColumn; }
            set
            {
                Swap(value, _featureColumn);
                _featureColumn = value;
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

        private sbyte _scoreColumn;
        public sbyte ScoreColumn
        {
            get { return _scoreColumn; }
            set
            {
                Swap(value, _scoreColumn);
                _scoreColumn = value;
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

        private sbyte _frameColumn;
        public sbyte FrameColumn
        {
            get { return _frameColumn; }
            set
            {
                Swap(value, _frameColumn);
                _frameColumn = value;
            }
        }

        private sbyte _attributeColumn;
        public sbyte AttributeColumn
        {
            get { return _attributeColumn; }
            set
            {
                Swap(value, _attributeColumn);
                _attributeColumn = value;
            }
        }

        public string Chr { set; get; }
        public string Source { set; get; }
        public string Feature { set; get; }
        public int Left { set; get; }
        public int Right { set; get; }
        public double Score { set; get; }
        public char Strand { set; get; }
        public string Frame { set; get; }
        public string Attribute { set; get; }

        public Columns(
            string source = "Source",
            string feature = "Feature",
            string chr = "chr1",
            int left = 10,
            int right = 20,
            double score = 100.0,
            char strand = '*',
            string frame = "Frame",
            string attribute = "att1=1;att2=v2",
            byte chrColumn = 0,
            sbyte sourceColumn = 1,
            sbyte featureColumn = 2,
            byte leftColumn = 3,
            sbyte rightColumn = 4,
            sbyte scoreColumn = 5,
            sbyte strandColumn = 6,
            sbyte frameColumn = 7,
            sbyte attributeColumn = 8
            )
        {
            Source = source;
            Feature = feature;
            Chr = chr;
            Left = left;
            Right = right;
            Score = score;
            Strand = strand;
            Frame = frame;
            Attribute = attribute;
            _chrColumn = chrColumn;
            _sourceColumn = sourceColumn;
            _featureColumn = featureColumn;
            _leftColumn = leftColumn;
            _rightColumn = rightColumn;
            _scoreColumn = scoreColumn;
            _strandColumn = strandColumn;
            _frameColumn = frameColumn;
            _attributeColumn = attributeColumn;
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (newValue < 0)
                newValue = (sbyte)(MaxColumnIndex() + 1);

            if (_chrColumn == oldValue) _chrColumn = (byte)newValue;
            else if (_sourceColumn == oldValue) _sourceColumn = newValue;
            else if (_featureColumn == oldValue) _featureColumn = newValue;
            else if (_leftColumn == oldValue) _leftColumn = (byte)newValue;
            else if (_rightColumn == oldValue) _rightColumn = newValue;
            else if (_scoreColumn == oldValue) _scoreColumn = newValue;
            else if (_strandColumn == oldValue) _strandColumn = newValue;
            else if (_frameColumn == oldValue) _frameColumn = newValue;
            else if (_attributeColumn == oldValue) _attributeColumn = newValue;
        }

        public sbyte MaxColumnIndex()
        {
            return
                Math.Max((sbyte)ChrColumn,
                Math.Max(SourceColumn,
                Math.Max(FeatureColumn,
                Math.Max((sbyte)LeftColumn,
                Math.Max(RightColumn,
                Math.Max(ScoreColumn,
                Math.Max(StrandColumn,
                Math.Max(FrameColumn, AttributeColumn))))))));
        }

        public string GetSampleHeader()
        {
            string header = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header += "chr\t";
                else if (SourceColumn == i) header += "Source\t";
                else if (FeatureColumn == i) header += "Feature\t";
                else if (LeftColumn == i) header += "Left\t";
                else if (RightColumn == i) header += "Right\t";
                else if (ScoreColumn == i) header += "Score\t";
                else if (StrandColumn == i) header += "Strand\t";
                else if (FrameColumn == i) header += "Frame\t";
                else if (AttributeColumn == i) header += "Attribute\t";
                else header += "aBcD\t";

            return header;
        }

        public string GetSampleLine()
        {
            string line = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) line += Chr + "\t";
                else if (SourceColumn == i) line += Source + "\t";
                else if (FeatureColumn == i) line += Feature + "\t";
                else if (LeftColumn == i) line += Left + "\t";
                else if (RightColumn == i) line += Right + "\t";
                else if (ScoreColumn == i) line += Score + "\t";
                else if (StrandColumn == i) line += Strand + "\t";
                else if (FrameColumn == i) line += Frame + "\t";
                else if (AttributeColumn == i) line += Attribute + "\t";
                else line += "AbCd\t";

            return line;
        }
    }
}
