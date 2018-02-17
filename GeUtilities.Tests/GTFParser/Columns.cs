// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;
using System.Text;

namespace GeUtilities.Tests.TGTFParser
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

        public string Source { set; get; }

        public string Feature { set; get; }

        public string Chr { set; get; }

        public int Left { set; get; }

        public int Right { set; get; }

        public double Score { set; get; }

        public char Strand { set; get; }

        public string Frame { set; get; }

        public string Attribute { set; get; }

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

        public Columns()
        {
            Source = "Source";
            Feature = "Feature";
            Chr = "chr1";
            Left = 10;
            Right = 20;
            Score = 100.0;
            Strand = '*';
            Frame = "Frame";
            Attribute = "att1=1;att2=v2";
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
            var header = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header.Append("chr\t");
                else if (SourceColumn == i) header.Append("Source\t");
                else if (FeatureColumn == i) header.Append("Feature\t");
                else if (LeftColumn == i) header.Append("Left\t");
                else if (RightColumn == i) header.Append("Right\t");
                else if (ScoreColumn == i) header.Append("Score\t");
                else if (StrandColumn == i) header.Append("Strand\t");
                else if (FrameColumn == i) header.Append("Frame\t");
                else if (AttributeColumn == i) header.Append("Attribute\t");
                else header.Append("aBcD\t");

            return header.ToString();
        }

        public string GetSampleLine()
        {
            var lineBuilder = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) lineBuilder.Append(Chr + "\t");
                else if (SourceColumn == i) lineBuilder.Append(Source + "\t");
                else if (FeatureColumn == i) lineBuilder.Append(Feature + "\t");
                else if (LeftColumn == i) lineBuilder.Append(Left + "\t");
                else if (RightColumn == i) lineBuilder.Append(Right + "\t");
                else if (ScoreColumn == i) lineBuilder.Append(Score + "\t");
                else if (StrandColumn == i) lineBuilder.Append(Strand + "\t");
                else if (FrameColumn == i) lineBuilder.Append(Frame + "\t");
                else if (AttributeColumn == i) lineBuilder.Append(Attribute + "\t");
                else lineBuilder.Append("AbCd\t");

            return lineBuilder.ToString();
        }
    }
}
