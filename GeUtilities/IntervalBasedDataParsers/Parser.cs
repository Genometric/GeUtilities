﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ReadOnlyCollection<string> ExcessChrs { get { return _excessChrs.AsReadOnly(); } }
        private List<string> _excessChrs;

        public ReadOnlyCollection<string> MissingChrs { get { return _missingChrs.AsReadOnly(); } }
        private List<string> _missingChrs;

        private const UInt32 _FNVPrime_32 = 16777619;
        private const UInt32 _FNVOffsetBasis_32 = 2166136261;

        /// <summary>
        /// Full path of source file name.
        /// </summary>
        private string _source;

        /// <summary>
        /// This parameter will be used for initializing the chromosome count and sex chromosomes mappings.
        /// </summary>
        private Genomes _genome;

        /// <summary>
        /// 
        /// </summary>
        private Assemblies _assembly;

        /// <summary>
        /// If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. 
        /// </summary>
        private byte _startOffset;

        /// <summary>
        /// Sets and gets the coloumn number of chromosome name.
        /// </summary>
        private sbyte _chrColumn;

        /// <summary>
        /// Sets and gets column number of peak left position.
        /// </summary>
        private sbyte _leftColumn;

        /// <summary>
        /// Sets and gets the column number of peak right position.
        /// </summary>
        private sbyte _rightColumn;

        /// <summary>
        /// Sets and gets the column number of strand.
        /// </summary>
        private sbyte _strandColumn;

        /// <summary>
        /// Sets and gets validity of the interval being parsed.
        /// In other words, indicates if the required fields of 
        /// the interval are parsed correctly. For instance, if
        /// the chromosome attribute of the peak is read.
        /// </summary>
        private bool _dropReadingPeak;

        /// <summary>
        /// Sets the number of lines to be read from input file.
        /// The default value is 4,294,967,295 (0xFFFFFFFF) which will be used if not set. 
        /// </summary>
        private UInt32 _maxLinesToBeRead;

        /// <summary>
        /// When read process is finished, this variable contains the number
        /// of dropped regions.
        /// </summary>
        private UInt16 _dropedLinesCount;

        /// <summary>
        /// Holds catched information of each chromosome's base pairs count. 
        /// This information will be updated based on the selected species.
        /// </summary>
        private Dictionary<string, int> _assemblyData;

        /// <summary>
        /// Contains all read information from the input file, and 
        /// will be retured as parser result.
        /// </summary>
        private ParsedIntervals<I, S> _data;

        private bool _readOnlyValidChrs;

        private HashFunction _hashFunction;


        public Parser(
            string source,
            Genomes genome,
            Assemblies assembly,
            byte startOffset,
            sbyte chrColumn,
            sbyte leftEndColumn,
            sbyte rightEndColumn,
            sbyte strandColumn,
            bool readOnlyValidChrs,
            uint maxLinesToBeRead,
            HashFunction hashFunction)
        {
            _source = source;
            _genome = genome;
            _assembly = assembly;
            _startOffset = startOffset;
            _chrColumn = chrColumn;
            _leftColumn = leftEndColumn;
            _rightColumn = rightEndColumn;
            _strandColumn = strandColumn;
            _readOnlyValidChrs = readOnlyValidChrs;
            _maxLinesToBeRead = maxLinesToBeRead;
            _hashFunction = hashFunction;
            _data.FilePath = Path.GetFullPath(_source);
            _data.FileName = Path.GetFileName(_source);
            _data.FileHashKey = GetFileHashKey(_data.FilePath);
            _data.Genome = _genome;
            _data.Assembly = _assembly;
            _assemblyData = GenomeAssemblies.Assembly(_assembly);
            _excessChrs = new List<string>();
            _missingChrs = new List<string>();
        }


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


        /// <summary>
        /// 
        /// </summary>
        protected List<string> Messages { set; get; }

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

            if (File.Exists(_source))
            {
                FileInfo fileInfo = new FileInfo(_source);
                long fileSize = fileInfo.Length;
                int lineSize = 0;
                string chrName = "";
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

                        if (line.Trim().Length > 0 && lineCounter <= _maxLinesToBeRead)
                        {
                            _dropReadingPeak = false;
                            string[] splittedLine = line.Split('\t');

                            if (!(_leftColumn < splittedLine.Length && int.TryParse(splittedLine[_leftColumn], out left)))
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid start/position column number");
                                continue;
                            }

                            if (_rightColumn > 0 && !(_rightColumn < splittedLine.Length && int.TryParse(splittedLine[_rightColumn], out right)))
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid stop column number");
                                continue;
                            }

                            I readingInterval = BuildInterval(left, right, splittedLine, lineCounter, out intervalName);
                            if (_dropReadingPeak)
                                continue;

                            if (_chrColumn < splittedLine.Length)
                            {
                                if (Regex.IsMatch(splittedLine[_chrColumn].ToLower(), "chr"))
                                    chrName = splittedLine[_chrColumn];
                                else if (int.TryParse(splittedLine[_chrColumn], out int chrNumber))
                                    chrName = "chr" + chrNumber;
                                else
                                {
                                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome number ( " + splittedLine[_chrColumn].ToString() + " )");
                                    continue;
                                }
                            }
                            else
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome column number");
                                continue;
                            }

                            if (_readOnlyValidChrs && !_assemblyData.ContainsKey(chrName))
                            {
                                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome number ( " + splittedLine[_chrColumn].ToString() + " )");
                                continue;
                            }

                            strand = '*';
                            if (_strandColumn != -1 && _strandColumn < line.Length)
                                if (char.TryParse(splittedLine[_strandColumn], out strand))
                                    if (strand != '+' && strand != '-' && strand != '*')
                                        strand = '*';

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

                            _data.Add(readingInterval, chrName, strand);
                        }
                    }
                }

                if (_dropedLinesCount > 0)
                    Messages.Insert(0, "\t" + _dropedLinesCount.ToString() + " Lines droped");
                _data.Messages = Messages;

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
        protected abstract I BuildInterval(int left, int right, string[] line, UInt32 lineCounter, out string intervalName);


        private void ReadMissingAndExcessChrs()
        {
            foreach (var chr in _data.Chromosomes)
                if (!_assemblyData.ContainsKey(chr.Key))
                    _excessChrs.Add(chr.Key);

            foreach (KeyValuePair<string, int> entry in _assemblyData)
                if (!_data.Chromosomes.ContainsKey(entry.Key.ToLower()))
                    _missingChrs.Add(entry.Key);
        }

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
            string key = _data.FileHashKey + "_" + readingPeak.Left.ToString() + "_" + readingPeak.Right.ToString() + "_" + lineNo.ToString();
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
            string key = _data.FileHashKey + "_" + readingPeak.Left.ToString() + "_" + readingPeak.Right.ToString() + "_" + lineNo.ToString();
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
