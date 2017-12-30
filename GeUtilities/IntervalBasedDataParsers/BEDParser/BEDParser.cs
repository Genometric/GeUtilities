// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.ReferenceGenomes;
using System;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class BEDParser<I> : Parser<I, BEDStats>
        where I : IChIPSeqPeak, new()
    {
        #region .::.         private properties         .::.

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
        /// If set to true, any peak that has invalid p-value will be dropped. 
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

        #endregion

        /// <summary>
        /// Parse standard Browser Extensible Data (BED) format.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        public BEDParser(
            string sourceFilePath,
            Assemblies assembly = Assemblies.Unknown,
            double defaultValue = 1E-8,
            PValueFormat pValueFormat = PValueFormat.SameAsInput,
            bool dropPeakIfInvalidValue = true,
            byte startOffset = 0,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            this(
                sourceFilePath: sourceFilePath,
                chrColumn: 0,
                leftEndColumn: 1,
                rightEndColumn: 2,
                nameColumn: 3,
                valueColumn: 4,
                strandColumn: -1,
                summitColumn: -1,
                assembly: assembly,
                defaultValue: defaultValue,
                pValueFormat: pValueFormat,
                dropPeakIfInvalidValue: dropPeakIfInvalidValue,
                startOffset: startOffset,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToBeRead: maxLinesToBeRead,
                hashFunction: hashFunction)
        { }


        /// <summary>
        /// Parse standard Browser Extensible Data (BED) format.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        /// <param name="startOffset">If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. </param>
        /// <param name="chrColumn">The column number of chromosome name.</param>
        /// <param name="leftEndColumn">The column number of peak left-end position.</param>
        /// <param name="rightEndColumn">The column number of peak right-end position.</param>
        /// <param name="nameColumn">The column number of peak name.</param>
        /// <param name="valueColumn">The column number of peak value.</param>
        /// <param name="summitColumn">The column number of peak summit. If summit is not available, set this value to -1 so that the summit will the mid point of the interval.</param>
        /// <param name="strandColumn">The column number of peak strand. If input is not stranded this value should be set to -1.</param>
        /// <param name="defaultValue">Default value of a peak. It will be used in case 
        /// invalid value is read from source.</param>
        /// <param name="pValueFormat">It specifies the value conversion option:
        /// <para>0 : no conversion.</para>
        /// <para>1 : value = (-10)Log10(value)</para>
        /// <para>2 : value =  (-1)Log10(value)</para>
        /// <param name="dropPeakIfInvalidValue">If set to true, a peak with invalid value will be dropped. 
        /// If set to false, a peak with invalid value with take up the default value.</param>
        public BEDParser(
            string sourceFilePath,
            byte chrColumn,
            byte leftEndColumn,
            sbyte rightEndColumn,
            byte nameColumn,
            byte valueColumn,
            sbyte strandColumn = -1,
            sbyte summitColumn = -1,
            Assemblies assembly = Assemblies.Unknown,
            double defaultValue = 1E-8,
            PValueFormat pValueFormat = PValueFormat.SameAsInput,
            bool dropPeakIfInvalidValue = true,
            byte startOffset = 0,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            base(sourceFilePath: sourceFilePath,
                startOffset: startOffset,
                chrColumn: chrColumn,
                leftEndColumn: leftEndColumn,
                rightEndColumn: rightEndColumn,
                strandColumn: strandColumn,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToBeRead: maxLinesToBeRead,
                hashFunction: hashFunction,
                data: new ParsedChIPseqPeaks<I>(),
                assembly: assembly)
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
        }

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter)
        {
            I rtv = new I
            {
                Left = left,
                Right = right
            };

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
            else rtv.Name = null;

            #endregion

            #region .::.     Process Summit          .::.

            if ((_summitColumn != -1 && _summitColumn < line.Length) && int.TryParse(line[_summitColumn], out int summit))
                rtv.Summit = summit;
            else
                rtv.Summit = (int)Math.Round((left + right) / 2.0);

            #endregion

            return rtv;
        }

        public new ParsedChIPseqPeaks<I> Parse()
        {
            var rtv = (ParsedChIPseqPeaks<I>)base.Parse();

            if (_defaultValueUtilizationCount > 0)
                Messages.Insert(0, "\tDefault p-value used for " + _defaultValueUtilizationCount.ToString() + " times");

            rtv.pValueMean = _pValueSum / rtv.IntervalsCount;
            rtv.pValueMin = _mostStringentPeak;
            rtv.pValueMax = _mostPermissivePeak;

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
            switch (_pValueFormat)
            {
                case PValueFormat.minus1_Log10_pValue: return Math.Pow(10.0, value / (-1.0));
                case PValueFormat.minus10_Log10_pValue: return Math.Pow(10.0, value / (-10.0));
                case PValueFormat.minus100_Log10_pValue: return Math.Pow(10.0, value / (-100.0));
                case PValueFormat.SameAsInput:
                default: return value;
            }
        }
    }
}
