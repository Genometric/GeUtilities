// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class BEDParser<I, M> : Parser<I, M>
        where I : IInterval<int, M>, new()
        where M : IChIPSeqPeak, new()
    {
        /// <summary>
        /// Parse standard Browser Extensible Data (BED) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="species">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        public BEDParser(
            string source,
            Genomes species,
            Assemblies assembly,
            bool readOnlyValidChrs)
        {
            InitializeDefaultValues();
            _source = source;
            _genome = species;
            _assembly = assembly;
            _readOnlyValidChrs = readOnlyValidChrs;
            Initialize();
        }

        /// <summary>
        /// Parse standard Browser Extensible Data (BED) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="species">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        /// <param name="startOffset">If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. </param>
        /// <param name="chrColumn">The coloumn number of chromosome name.</param>
        /// <param name="leftEndColumn">The column number of peak left-end position.</param>
        /// <param name="rightEndColumn">The column number of peak right-end position.</param>
        /// <param name="nameColumn">The column number of peak name.</param>
        /// <param name="valueColumn">The column number of peak value.</param>
        /// <param name="summitColumn">The column number of peak summit. If summit is not available, set this value to -1 so that the summit will the mid point of the interval.</param>
        /// /// <param name="strandColumn">The column number of peak strand. If input is not stranded this value should be set to -1.</param>
        public BEDParser(
            string source,
            Genomes species,
            Assemblies assembly,
            bool readOnlyValidChrs,
            byte startOffset,
            byte chrColumn,
            byte leftEndColumn,
            byte rightEndColumn,
            byte nameColumn,
            byte valueColumn,
            sbyte summitColumn,
            sbyte strandColumn)
        {
            InitializeDefaultValues();
            _source = source;
            _genome = species;
            _assembly = assembly;
            _readOnlyValidChrs = readOnlyValidChrs;
            _startOffset = startOffset;
            _chrColumn = chrColumn;
            _leftColumn = leftEndColumn;
            _rightColumn = rightEndColumn;
            _nameColumn = nameColumn;
            _valueColumn = valueColumn;
            _summitColumn = summitColumn;
            _strandColumn = strandColumn;
            Initialize();
        }


        /// <summary>
        /// Parse standard Browser Extensible Data (BED) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="species">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        /// <param name="startOffset">If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. </param>
        /// <param name="chrColumn">The coloumn number of chromosome name.</param>
        /// <param name="leftEndColumn">The column number of peak left-end position.</param>
        /// <param name="rightEndColumn">The column number of peak right-end position.</param>
        /// <param name="nameColumn">The column number of peak name.</param>
        /// <param name="valueColumn">The column number of peak value.</param>
        /// <param name="summitColumn">The column number of peak summit. If summit is not available, set this value to -1 so that the summit will the mid point of the interval.</param>
        /// <param name="strandColumn">The column number of peak strand. If input is not stranded this value should be set to -1.</param>
        /// <param name="defaultValue">Default value of a peak. It will be used in case 
        /// invalid value is read from source.</param>
        /// <param name="pValueFormat">It specifies the value convertion option:
        /// <para>0 : no convertion.</para>
        /// <para>1 : value = (-10)Log10(value)</para>
        /// <para>2 : value =  (-1)Log10(value)</para>
        /// <param name="dropPeakIfInvalidValue">If set to true, a peak with invalid value will be droped. 
        /// If set to false, a peak with invalid value with take up the default value.</param>
        public BEDParser(
            string source,
            Genomes species,
            Assemblies assembly,
            bool readOnlyValidChrs,
            byte startOffset,
            byte chrColumn,
            byte leftEndColumn,
            byte rightEndColumn,
            byte nameColumn,
            byte valueColumn,
            sbyte summitColumn,
            sbyte strandColumn,
            double defaultValue,
            PValueFormat pValueFormat,
            bool dropPeakIfInvalidValue, 
            HashFunction hashFunction)
        {
            InitializeDefaultValues();
            _source = source;
            _genome = species;
            _assembly = assembly;
            _readOnlyValidChrs = readOnlyValidChrs;
            _startOffset = startOffset;
            _chrColumn = chrColumn;
            _leftColumn = leftEndColumn;
            _rightColumn = rightEndColumn;
            _nameColumn = nameColumn;
            _valueColumn = valueColumn;
            _summitColumn = summitColumn;
            _strandColumn = strandColumn;
            _defaultValue = defaultValue;
            _pValueFormat = pValueFormat;
            _dropPeakIfInvalidValue = dropPeakIfInvalidValue;
            _hashFunction = hashFunction;
            Initialize();
        }


        #region .::.         private Variables declaration               .::.

        /// <summary>
        /// Sets and gets the column number of peak name.
        /// </summary>
        private byte _nameColumn;

        /// <summary>
        /// Sets and gets the column number of p-value.
        /// </summary>
        private byte _valueColumn;

        /// <summary>
        /// Sets and gets the column number of peak summit.
        /// </summary>
        private sbyte _summitColumn;

        /// <summary>
        /// Sets and gets the default p-value that Will be used as region's p-value if the 
        /// region in source file has an invalid p-value and 'drop_Peak_if_no_p_Value_is_given = false'
        /// </summary>
        private double _defaultValue;

        /// <summary>
        /// Sets and gets the p-value conversion will be based on this parameter.
        /// </summary>
        private PValueFormat _pValueFormat;

        /// <summary>
        /// If set to true, any peak that has invalid p-value will be droped. 
        /// </summary>
        private bool _dropPeakIfInvalidValue;

        /// <summary>
        /// When read process is finished, this variable contains the number
        /// of regions that contained invalid p-value and replaced by default p-value. 
        /// </summary>
        private UInt16 _defaultValueUtilizationCount;

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

        /// <summary>
        /// The variable is set to true if at least one peak misses summit. 
        /// In that case, the function will go through all the peaks after 
        /// they are parsed, and replaces -1 summit by mid-point.
        /// </summary>
        private bool _summitToMidRequired;

        #endregion


        private void InitializeDefaultValues()
        {
            maxLinesToBeRead = uint.MaxValue;
            _chrColumn = 0;
            _leftColumn = 1;
            _rightColumn = 2;
            _nameColumn = 3;
            _valueColumn = 4;
            _summitColumn = -1;
            _strandColumn = -1;
            _defaultValue = 1E-8;
            _pValueFormat = PValueFormat.SameAsInput;
            _dropPeakIfInvalidValue = true;
            _mostStringentPeak = new I();
            _mostPermissivePeak = new I();
            _mostStringentPeak.Metadata = new M() { Value = 1 };
            _mostPermissivePeak.Metadata = new M() { Value = 0 };
            _parsingType = ParsingType.ChIPseq;
            _summitToMidRequired = false;
        }

        protected override I ParseLine(string[] line, UInt32 lineCounter, out string intervalName)
        {
            I rtv = new I();
            rtv.Metadata = new M();

            #region .::.     Process p-value         .::.

            if (_valueColumn < line.Length)
            {
                if (double.TryParse(line[_valueColumn], out double pValue))
                {
                    rtv.Metadata.Value = PValueConvertor(pValue);
                }
                else if (_dropPeakIfInvalidValue == true)
                {
                    _dropLine = true;
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid p-value ( " + line[_valueColumn] + " )");
                }
                else
                {
                    rtv.Metadata.Value = _defaultValue;
                    _defaultValueUtilizationCount++;
                }

                if (!double.IsNaN(rtv.Metadata.Value))
                {
                    _pValueSum += rtv.Metadata.Value;

                    if (rtv.Metadata.Value > _mostPermissivePeak.Metadata.Value)
                        _mostPermissivePeak = rtv;

                    if (rtv.Metadata.Value < _mostStringentPeak.Metadata.Value)
                        _mostStringentPeak = rtv;
                }
            }
            else
            {
                if (_dropPeakIfInvalidValue == true)
                {
                    _dropLine = true;
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid p-value column number");
                }
                else
                {
                    rtv.Metadata.Value = _defaultValue;
                    _defaultValueUtilizationCount++;
                }
            }

            #endregion
            #region .::.     Process I Name          .::.

            if (_nameColumn < line.Length) rtv.Metadata.Name = line[_nameColumn];
            else rtv.Metadata.Name = "null";
            intervalName = rtv.Metadata.Name;

            #endregion
            #region .::.     Process Summit          .::.

            int summit = 0;
            if (_summitColumn != -1 && _summitColumn < line.Length)
                if (int.TryParse(line[_summitColumn], out summit))
                    rtv.Metadata.Summit = summit;
                else { rtv.Metadata.Summit = -1; _summitToMidRequired = true; }
            else { rtv.Metadata.Summit = -1; _summitToMidRequired = true; }

            #endregion

            return rtv;
        }

        public ParsedChIPseqPeaks<int, I, M> Parse()
        {
            var rtv = (ParsedChIPseqPeaks<int, I, M>)PARSE();

            if (_defaultValueUtilizationCount > 0)
                _messages.Insert(0, "\tDefault p-value used for " + _defaultValueUtilizationCount.ToString() + " times");

            rtv.pValueMean = _pValueSum / rtv.intervalsCount;
            rtv.pValueMin = _mostStringentPeak;
            rtv.pValueMax = _mostPermissivePeak;

            if (_summitToMidRequired)
                foreach (var chr in rtv.intervals)
                    foreach (var strand in chr.Value)
                        foreach (var peak in strand.Value)
                            if (peak.Metadata.Summit == -1)
                                peak.Metadata.Summit = (int)Math.Round((peak.Left + peak.Right) / 2.0);

            Status = "100";
            return rtv;
        }


        /// <summary>
        /// Converts the p-value presented in (-1)Log10 or (-10)Log10 to original format.
        /// The conversion is done based on conversion option which specifies whether the input is in (-1)Log10(p-value) or (-10)Log10(p-value) format.
        /// </summary>
        /// <param name="value">The p-value in (-1)Log10 or (-10)Log10 format</param>
        /// <returns>The coverted p-value if the converstion type is valid, otherwise it returns 0</returns>
        private double PValueConvertor(double value)
        {
            switch (_pValueFormat)
            {
                case PValueFormat.SameAsInput: return value;
                case PValueFormat.minus1_Log10_pValue: return Math.Pow(10.0, value / (-1.0));
                case PValueFormat.minus10_Log10_pValue: return Math.Pow(10.0, value / (-10.0));
                case PValueFormat.minus100_Log10_pValue: return Math.Pow(10.0, value / (-100.0));
                default: return 0;
            }
        }
    }
}
