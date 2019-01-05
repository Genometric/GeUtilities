// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Intervals.Functions;
using Genometric.GeUtilities.ReferenceGenomes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Genometric.GeUtilities.Intervals.Parsers.Model
{
    public abstract class Parser<I, S>
        where I : IInterval<int>
        where S : IStats<int>, new()
    {
        private ParsedIntervals<I, S> _data;

        /// <summary>
        /// Full path of source file name.
        /// </summary>
        private string _sourceFilePath;

        /// <summary>
        /// Sets and gets the column number of chromosome name.
        /// </summary>
        private readonly byte _chrColumn;

        /// <summary>
        /// Sets and gets column number of peak left position.
        /// </summary>
        private readonly byte _leftColumn;

        /// <summary>
        /// Sets and gets the column number of peak right position.
        /// </summary>
        private readonly sbyte _rightColumn;

        /// <summary>
        /// Sets and gets the column number of strand.
        /// </summary>
        private readonly sbyte _strandColumn;

        /// <summary>
        /// When read process is finished, this variable contains the number
        /// of dropped regions.
        /// </summary>
        private ushort _dropedLinesCount;

        /// <summary>
        /// Holds cached information of each chromosome's base pairs count. 
        /// This information will be updated based on the selected species.
        /// </summary>
        private ReadOnlyDictionary<string, int> _assemblyData;

        /// <summary>
        /// Sets and gets validity of the interval being parsed.
        /// In other words, indicates if the required fields of 
        /// the interval are parsed correctly. For instance, if
        /// the chromosome attribute of the peak is read.
        /// </summary>
        private bool DropReadingPeak { set; get; }

        #region .::.         Status variable and it's event controllers   .::.

        private string _status = "0";
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

        protected List<string> Messages { set; get; }

        /// <summary>
        /// Sets and gets the number of lines to be skipped from the 
        /// beginning of a file. This variable can be used to skip
        /// header lines of a file. However, if this property is not
        /// set, and source file contains header lines, still the 
        /// header lines will be ignored and reported as improperly
        /// formated data lines.
        /// </summary>
        public byte ReadOffset { set; get; }

        /// <summary>
        /// Sets and gets the number of lines to read from input file.
        /// The default value is 4,294,967,295 (0xFFFFFFFF).
        /// </summary>
        public uint MaxLinesToRead { set; get; }

        /// <summary>
        /// Set and gets if the parser should read only regions whose 
        /// chromosome value is defined in the specified assembly. For instance, 
        /// if set to false, the parser will not read a peak with chr44 
        /// when the assembly is set to homo sapiens.
        /// </summary>
        public bool ReadOnlyAssemblyChrs
        {
            set { _readOnlyAssemblyChrs = value; }
            get { return Assembly != Assemblies.Unknown && _readOnlyAssemblyChrs; }
        }
        private bool _readOnlyAssemblyChrs = true;

        public ReadOnlyCollection<string> ExcessChrs { get { return _excessChrs.AsReadOnly(); } }
        private List<string> _excessChrs;

        public ReadOnlyCollection<string> MissingChrs { get { return _missingChrs.AsReadOnly(); } }
        private List<string> _missingChrs;

        /// <summary>
        /// Set and gets the assembly of source file. 
        /// The assembly info are used for variety of purposes,
        /// e.g., to determine if a region has a 
        /// chromosome value valid w.r.t. to the assembly.
        /// </summary>
        public Assemblies Assembly { set; get; }

        /// <summary>
        /// Sets and gets the fields separator. 
        /// </summary>
        public char Delimiter { set; get; }

        protected Parser(BaseColumns columns)
        {
            _chrColumn = columns.Chr;
            _leftColumn = columns.Left;
            _rightColumn = columns.Right;
            _strandColumn = columns.Strand;
            MaxLinesToRead = uint.MaxValue;
            Delimiter = '\t';
        }

        protected ParsedIntervals<I, S> Parse(string sourceFilePath, ParsedIntervals<I, S> data)
        {
            _sourceFilePath = sourceFilePath;
            _data = data;
            _data.FilePath = Path.GetFullPath(_sourceFilePath);
            _data.FileName = Path.GetFileName(_sourceFilePath);
            _data.FileHashKey = HashFunctions.FNVHashFunction(_data.FilePath);
            _excessChrs = new List<string>();
            _missingChrs = new List<string>();
            Messages = new List<string>();

            _assemblyData = References.GetGenomeSizes(Assembly);
            if (!File.Exists(_sourceFilePath))
                throw new FileNotFoundException(string.Format("The file `{0}` does not exist or is inaccessible.", _sourceFilePath));

            Parse();

            if (_dropedLinesCount > 0)
                Messages.Insert(0, "\t" + _dropedLinesCount.ToString() + " Lines dropped");
            _data.Messages = Messages;

            if (Assembly != Assemblies.Unknown)
                ReadMissingAndExcessChrs();

            _data.Assembly = Assembly;
            return _data;
        }

        /// <summary>
        /// Reads the regions presented in source file; and generates chromosome-wide statistics.
        /// </summary>
        /// <returns>Returns parsed intervals.</returns>
        private void Parse()
        {
            int left = 0;
            int right = 0;
            string line;
            uint lineCounter = 0;

            DropReadingPeak = false;
            byte readOffset = ReadOffset;

            FileInfo fileInfo = new FileInfo(_sourceFilePath);
            long fileSize = fileInfo.Length;
            int lineSize = 0;
            string chrName = "";
            char strand = '*';

            using (StreamReader fileReader = new StreamReader(_sourceFilePath))
            {
                while (readOffset-- > 0)
                {
                    line = fileReader.ReadLine();
                    lineCounter++;
                }

                while ((line = fileReader.ReadLine()) != null)
                {
                    if (++lineCounter > MaxLinesToRead) break;

                    lineSize += fileReader.CurrentEncoding.GetByteCount(line);
                    Status = (Math.Round((lineSize * 100.0) / fileSize, 0)).ToString();

                    if (line.Trim().Length <= 0) continue;

                    DropReadingPeak = false;
                    string[] splittedLine = line.Split(Delimiter);

                    if (!(_leftColumn < splittedLine.Length && int.TryParse(splittedLine[_leftColumn], out left)))
                    {
                        DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid start/position column number");
                        continue;
                    }

                    if (_rightColumn >= 0 && !(_rightColumn < splittedLine.Length && int.TryParse(splittedLine[_rightColumn], out right)))
                    {
                        DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid stop column number");
                        continue;
                    }

                    I readingInterval = BuildInterval(left, right, splittedLine, lineCounter, _data.FileHashKey + lineCounter.ToString());
                    if (DropReadingPeak)
                        continue;

                    chrName = null;
                    if (_chrColumn < splittedLine.Length)
                    {
                        if (Regex.IsMatch(splittedLine[_chrColumn].ToLower(), "chr"))
                            chrName = splittedLine[_chrColumn];
                        else if (int.TryParse(splittedLine[_chrColumn], out int chrNumber))
                            chrName = "chr" + chrNumber;
                        else if (_assemblyData.ContainsKey("chr" + splittedLine[_chrColumn]))
                            chrName = "chr" + splittedLine[_chrColumn];
                        else
                            chrName = splittedLine[_chrColumn];
                        if (ReadOnlyAssemblyChrs && !_assemblyData.ContainsKey(chrName))
                            chrName = null;
                    }
                    if (chrName == null)
                    {
                        DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid chromosome number ( " + splittedLine[_chrColumn].ToString() + " )");
                        continue;
                    }

                    strand = '*';
                    if (_strandColumn != -1 && _strandColumn < splittedLine.Length &&
                       (char.TryParse(splittedLine[_strandColumn], out strand) && strand != '+' && strand != '-' && strand != '*'))
                        strand = '*';

                    _data.Add(readingInterval, chrName, strand);
                    _data.IntervalsCount++;
                }
            }
        }

        /// <summary>
        /// Parses the line for necessary information. 
        /// <para>Chromosome, Left-end, and Right-end will be 
        /// determined and added to returned value.</para>
        /// <para>Set the parameter _dropLine to true if
        /// parsing is unsuccessful.</para>
        /// </summary>
        /// <param name="line">The spitted line read from input.</param>
        /// <returns>The interval this line delegates.</returns>
        protected abstract I BuildInterval(int left, int right, string[] line, uint lineCounter, string hashSeed);

        private void ReadMissingAndExcessChrs()
        {
            foreach (var chr in _data.Chromosomes)
                if (!_assemblyData.ContainsKey(chr.Key))
                    _excessChrs.Add(chr.Key);

            foreach (KeyValuePair<string, int> entry in _assemblyData)
                if (!_data.Chromosomes.ContainsKey(entry.Key.ToLower()))
                    _missingChrs.Add(entry.Key);
        }        

        protected void DropLine(string message)
        {
            Messages.Add(message);
            DropReadingPeak = true;
            _dropedLinesCount++;
        }
    }
}
