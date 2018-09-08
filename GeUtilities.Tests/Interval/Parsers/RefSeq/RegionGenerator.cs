// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.Intervals.Model;
using Genometric.GeUtilities.Intervals.Parsers.Model;
using System;
using System.Text;

namespace GeUtilities.Tests.Interval.Parsers.RefSeq
{
    public class RegionGenerator
    {
        public RefSeqColumns Columns { private set; get; }

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

        public byte RefSeqIDColumn
        {
            get { return Columns.RefSeqID; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.RefSeqID);
                Columns.RefSeqID = value;
            }
        }

        public byte GeneSymbolColumn
        {
            get { return Columns.GeneSeymbol; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.GeneSeymbol);
                Columns.GeneSeymbol = value;
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

        public string Chr { set; get; }

        public int Left { set; get; }

        public int Right { set; get; }

        public string RefSeqID { set; get; }

        public string GeneSymbol { set; get; }

        public char Strand { set; get; }

        public Gene Gene
        {
            get
            {
                return new Gene(Left, Right, RefSeqID, GeneSymbol);
            }
        }

        public RegionGenerator()
        {
            Columns = new RefSeqColumns()
            {
                Chr = 0,
                Left = 1,
                Right = 2,
                RefSeqID = 3,
                GeneSeymbol = 4,
                Strand = -1,
            };

            Chr = "chr1";
            Left = 10;
            Right = 20;
            RefSeqID = "RefSeqID";
            GeneSymbol = "GeneSymbol";
            Strand = '*';
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (newValue < 0)
                newValue = (sbyte)(MaxColumnIndex() + 1);

            if (Columns.Chr == oldValue) Columns.Chr = (byte)newValue;
            else if (Columns.Left == oldValue) Columns.Left = (byte)newValue;
            else if (Columns.Right == oldValue) Columns.Right = newValue;
            else if (Columns.RefSeqID == oldValue) Columns.RefSeqID = (byte)newValue;
            else if (Columns.GeneSeymbol == oldValue) Columns.GeneSeymbol = (byte)newValue;
            else if (Columns.Strand == oldValue) Columns.Strand = newValue;
        }

        public sbyte MaxColumnIndex()
        {
            return
                Math.Max((sbyte)ChrColumn,
                Math.Max((sbyte)LeftColumn,
                Math.Max(RightColumn,
                Math.Max((sbyte)RefSeqIDColumn,
                Math.Max((sbyte)GeneSymbolColumn, StrandColumn)))));
        }

        public string GetSampleHeader()
        {
            var header = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header.Append("chr\t");
                else if (LeftColumn == i) header.Append("Left\t");
                else if (RightColumn == i) header.Append("Right\t");
                else if (RefSeqIDColumn == i) header.Append("RefSeqID\t");
                else if (GeneSymbolColumn == i) header.Append("GeneSymbol\t");
                else if (StrandColumn == i) header.Append("Strand\t");
                else header.Append("aBcD\t");

            return header.ToString();
        }

        public string GetSampleLine()
        {
            var lineBuilder = new StringBuilder("");

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) lineBuilder.Append(Chr + "\t");
                else if (LeftColumn == i) lineBuilder.Append(Left + "\t");
                else if (RightColumn == i) lineBuilder.Append(Right + "\t");
                else if (RefSeqIDColumn == i) lineBuilder.Append(RefSeqID + "\t");
                else if (GeneSymbolColumn == i) lineBuilder.Append(GeneSymbol + "\t");
                else if (StrandColumn == i) lineBuilder.Append(Strand + "\t");
                else lineBuilder.Append("AbCd\t");

            return lineBuilder.ToString();
        }
    }
}
