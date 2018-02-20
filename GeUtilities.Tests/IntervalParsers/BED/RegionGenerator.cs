// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IntervalParsers.Model.Columns;
using Genometric.GeUtilities.IntervalParsers.Model.Defaults;
using System;
using System.Text;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.IntervalParsers.BED
{
    public class RegionGenerator
    {
        public BEDColumns Columns { private set; get; }

        public byte ChrColumn
        {
            get { return Columns.Chr; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Chr);
                Columns.Chr = value;
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

        public byte NameColumn
        {
            get { return Columns.Name; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Name);
                Columns.Name = value;
            }
        }

        public byte ValueColumn
        {
            get { return Columns.Value; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Value);
                Columns.Value = value;
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

        public sbyte SummitColumn
        {
            get { return Columns.Summit; }
            set
            {
                Swap(value, Columns.Summit);
                Columns.Summit = value;
            }
        }

        public string Chr { set; get; }

        public int Left { set; get; }

        public int Right { set; get; }

        public int Summit { set; get; }

        public string Name { set; get; }

        public double Value { set; get; }

        public char Strand { set; get; }

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

        public RegionGenerator()
        {
            // NOTE
            // The following default column indexes must match the
            // BED file type specifications. These specifications can 
            // be obtained from various resources such as Ensembl: 
            // https://uswest.ensembl.org/info/website/upload/bed.html
            Columns = new BEDColumns()
            {
                Chr = 0,
                Left = 1,
                Right = 2,
                Name = 3,
                Value = 4,
                Strand = -1,
                Summit = -1
            };

            Chr = "chr1";
            Left = 10;
            Right = 20;
            Summit = 15;
            Name = "GeUtilities_01";
            Value = 123.45;
            Strand = '*';
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (newValue < 0)
                newValue = (sbyte)(MaxColumnIndex() + 1);

            if (Columns.Chr == oldValue) Columns.Chr = (byte)newValue;
            else if (Columns.Left == oldValue) Columns.Left = (byte)newValue;
            else if (Columns.Right == oldValue) Columns.Right = newValue;
            else if (Columns.Name == oldValue) Columns.Name = (byte)newValue;
            else if (Columns.Value == oldValue) Columns.Value = (byte)newValue;
            else if (Columns.Strand == oldValue) Columns.Strand = newValue;
            else if (Columns.Summit == oldValue) Columns.Summit = newValue;
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

        public string GetSampleLine(char delimiter = '\t')
        {
            var lineBuilder = new StringBuilder("");
            string d = delimiter.ToString();

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) lineBuilder.Append(Chr + d);
                else if (LeftColumn == i) lineBuilder.Append(Left + d);
                else if (RightColumn == i) lineBuilder.Append(Right + d);
                else if (NameColumn == i) lineBuilder.Append(Name + d);
                else if (ValueColumn == i) lineBuilder.Append(Value + d);
                else if (StrandColumn == i) lineBuilder.Append(Strand + d);
                else if (SummitColumn == i) lineBuilder.Append(Summit + d);
                else lineBuilder.Append("AbCd" + d);

            return lineBuilder.ToString();
        }
    }
}
