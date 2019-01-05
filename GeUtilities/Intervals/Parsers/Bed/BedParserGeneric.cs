// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Intervals.Parsers.Model;
using System;

namespace Genometric.GeUtilities.Intervals.Parsers
{
    public class BedParser<I> : Parser<I, BedStats>
        where I : IPeak
    {
        /// <summary>
        /// Sets and gets the column number of peak name.
        /// </summary>
        private readonly byte _nameColumn;

        /// <summary>
        /// Sets and gets the column number of p-value.
        /// </summary>
        private readonly byte _valueColumn;

        /// <summary>
        /// Sets and gets the column number of peak summit.
        /// </summary>
        private readonly sbyte _summitColumn;

        /// <summary>
        /// When read process is finished, this variable contains the number
        /// of regions that contained invalid p-value and replaced by default p-value. 
        /// </summary>
        private ushort _defaultValueUtilizationCount;

        /// <summary>
        /// Sets and gets the most stringent peak of the sample.
        /// </summary>
        private I _mostStringentPeak;

        /// <summary>
        /// Sets and gets the most permissive peak of the sample.
        /// </summary>
        private I _mostPermissivePeak;

        /// <summary>
        /// Sets and gets the sum of all determined p-values. 
        /// It will be used to estimate average p-value of the sample.
        /// </summary>
        private double _pValueSum;

        private readonly IPeakConstructor<I> _constructor;

        /// <summary>
        /// Sets and gets the default p-value that Will be used as a region's p-value if the 
        /// region in source file has an invalid p-value and 'DropPeakIfNoPValueIsGiven = false'.
        /// </summary>
        public double DefaultValue { set; get; }

        /// <summary>
        /// Sets and gets if a region with invalid p-value should be 
        /// read (using default p-value) or dropped. Therefore, if set
        /// to true, any peak that has invalid p-value will be dropped. 
        /// </summary>
        public bool DropPeakIfInvalidValue { set; get; }

        /// <summary>
        /// Sets and gets the p-value conversion.
        /// </summary>
        public PValueFormats PValueFormat { set; get; }

        /// <summary>
        /// Sets and gets if the parser should assert if 
        /// a determined p-value falls in [0, 1] range.
        /// </summary>
        public bool ValidatePValue { set; get; }

        /// <summary>
        /// Parse standard Browser Extensible Data (BED) format.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        public BedParser(BedColumns columns, IPeakConstructor<I> constructor) : base(columns)
        {
            _constructor = constructor;
            _nameColumn = columns.Name;
            _valueColumn = columns.Value;
            _summitColumn = columns.Summit;
            _mostStringentPeak = _constructor.Construct(0, 2, 1);
            _mostPermissivePeak = _constructor.Construct(0, 2, 0);
            DefaultValue = 1E-8;
            DropPeakIfInvalidValue = true;
            ValidatePValue = true;
            PValueFormat = PValueFormats.SameAsInput;
        }

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter, string hashSeed)
        {
            #region .::.     Process p-value         .::.

            double value = 0;
            if (_valueColumn < line.Length)
            {
                if (double.TryParse(line[_valueColumn], out double pValue))
                {
                    value = PValueConvertor(pValue);
                }
                else if (DropPeakIfInvalidValue)
                {
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid p-value ( " + line[_valueColumn] + " )");
                }
                else
                {
                    value = DefaultValue;
                    _defaultValueUtilizationCount++;
                }
            }
            else
            {
                if (DropPeakIfInvalidValue)
                {
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid p-value column number");
                }
                else
                {
                    value = DefaultValue;
                    _defaultValueUtilizationCount++;
                }
            }

            if (ValidatePValue && (value < 0 || value > 1))
                DropLine("\tLine " + lineCounter.ToString() + string.Format("\t:\tInvalid p-value ({0})", value));

            #endregion

            if (!(_summitColumn != -1 && _summitColumn < line.Length && int.TryParse(line[_summitColumn], out int summit)))
                summit = (int)Math.Round((left + right) / 2.0);

            I rtv = _constructor.Construct(
                left, 
                right,
                value,
                _nameColumn < line.Length ? line[_nameColumn] : null,
                summit,
                hashSeed);

            if (_valueColumn < line.Length && !double.IsNaN(value))
            {
                _pValueSum += value;

                if (value > _mostPermissivePeak.Value)
                    _mostPermissivePeak = rtv;

                if (value < _mostStringentPeak.Value)
                    _mostStringentPeak = rtv;
            }

            return rtv;
        }

        public Bed<I> Parse(string sourceFilePath)
        {
            var rtv = (Bed<I>)Parse(sourceFilePath, new Bed<I>());

            if (_defaultValueUtilizationCount > 0)
                Messages.Insert(0, "\tDefault p-value used for " + _defaultValueUtilizationCount.ToString() + " times");

            rtv.PValueMean = _pValueSum / rtv.IntervalsCount;
            rtv.PValueMin = _mostStringentPeak;
            rtv.PValueMax = _mostPermissivePeak;

            Status = "100";
            return rtv;
        }

        /// <summary>
        /// Converts the p-value presented in (-1)Log10 or (-10)Log10 to original format.
        /// The conversion is done based on conversion option which specifies whether the input is in (-1)Log10(p-value) or (-10)Log10(p-value) format.
        /// </summary>
        /// <param name="value">The p-value in (-1)Log10 or (-10)Log10 format</param>
        /// <returns>The converted p-value if the conversion type is valid, otherwise it returns 0</returns>
        private double PValueConvertor(double value)
        {
            switch (PValueFormat)
            {
                case PValueFormats.minus1_Log10_pValue: return Math.Pow(10.0, value / (-1.0));
                case PValueFormats.minus10_Log10_pValue: return Math.Pow(10.0, value / (-10.0));
                case PValueFormats.minus100_Log10_pValue: return Math.Pow(10.0, value / (-100.0));
                case PValueFormats.SameAsInput:
                default: return value;
            }
        }
    }
}
