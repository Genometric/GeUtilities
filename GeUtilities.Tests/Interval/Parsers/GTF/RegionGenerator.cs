// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Interval.Model;
using Genometric.GeUtilities.Interval.Parsers.Model;
using System;
using System.Text;

namespace GeUtilities.Tests.Interval.Parsers.GTF
{
    public class RegionGenerator
    {
        public GTFColumns Columns { private set; get; }

        public byte ChrColumn
        {
            get { return Columns.Chr; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Chr);
                Columns.Chr = value;
            }
        }

        public sbyte SourceColumn
        {
            get { return Columns.Source; }
            set
            {
                Swap(value, Columns.Source);
                Columns.Source = value;
            }
        }

        public sbyte FeatureColumn
        {
            get { return Columns.Feature; }
            set
            {
                Swap(value, Columns.Feature);
                Columns.Feature = value;
            }
        }

        public byte LeftColumn
        {
            get { return Columns.Left; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Left);
                Columns.Left = value;
            }
        }

        public sbyte RightColumn
        {
            get { return Columns.Right; }
            set
            {
                Swap(value, Columns.Right);
                Columns.Right = value;
            }
        }

        public sbyte ScoreColumn
        {
            get { return Columns.Score; }
            set
            {
                Swap(value, Columns.Score);
                Columns.Score = value;
            }
        }

        public sbyte StrandColumn
        {
            get { return Columns.Strand; }
            set
            {
                Swap(value, Columns.Strand);
                Columns.Strand = value;
            }
        }

        public sbyte FrameColumn
        {
            get { return Columns.Frame; }
            set
            {
                Swap(value, Columns.Frame);
                Columns.Frame = value;
            }
        }

        public sbyte AttributeColumn
        {
            get { return Columns.Attribute; }
            set
            {
                Swap(value, Columns.Attribute);
                Columns.Attribute = value;
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
                return new GeneralFeature(Left, Right, Source, Feature, Score, Frame, Attribute);
            }
        }

        public RegionGenerator()
        {
            // NOTE
            // The following default column indexes must match the
            // GTF file type specifications. These specifications can 
            // be obtained from various resources such as Ensembl: 
            // https://uswest.ensembl.org/info/website/upload/gff.html
            Columns = new GTFColumns()
            {
                Chr = 0,
                Source = 1,
                Feature = 2,
                Left = 3,
                Right = 4,
                Score = 5,
                Strand = 6,
                Frame = 7,
                Attribute = 8
            };

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

            if (Columns.Chr == oldValue) Columns.Chr = (byte)newValue;
            else if (Columns.Source == oldValue) Columns.Source = newValue;
            else if (Columns.Feature == oldValue) Columns.Feature = newValue;
            else if (Columns.Left == oldValue) Columns.Left = (byte)newValue;
            else if (Columns.Right == oldValue) Columns.Right = newValue;
            else if (Columns.Score == oldValue) Columns.Score = newValue;
            else if (Columns.Strand == oldValue) Columns.Strand = newValue;
            else if (Columns.Frame == oldValue) Columns.Frame = newValue;
            else if (Columns.Attribute == oldValue) Columns.Attribute = newValue;
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
