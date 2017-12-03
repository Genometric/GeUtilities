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
    public abstract class Parser<I, S>
        where I : IInterval<int>, new()
        where S : IStats<int>, new()
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
        protected string Source { set; get; }

        /// <summary>
        /// This parameter will be used for initializing the chromosome count and sex chromosomes mappings.
        /// </summary>
        protected Genomes Genome { set; get; }

        /// <summary>
        /// 
        /// </summary>
        protected Assemblies Assembly { set; get; }

        /// <summary>
        /// If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. 
        /// </summary>
        protected byte StartOffset { set; get; }

        /// <summary>
        /// Sets and gets the coloumn number of chromosome name.
        /// </summary>
        protected sbyte ChrColumn { set; get; }

        /// <summary>
        /// Sets and gets column number of peak left position.
        /// </summary>
        protected sbyte LeftColumn { set; get; }

        /// <summary>
        /// Sets and gets the column number of peak right position.
        /// </summary>
        protected sbyte RightColumn { set; get; }

        /// <summary>
        /// Sets and gets the column number of strand.
        /// </summary>
        protected sbyte StrandColumn { set; get; }

        /// <summary>
        /// Sets and gets validity of the interval being parsed.
        /// In other words, indicates if the required fields of 
        /// the interval are parsed correctly. For instance, if
        /// the chromosome attribute of the peak is read.
        /// </summary>
        private bool _dropReadingPeak;

        /// <summary>
        /// 
        /// </summary>
        protected List<string> Messages { set; get; }

        /// <summary>
        /// Contains all read information from the input file, and 
        /// will be retured as parser result.
        /// </summary>
        protected ParsedIntervals<I, S> Data { set; get; }

        protected bool ReadOnlyValidChrs { set; get; }

        internal HashFunction HashFunction { set; get; }

        #endregion

        #region .::.         Private Members                             .::.

        /// <summary>
        /// Holds catched information of each chromosome's base pairs count. 
        /// This information will be updated based on the selected species.
        /// </summary>
        private Dictionary<string, int> _basePairsCount;

        /// <summary>
        /// When read process is finished, this variable contains the number
        /// of dropped regions.
        /// </summary>
        private UInt16 _dropedLinesCount;

        /// <summary>
        /// Sets and gets chromosome-wide information and statistics of the sample.
        /// </summary>
        private Dictionary<string, BEDStats> _chrs;

        /// <summary>
        /// Sets and gets chromosome-wide the sum of width of all read intervals.
        /// </summary>
        private Dictionary<string, uint> _pw__Sum;

        /// <summary>
        /// Sets and gets chromosome-wide the mean of width of all read intervals.
        /// </summary>
        private Dictionary<string, double> _pw_Mean;

        /// <summary>
        /// Sets and gets chromosome-wide the standard deviation of width of all read intervals.
        /// </summary>
        private Dictionary<string, double> _pw_STDV;

        /// <summary>
        /// Holds the p-value sum of all read intervals chromosome wide.
        /// </summary>
        private Dictionary<string, double> _pV__Sum;

        /// <summary>
        /// Holds the p-value mean of all read intervals chromosome wide.
        /// </summary>
        private Dictionary<string, double> _pV_Mean;

        /// <summary>
        /// Hold the p-value standard deviation of all read intervals chromosome wide
        /// </summary>
        private Dictionary<string, double> _pV_STDV;

        private const UInt32 _FNVPrime_32 = 16777619;
        private const UInt32 _FNVOffsetBasis_32 = 2166136261;

        #endregion


        protected void Initialize()
        {
            Data.FilePath = Path.GetFullPath(Source);
            Data.FileName = Path.GetFileName(Source);
            Data.FileHashKey = GetFileHashKey(Data.FilePath);
            Data.Genome = Genome;
            Data.Assembly = Assembly;
            _basePairsCount = GenomeAssemblies.Assembly(Assembly);
            _chrs = new Dictionary<string, BEDStats>();
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
        protected ParsedIntervals<I, S> PARSE()
        {
            int left = 0;
            int right = 0;
            string line;
            UInt32 lineCounter = 0;
            string intervalName = "";
            Messages = new List<string>();
            _dropReadingPeak = false;

            if (File.Exists(Source))
            {
                FileInfo fileInfo = new FileInfo(Source);
                long fileSize = fileInfo.Length;
                int lineSize = 0;
                string chrName = "";
                char strand = '*';

                using (StreamReader fileReader = new StreamReader(Source))
                {
                    while (StartOffset-- > 0)
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
                            _dropReadingPeak = false;
                            string[] splittedLine = line.Split('\t');

                            if (!(LeftColumn < splittedLine.Length && int.TryParse(splittedLine[LeftColumn], out left)))
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid start/position column number");
                                continue;
                            }

                            if (RightColumn > 0 && !(RightColumn < splittedLine.Length && int.TryParse(splittedLine[RightColumn], out right)))
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid stop column number");
                                continue;
                            }

                            I readingInterval = BuildInterval(left, right, splittedLine, lineCounter, out intervalName);
                            if (_dropReadingPeak)
                                continue;

                            if (ChrColumn < splittedLine.Length)
                            {
                                if (Regex.IsMatch(splittedLine[ChrColumn].ToLower(), "chr"))
                                    chrName = splittedLine[ChrColumn];
                                else if (int.TryParse(splittedLine[ChrColumn], out int chrNumber))
                                    chrName = "chr" + chrNumber;
                                else
                                {
                                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome number ( " + splittedLine[ChrColumn].ToString() + " )");
                                    continue;
                                }
                            }
                            else
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome column number");
                                continue;
                            }

                            if (ReadOnlyValidChrs && !_basePairsCount.ContainsKey(chrName))
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome number ( " + splittedLine[ChrColumn].ToString() + " )");
                                continue;
                            }

                            strand = '*';
                            if (StrandColumn != -1 && StrandColumn < line.Length)
                                if (char.TryParse(splittedLine[StrandColumn], out strand))
                                    if (strand != '+' && strand != '-' && strand != '*')
                                        strand = '*';

                            switch (HashFunction)
                            {
                                case HashFunction.One_at_a_Time:
                                default:
                                    readingInterval.HashKey = OneAtATimeHashFunction(readingInterval, lineCounter);
                                    break;

                                case HashFunction.FNV:
                                    readingInterval.HashKey = FNVHashFunction(readingInterval, lineCounter);
                                    break;
                            }

                            Data.Add(readingInterval, chrName, strand);
                        }
                    }
                }

                if (_dropedLinesCount > 0)
                    Messages.Insert(0, "\t" + _dropedLinesCount.ToString() + " Lines droped");
                Data.Messages = Messages;

                ReadMissingAndExcessChrs();
            }
            else
            {
                // throw an exception to inform that the file is not present
            }

            return Data;
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
        protected abstract I BuildInterval(int left, int right, string[] line, UInt32 lineCounter, out string intervalName);


        private void ReadMissingAndExcessChrs()
        {
            foreach (KeyValuePair<string, BEDStats> entry in _chrs)
                if (!_basePairsCount.ContainsKey(entry.Key)) //FixChrCasing(entry.Key)))
                    excessChrs.Add(entry.Key); //FixChrCasing(entry.Key));

            foreach (KeyValuePair<string, int> entry in _basePairsCount)
                if (!_chrs.ContainsKey(entry.Key.ToLower()))
                    missingChrs.Add(entry.Key);
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
            string key = Data.FileHashKey + "_" + readingPeak.Left.ToString() + "_" + readingPeak.Right.ToString() + "_" + lineNo.ToString();
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
            string key = Data.FileHashKey + "_" + readingPeak.Left.ToString() + "_" + readingPeak.Right.ToString() + "_" + lineNo.ToString();
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
            Messages.Add(message);
            _dropReadingPeak = true;
            _dropedLinesCount++;
        }
    }
}
