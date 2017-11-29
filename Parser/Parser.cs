// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Genometric.GeUtilities.Parsers
{
    public abstract class Parser<I, M>
        where I : IInterval<int, M>, new()
        where M : IExtendedMetadata, new()
    {
        /// <summary>
        /// decimal places/precision of numbers when rounding them. It is used for 
        /// chromosome-wide stats estimation.
        /// </summary>
        public byte decimalPlaces;

        /// <summary>
        /// Sets the number of lines to be read from input file.
        /// The default value is 4,294,967,295 (0xFFFFFFFF) which will be used if not set. 
        /// </summary>
        public UInt32 maxLinesToBeRead;
        public List<string> excessChrs;
        public List<string> missingChrs;

        #region .::.         Status variable and it's event controlers   .::.

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnStatusValueChaned(value);
                }
            }
        }
        public event EventHandler<ParserEventArgs> StatusChanged;
        private void OnStatusValueChaned(string value)
        {
            StatusChanged?.Invoke(this, new ParserEventArgs(value));
        }

        #endregion

        #region .::.         Protected Members                           .::.

        /// <summary>
        /// Full path of source file name.
        /// </summary>
        protected string _source { set; get; }

        /// <summary>
        /// This parameter will be used for initializing the chromosome count and sex chromosomes mappings.
        /// </summary>
        protected Genomes _genome { set; get; }

        /// <summary>
        /// 
        /// </summary>
        protected Assemblies _assembly { set; get; }

        /// <summary>
        /// If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. 
        /// </summary>
        protected byte _startOffset { set; get; }

        /// <summary>
        /// Sets and gets the coloumn number of chromosome name.
        /// </summary>
        protected byte _chrColumn { set; get; }

        /// <summary>
        /// Sets and gets column number of peak left position.
        /// </summary>
        protected byte _leftColumn { set; get; }

        /// <summary>
        /// Sets and gets the column number of peak right position.
        /// </summary>
        protected byte _rightColumn { set; get; }

        /// <summary>
        /// Sets and gets the column number of strand.
        /// </summary>
        protected sbyte _strandColumn { set; get; }

        /// <summary>
        /// It needs to be set to true if an unsuccessful attemp to read a region is occured, 
        /// such as invalid chromosome number. If the value is true, the reading peak will 
        /// not be stored. 
        /// </summary>
        protected bool _dropLine { set; get; }

        /// <summary>
        /// 
        /// </summary>
        protected List<string> _messages { set; get; }

        protected bool _readOnlyValidChrs { set; get; }

        internal ParsingType _parsingType { set; get; }

        internal HashFunction _hashFunction { set; get; }
        #endregion

        #region .::.         Private Members                             .::.

        /// <summary>
        /// Holds catched information of each chromosome's base pairs count. 
        /// This information will be updated based on the selected species.
        /// </summary>
        private Dictionary<string, int> _basePairsCount { set; get; }

        /// <summary>
        /// When read process is finished, this variable contains the number
        /// of dropped regions.
        /// </summary>
        private UInt16 _dropedLinesCount { set; get; }

        /// <summary>
        /// Contains all read information from the input file, and 
        /// will be retured as parser result.
        /// </summary>
        private ParsedIntervals<int, I, M> _data { set; get; }

        /// <summary>
        /// Sets and gets chromosome-wide information and statistics of the sample.
        /// </summary>
        private Dictionary<string, ChrStatistics> _chrs { set; get; }

        /// <summary>
        /// Sets and gets chromosome-wide the sum of width of all read intervals.
        /// </summary>
        private Dictionary<string, uint> _pw__Sum { set; get; }

        /// <summary>
        /// Sets and gets chromosome-wide the mean of width of all read intervals.
        /// </summary>
        private Dictionary<string, double> _pw_Mean { set; get; }

        /// <summary>
        /// Sets and gets chromosome-wide the standard deviation of width of all read intervals.
        /// </summary>
        private Dictionary<string, double> _pw_STDV { set; get; }

        /// <summary>
        /// Holds the p-value sum of all read intervals chromosome wide.
        /// </summary>
        private Dictionary<string, double> _pV__Sum { set; get; }

        /// <summary>
        /// Holds the p-value mean of all read intervals chromosome wide.
        /// </summary>
        private Dictionary<string, double> _pV_Mean { set; get; }

        /// <summary>
        /// Hold the p-value standard deviation of all read intervals chromosome wide
        /// </summary>
        private Dictionary<string, double> _pV_STDV { set; get; }

        private const UInt32 _FNVPrime_32 = 16777619;
        private const UInt32 _FNVOffsetBasis_32 = 2166136261;

        #endregion


        protected void Initialize()
        {
            /// Factory
            switch (_parsingType)
            {
                case ParsingType.ChIPseq:
                    _data = new ParsedChIPseqPeaks<int, I, M>();
                    break;

                case ParsingType.RefSeq:
                    _data = new ParsedRefSeqGenes<int, I, M>();
                    break;

                case ParsingType.GeneralFeatures:
                    _data = new ParsedGeneralFeatures<int, I, M>();
                    break;

                case ParsingType.VCF:
                    _data = new ParsedVariants<int, I, M>();
                    break;
            }

            _data.filePath = Path.GetFullPath(_source);
            _data.fileName = Path.GetFileName(_source);
            _data.fileHashKey = GetFileHashKey(_data.filePath);
            _data.genome = _genome;
            _data.assembly = _assembly;
            _basePairsCount = GenomeAssemblies.Assembly(_assembly);
            _chrs = new Dictionary<string, ChrStatistics>();
            _pw__Sum = new Dictionary<string, uint>();
            _pw_Mean = new Dictionary<string, double>();
            _pw_STDV = new Dictionary<string, double>();
            _pV__Sum = new Dictionary<string, double>();
            _pV_Mean = new Dictionary<string, double>();
            _pV_STDV = new Dictionary<string, double>();

            excessChrs = new List<string>();
            missingChrs = new List<string>();
        }

        /// <summary>
        /// Reads the regions presented in source file; and generates chromosome-wide statistics.
        /// </summary>
        /// <returns>Returns parsed intervals.</returns>
        protected ParsedIntervals<int, I, M> PARSE()
        {
            int left = 0;
            int right = 0;
            string line;
            UInt32 lineCounter = 0;
            string intervalName = "";
            _messages = new List<string>();
            _dropLine = false;

            if (File.Exists(_source))
            {
                FileInfo fileInfo = new FileInfo(_source);
                long fileSize = fileInfo.Length;
                int lineSize = 0;
                string chrTitle = "";
                char strand = '*';

                using (StreamReader fileReader = new StreamReader(_source))
                {
                    while (_startOffset-- > 0)
                    {
                        line = fileReader.ReadLine();
                        lineCounter++;
                    }

                    while ((line = fileReader.ReadLine()) != null)
                    {
                        lineCounter++;
                        lineSize += fileReader.CurrentEncoding.GetByteCount(line);
                        Status = (Math.Round((lineSize * 100.0) / fileSize, 0)).ToString();

                        if (line.Trim().Length > 0 && lineCounter <= maxLinesToBeRead)
                        {
                            _dropLine = false;
                            string[] splittedLine = line.Split('\t');
                            I readingInterval = ParseLine(splittedLine, lineCounter, out intervalName);
                            if (_dropLine)
                            {
                                DropLine("");
                                continue;
                            }

                            #region .::.     Determine Start/Stop      .::.

                            if (_parsingType == ParsingType.VCF)
                            {
                                if(_leftColumn < splittedLine.Length)
                                {
                                    if(int.TryParse(splittedLine[_leftColumn], out left))
                                    {
                                        readingInterval.Left = left;
                                        readingInterval.Right = left + 1;
                                    }
                                    else
                                    {
                                        DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid position column number");
                                        continue;
                                    }
                                }
                                else
                                {
                                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid position column number");
                                    continue;
                                }
                            }
                            else
                            {
                                if (_leftColumn < splittedLine.Length && _rightColumn < splittedLine.Length)
                                {
                                    if (int.TryParse(splittedLine[_leftColumn], out left) &&
                                        int.TryParse(splittedLine[_rightColumn], out right))
                                    {
                                        readingInterval.Left = left;
                                        readingInterval.Right = right;
                                    }
                                    else
                                    {
                                        DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid Start\\Stop column number");
                                        continue;
                                    }
                                }
                                else
                                {
                                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid Start\\Stop column number");
                                    continue;
                                }
                            }

                            #endregion
                            #region .::.     Determine chromosome      .::.


                            if (_parsingType == ParsingType.VCF)
                            {
                                chrTitle = "chr" + splittedLine[_chrColumn];
                            }
                            else
                            {
                                if (_chrColumn < splittedLine.Length &&
                                    Regex.IsMatch(splittedLine[_chrColumn].ToLower(), "chr"))
                                {
                                    chrTitle = splittedLine[_chrColumn];//.ToLower();
                                }
                                else
                                {
                                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome number ( " + splittedLine[_chrColumn].ToString() + " )");
                                    continue;
                                }
                            }

                            if (_readOnlyValidChrs && !_basePairsCount.ContainsKey(chrTitle)) //FixChrCasing(chrTitle)))
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome number ( " + splittedLine[_chrColumn].ToString() + " )");
                                continue;
                            }
                            #endregion
                            #region .::.     Determine Strand          .::.

                            strand = '*';
                            if (_strandColumn != -1 && _strandColumn < line.Length)
                                if (char.TryParse(splittedLine[_strandColumn], out strand))
                                    if (strand != '+' && strand != '-' && strand != '*')
                                        strand = '*';

                            #endregion

                            if (!_data.intervals.ContainsKey(chrTitle))
                                AddNewChromosome(chrTitle, strand);
                            else if (!_data.intervals[chrTitle].ContainsKey(strand))
                                _data.intervals[chrTitle].Add(strand, new List<I>());

                            switch (_hashFunction)
                            {
                                case HashFunction.One_at_a_Time:
                                default:
                                    readingInterval.HashKey = OneAtATimeHashFunction(readingInterval, lineCounter);
                                    break;

                                case HashFunction.FNV:
                                    readingInterval.HashKey = FNVHashFunction(readingInterval, lineCounter);
                                    break;
                            }


                            //readingInterval.Metadata.HashKey = readingInterval.HashKey;

                            _data.intervals[chrTitle][strand].Add(readingInterval);

                            _chrs[chrTitle].count++;

                            #region .::.     Peak width surveys    .::.

                            if (readingInterval.Right - readingInterval.Left > _chrs[chrTitle].peakWidthMax)
                                _chrs[chrTitle].peakWidthMax = (uint)(readingInterval.Right - readingInterval.Left);

                            if (readingInterval.Right - readingInterval.Left < _chrs[chrTitle].peakWidthMin)
                                _chrs[chrTitle].peakWidthMin = (uint)(readingInterval.Right - readingInterval.Left);

                            _pw__Sum[chrTitle] = (uint)(_pw__Sum[chrTitle] + (readingInterval.Right - readingInterval.Left));

                            #endregion
                            #region .::.     p-value surveys       .::.

                            if (!double.IsNaN(readingInterval.Metadata.Value))
                            {
                                if (readingInterval.Metadata.Value > _chrs[chrTitle].pValueMax)
                                    _chrs[chrTitle].pValueMax = readingInterval.Metadata.Value;

                                if (readingInterval.Metadata.Value < _chrs[chrTitle].pValueMin)
                                    _chrs[chrTitle].pValueMin = readingInterval.Metadata.Value;

                                _pV__Sum[chrTitle] += readingInterval.Metadata.Value;
                            }

                            #endregion
                        }
                    }
                }

                if (_dropedLinesCount > 0)
                    _messages.Insert(0, "\t" + _dropedLinesCount.ToString() + " Lines droped");
                _data.messages = _messages;

                EstimateStatistics();
                ReadMissingAndExcessChrs();
            }
            else
            {
                // throw an exception to inform that the file is not present
            }

            return _data;
        }

        /// <summary>
        /// Parses the line for necessary information. 
        /// <para>Chromosome, Left-end, and Right-end will be 
        /// determined and added to returned value.</para>
        /// <para>Set the parameter _dropLine to true if
        /// parsing is unsuccessful.</para>
        /// </summary>
        /// <param name="line">The splitted line read from input.</param>
        /// <returns>The interval this line delegates.</returns>
        protected abstract I ParseLine(string[] line, UInt32 lineCounter, out string intervalName);

        private void EstimateStatistics()
        {
            uint totalPeakCount = 0;

            #region .::.     chromosome Statistics preparation       .::.

            foreach (KeyValuePair<string, ChrStatistics> entry in _chrs)
            {
                totalPeakCount += (uint)entry.Value.count;

                if ((uint)entry.Value.count != 0)
                {
                    _pw_Mean[entry.Key] = Math.Round(_pw__Sum[entry.Key] / (double)_chrs[entry.Key].count, decimalPlaces);
                    _pV_Mean[entry.Key] = _pV__Sum[entry.Key] / (double)_chrs[entry.Key].count;
                }
                else
                {
                    _pw_Mean[entry.Key] = 0;
                    _pV_Mean[entry.Key] = 0;
                }
            }

            foreach (KeyValuePair<string, ChrStatistics> entry in _chrs)
                foreach (var strand in _data.intervals[entry.Key])
                    foreach (I interval in strand.Value)
                    {
                        _pw_STDV[entry.Key] += Math.Pow((interval.Right - interval.Left) - _pw_Mean[entry.Key], 2.0);
                        _pV_STDV[entry.Key] += Math.Pow(interval.Metadata.Value - _pV_Mean[entry.Key], 2.0);
                    }

            foreach (KeyValuePair<string, ChrStatistics> entry in _chrs)
            {
                if ((double)entry.Value.count != 0)
                {
                    _pw_STDV[entry.Key] = Math.Sqrt(_pw_STDV[entry.Key] / (double)entry.Value.count);
                    _pV_STDV[entry.Key] = Math.Sqrt(_pV_STDV[entry.Key] / (double)entry.Value.count);
                }
                else
                {
                    _pw_STDV[entry.Key] = 0;
                    _pV_STDV[entry.Key] = 0;
                }
            }

            #endregion

            if (totalPeakCount > 0)
            {
                #region -_-        Store chromosome wide statistics         -_-

                foreach (KeyValuePair<string, ChrStatistics> entry in _chrs)
                {
                    float _coverage = float.NaN;
                    if (_basePairsCount.ContainsKey(entry.Key)) //FixChrCasing(entry.Key)))
                        _coverage = (float)Math.Round(((int)_pw__Sum[entry.Key] * 100.0) / _basePairsCount[entry.Key], decimalPlaces); //FixChrCasing(entry.Key)], decimalPlaces);

                    if (entry.Value.peakWidthMin == uint.MaxValue) entry.Value.peakWidthMin = 0;

                    if (entry.Value.pValueMin == 1) entry.Value.pValueMin = 0;

                    _data.chrStatistics.Add(entry.Key, new ChrStatistics()
                    {
                        chrTitle = entry.Key, //FixChrCasing(entry.Key),
                        count = entry.Value.count,
                        percentage = (Math.Round((double)((entry.Value.count * 100) / totalPeakCount), decimalPlaces)).ToString() + " %",

                        peakWidthMax = entry.Value.peakWidthMax,
                        peakWidthMin = entry.Value.peakWidthMin,
                        peakWidthMean = Math.Round((float)_pw_Mean[entry.Key], decimalPlaces),
                        peakWidthSTDV = Math.Round((float)_pw_STDV[entry.Key], decimalPlaces),

                        pValueMax = entry.Value.pValueMax,
                        pValueMin = entry.Value.pValueMin,
                        pValueMean = _pV_Mean[entry.Key],
                        pValueSTDV = _pV_STDV[entry.Key],

                        coverage = _coverage
                    });
                }

                #endregion

                _data.intervalsCount = Convert.ToInt32(totalPeakCount);
            }
        }
        private void ReadMissingAndExcessChrs()
        {
            foreach (KeyValuePair<string, ChrStatistics> entry in _chrs)
                if (!_basePairsCount.ContainsKey(entry.Key)) //FixChrCasing(entry.Key)))
                    excessChrs.Add(entry.Key); //FixChrCasing(entry.Key));

            foreach (KeyValuePair<string, int> entry in _basePairsCount)
                if (!_chrs.ContainsKey(entry.Key.ToLower()))
                    missingChrs.Add(entry.Key);
        }

        private void AddNewChromosome(string newChr, char newStrand)
        {
            _data.intervals.Add(newChr, new Dictionary<char, List<I>>());
            _data.intervals[newChr].Add(newStrand, new List<I>());
            _chrs.Add(newChr, new ChrStatistics()
            {
                chrTitle = newChr,
                count = 0,
                percentage = "0 %",

                pValueMax = 0,
                pValueMin = 1,
                pValueMean = 0,
                pValueSTDV = 0,

                peakWidthMax = 0,
                peakWidthMin = uint.MaxValue,
                peakWidthMean = 0,
                peakWidthSTDV = 0,

                coverage = 0,
            });

            _pw__Sum.Add(newChr, 0);
            _pw_Mean.Add(newChr, 0.0);
            _pw_STDV.Add(newChr, 0.0);
            _pV__Sum.Add(newChr, 0.0);
            _pV_Mean.Add(newChr, 0.0);
            _pV_STDV.Add(newChr, 0.0);
        }

        /// <summary>
        /// Returns case-fixed chr title. 
        /// <para>During parsing process all chr title are changed to
        /// lower-case. However, it is prefered to have chrX, chrY and 
        /// chrM in camel casing. This function will do conversion.</para>
        /// </summary>
        /// <param name="allLowerCaseChr">camel-cased chr titles.</param>
        /// <returns></returns>
        /*private string FixChrCasing(string allLowerCaseChr)
        {
            switch (allLowerCaseChr)
            {
                case "chrx": return "chrX";
                case "chry": return "chrY";
                case "chrm": return "chrM";
                default: return allLowerCaseChr;
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private UInt32 GetFileHashKey(string file)
        {
            int len = file.Length;

            UInt32 hashKey = 0;
            for (int i = 0; i < len; i++)
            {
                hashKey += file[i];
                hashKey += (hashKey << 10);
                hashKey ^= (hashKey >> 6);
            }

            hashKey += (hashKey << 3);
            hashKey ^= (hashKey >> 11);
            hashKey += (hashKey << 15);

            return hashKey;
        }

        /// <summary>
        /// Returns hash key based on One-at-a-Time method
        /// generated based on Dr. Dobb's left methods.
        /// </summary>
        /// <returns>Hashkey of the interval.</returns>
        private UInt32 OneAtATimeHashFunction(I readingPeak, UInt32 lineNo)
        {
            string key = _data.fileHashKey + "_" + readingPeak.Left.ToString() + "_" + readingPeak.Right.ToString() + "_" + lineNo.ToString();
            int len = key.Length;

            UInt32 hashKey = 0;
            for (int i = 0; i < len; i++)
            {
                hashKey += key[i];
                hashKey += (hashKey << 10);
                hashKey ^= (hashKey >> 6);
            }

            hashKey += (hashKey << 3);
            hashKey ^= (hashKey >> 11);
            hashKey += (hashKey << 15);

            return hashKey;
        }
        private UInt32 FNVHashFunction(I readingPeak, UInt32 lineNo)
        {
            string key = _data.fileHashKey + "_" + readingPeak.Left.ToString() + "_" + readingPeak.Right.ToString() + "_" + lineNo.ToString();
            UInt32 hash = _FNVOffsetBasis_32;
            for (var i = 0; i < key.Length; i++)
            {
                hash = hash ^ key[i]; // exclusive OR
                hash *= _FNVPrime_32;
            }

            return hash;
        }

        protected void DropLine(string message)
        {
            _messages.Add(message);
            _dropedLinesCount++;
        }
    }
}
