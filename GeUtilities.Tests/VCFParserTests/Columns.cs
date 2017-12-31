// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalBasedDataParsers.Model.Defaults;
using System;

namespace GeUtilities.Tests.VCFParser
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

        private byte _refbColumn = 3;
        public byte RefbColumn
        {
            get { return _refbColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_refbColumn);
                _refbColumn = value;
            }
        }

        private byte _altbColumn = 4;
        public byte AltbColumn
        {
            get { return _altbColumn; }
            set
            {
                Swap((sbyte)value, (sbyte)_altbColumn);
                _altbColumn = value;
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

        public string Chr { set; get; }
        public char Strand { set; get; }

        public Variant Variant { set; get; }

        /// <summary>
        /// NOTE: The only option for an array default value is 'null'.
        /// Therefore, a default value for null ref/alt base arrays is
        /// set in the constructor.
        /// and their value is 
        /// </summary>
        public Columns(
            string chr = "chr1",
            int position = 10,
            string id = "id_001",
            Base[] refBase = null,
            Base[] altBase = null,
            double quality = 654.321,
            string filter = "filter_001",
            string info = "info_001",
            char strand = '*',
            byte chrColumn = 0,
            byte positionColumn = 1,
            byte idColumn = 2,
            byte refbColumn = 3,
            byte altbColumn = 4,
            byte qualityColumn = 5,
            byte filterColumn = 6,
            byte infoColumn = 7,
            sbyte strandColumn = -1
            )
        {
            Chr = chr;
            Strand = strand;
            Variant = new Variant()
            {
                Left = position,
                ID = id,
                Quality = quality,
                Filter = filter,
                Info = info,
                AltBase = altBase ?? (new Base[] { Base.U, Base.T, Base.N }),
                RefBase = refBase ?? (new Base[] { Base.A, Base.C, Base.G })
            };
            ChrColumn = chrColumn;
            PositionColumn = positionColumn;
            IDColumn = idColumn;
            RefbColumn = refbColumn;
            AltbColumn = altbColumn;
            QualityColumn = qualityColumn;
            FilterColumn = filterColumn;
            InfoColumn = infoColumn;
            StrandColumn = strandColumn;
        }

        private void Swap(sbyte oldValue, sbyte newValue)
        {
            if (newValue < 0)
                newValue = (sbyte)(MaxColumnIndex() + 1);

            if (_chrColumn == oldValue) _chrColumn = (byte)newValue;
            else if (_positionColumn == oldValue) _positionColumn = (byte)newValue;
            else if (_idColumn == oldValue) _idColumn = (byte)newValue;
            else if (_refbColumn == oldValue) _refbColumn = (byte)newValue;
            else if (_altbColumn == oldValue) _altbColumn = (byte)newValue;
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
                Math.Max((sbyte)RefbColumn,
                Math.Max((sbyte)AltbColumn,
                Math.Max((sbyte)QualityColumn,
                Math.Max((sbyte)FilterColumn,
                Math.Max((sbyte)InfoColumn, StrandColumn))))))));
        }

        public string GetSampleHeader()
        {
            string header = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) header += "chr\t";
                else if (PositionColumn == i) header += "Position\t";
                else if (IDColumn == i) header += "ID\t";
                else if (RefbColumn == i) header += "RefBase\t";
                else if (AltbColumn == i) header += "AltBase\t";
                else if (QualityColumn == i) header += "Quality\t";
                else if (FilterColumn == i) header += "Filter\t";
                else if (InfoColumn == i) header += "Info\t";
                else if (StrandColumn == i) header += "Strand\t";
                else header += "aBcD\t";

            return header;
        }

        public string GetSampleLine()
        {
            string line = "";

            for (sbyte i = 0; i <= MaxColumnIndex(); i++)
                if (ChrColumn == i) line += Chr + "\t";
                else if (PositionColumn == i) line += Variant.Left + "\t";
                else if (IDColumn == i) line += Variant.ID + "\t";
                else if (RefbColumn == i) line += string.Join("", Variant.RefBase) + "\t";
                else if (AltbColumn == i) line += string.Join("", Variant.AltBase) + "\t";
                else if (QualityColumn == i) line += Variant.Quality + "\t";
                else if (FilterColumn == i) line += Variant.Filter + "\t";
                else if (InfoColumn == i) line += Variant.Info + "\t";
                else if (StrandColumn == i) line += Strand + "\t";
                else line += "AbCd\t";

            return line;
        }
    }
}
