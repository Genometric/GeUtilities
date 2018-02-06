// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;

namespace GeUtilities.Tests.TGTFParser
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

        private byte _leftColumn = 3;
        public byte LeftColumn
        {
            get { return _leftColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_leftColumn);
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

        private string _source = "Source";
        public string Source
        {
            set { _source = value; }
            get { return _source; }
        }

        private string _feature = "Feature";
        public string Feature
        {
            set { _feature = value; }
            get { return _feature; }
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

        private double _score = 100.0;
        public double Score
        {
            set { _score = value; }
            get { return _score; }
        }

        private char _strand = '*';
        public char Strand
        {
            set { _strand = value; }
            get { return _strand; }
        }

        private string _frame = "Frame";
        public string Frame
        {
            set { _frame = value; }
            get { return _frame; }
        }

        private string _attribute = "att1=1;att2=v2";
        public string Attribute
        {
            set { _attribute = value; }
            get { return _attribute; }
        }

        public GeneralFeature GFeature { get
            {
                return new GeneralFeature
                {
                    Source = Source,
                    Feature = Feature,
                    Left = Left,
                    Right = Right,
                    Score = Score,
                    Frame = Frame,
                    Attribute = Attribute,
                };
            }
        }

        public Columns() { }

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
