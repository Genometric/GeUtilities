// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Interval.Model;
using Genometric.GeUtilities.Interval.Parsers.Model;
using System;
using System.Text;

namespace GeUtilities.Tests.Interval.Parsers.VCF
{
    public class RegionGenerator
    {
        public VCFColumns Columns { private set; get; }

        public byte ChrColumn
        {
            get { return Columns.Chr; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Chr);
                Columns.Chr = value;
            }
        }

        public byte PositionColumn
        {
            get { return Columns.Left; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Left);
                Columns.Left = value;
            }
        }

        public byte IDColumn
        {
            get { return Columns.ID; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.ID);
                Columns.ID = value;
            }
        }

        public byte RefbColumn
        {
            get { return Columns.RefBase; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.RefBase);
                Columns.RefBase = value;
            }
        }

        public byte AltbColumn
        {
            get { return Columns.AltBase; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.AltBase);
                Columns.AltBase = value;
            }
        }

        public byte QualityColumn
        {
            get { return Columns.Quality; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Quality);
                Columns.Quality = value;
            }
        }

        public byte FilterColumn
        {
            get { return Columns.Filter; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Filter);
                Columns.Filter = value;
            }
        }

        public byte InfoColumn
        {
            get { return Columns.Info; }
            set
            {
                Swap((sbyte)value, (sbyte)Columns.Info);
                Columns.Info = value;
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
                return new Variant(
                    left: Position,
                    right: Position + 1,
                    id: Id,
                    refBase: RefBase ?? (new Base[] { Base.A, Base.C, Base.G }),
                    altBase: AltBase ?? (new Base[] { Base.U, Base.T, Base.N }),
                    quality: Quality, filter: Filter, info: Info);
            }
        }

        public RegionGenerator()
        {
            // NOTE
            // The following default column indexes must match the
            // VCF file type specifications. These specifications can 
            // be obtained from various resources such as the following: 
            // http://www.internationalgenome.org/wiki/Analysis/vcf4.0
            Columns = new VCFColumns()
            {
                Chr = 0,
                Left = 1,
                Right = -1,
                ID = 2,
                RefBase = 3,
                AltBase = 4,
                Quality = 5,
                Filter = 6,
                Info = 7,
                Strand = 8
            };

            Chr = "chr1";
            Position = 10;
            Id = "id_001";
            RefBase = new Base[] { Base.A, Base.C, Base.G };
            AltBase = new Base[] { Base.U, Base.T, Base.N };
            Quality = 654.321;
            Filter = "filter_001";
            Info = "info_001";
            Strand = '*';
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (newValue < 0)
                newValue = (sbyte)(MaxColumnIndex() + 1);

            if (Columns.Chr == oldValue) Columns.Chr = (byte)newValue;
            else if (Columns.Left == oldValue) Columns.Left = (byte)newValue;
            else if (Columns.ID == oldValue) Columns.ID = (byte)newValue;
            else if (Columns.RefBase == oldValue) Columns.RefBase = (byte)newValue;
            else if (Columns.AltBase == oldValue) Columns.AltBase = (byte)newValue;
            else if (Columns.Quality == oldValue) Columns.Quality = (byte)newValue;
            else if (Columns.Filter == oldValue) Columns.Filter = (byte)newValue;
            else if (Columns.Info == oldValue) Columns.Info = (byte)newValue;
            else if (Columns.Strand == oldValue) Columns.Strand = newValue;
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
