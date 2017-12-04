// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class BEDParser<I> : Parser<I, BEDStats>
        where I : IChIPSeqPeak, new()
    {
        /// <summary>
        /// Parse standard Browser Extensible Data (BED) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        public BEDParser(
            string source,
            Genomes genome,
            Assemblies assembly,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue) :
            this(
                source: source,
                genome: genome,
                assembly: assembly,
                readOnlyValidChrs: readOnlyValidChrs,
                startOffset: 0,
                chrColumn: 0,
                leftEndColumn: 1,
                rightEndColumn: 2,
                nameColumn: 3,
                valueColumn: 4,
                summitColumn: -1,
                strandColumn: -1,
                maxLinesToBeRead: maxLinesToBeRead)
        { }


        /// <summary>
        /// Parse standard Browser Extensible Data (BED) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
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
            Genomes genome,
            Assemblies assembly,
            sbyte chrColumn,
            sbyte leftEndColumn,
            sbyte rightEndColumn,
            byte nameColumn,
            byte valueColumn,
            sbyte summitColumn,
            sbyte strandColumn,
            double defaultValue = 1E-8,
            PValueFormat pValueFormat = PValueFormat.SameAsInput,
            bool dropPeakIfInvalidValue = true,
            byte startOffset = 0,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            base(source: source,
                genome: genome,
                assembly: assembly,
                startOffset: startOffset,
                chrColumn: chrColumn,
                leftEndColumn: leftEndColumn,
                rightEndColumn: rightEndColumn,
                strandColumn: strandColumn,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToBeRead: maxLinesToBeRead,
                hashFunction: hashFunction)
        {
            _nameColumn = nameColumn;
            _valueColumn = valueColumn;
            _summitColumn = summitColumn;
            _defaultValue = defaultValue;
            _pValueFormat = pValueFormat;
            _dropPeakIfInvalidValue = dropPeakIfInvalidValue;
            _mostStringentPeak = new I();
            _mostPermissivePeak = new I();
            _mostStringentPeak.Value = 1;
            _mostPermissivePeak.Value = 0;
            _summitToMidRequired = false;
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

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter, out string intervalName)
        {
            I rtv = new I();

            #region .::.     Process p-value         .::.

            if (_valueColumn < line.Length)
            {
                if (double.TryParse(line[_valueColumn], out double pValue))
                {
                    rtv.Value = PValueConvertor(pValue);
                }
                else if (_dropPeakIfInvalidValue == true)
                {
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid p-value ( " + line[_valueColumn] + " )");
                }
                else
                {
                    rtv.Value = _defaultValue;
                    _defaultValueUtilizationCount++;
                }

                if (!double.IsNaN(rtv.Value))
                {
                    _pValueSum += rtv.Value;

                    if (rtv.Value > _mostPermissivePeak.Value)
                        _mostPermissivePeak = rtv;

                    if (rtv.Value < _mostStringentPeak.Value)
                        _mostStringentPeak = rtv;
                }
            }
            else
            {
                if (_dropPeakIfInvalidValue == true)
                {
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid p-value column number");
                }
                else
                {
                    rtv.Value = _defaultValue;
                    _defaultValueUtilizationCount++;
                }
            }

            #endregion
            #region .::.     Process I Name          .::.

            if (_nameColumn < line.Length) rtv.Name = line[_nameColumn];
            else rtv.Name = "null";
            intervalName = rtv.Name;

            #endregion
            #region .::.     Process Summit          .::.

            int summit = 0;
            if (_summitColumn != -1 && _summitColumn < line.Length)
                if (int.TryParse(line[_summitColumn], out summit))
                    rtv.Summit = summit;
                else { rtv.Summit = -1; _summitToMidRequired = true; }
            else { rtv.Summit = -1; _summitToMidRequired = true; }

            #endregion

            return rtv;
        }

        public ParsedChIPseqPeaks<I> Parse()
        {
            var rtv = (ParsedChIPseqPeaks<I>)PARSE();

            if (_defaultValueUtilizationCount > 0)
                Messages.Insert(0, "\tDefault p-value used for " + _defaultValueUtilizationCount.ToString() + " times");

            rtv.pValueMean = _pValueSum / rtv.IntervalsCount;
            rtv.pValueMin = _mostStringentPeak;
            rtv.pValueMax = _mostPermissivePeak;

            if (_summitToMidRequired)
                foreach (var chr in rtv.Chromosomes)
                    foreach (var strand in chr.Value.Strands)
                        foreach (var peak in strand.Value.intervals)
                            if (peak.Summit == -1)
                                peak.Summit = (int)Math.Round((peak.Left + peak.Right) / 2.0);

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
