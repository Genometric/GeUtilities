// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class RefSeqGenesParser<I> : Parser<I, IntervalStats>
        where I : IGene, new()
    {
        /// <summary>
        /// Parse refseq genes presented in tab-delimited text file.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="species">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        public RefSeqGenesParser(
            string source,
            Genomes species,
            Assemblies assembly,
            bool readOnlyValidChrs)
        {
            Source = source;
            Genome = species;
            Assembly = assembly;
            ChrColumn = 0;
            LeftColumn = 1;
            RightColumn = 2;
            _refseqIDColumn = 3;
            _officialGeneColumn = 4;
            _strandColumn = -1;
            _readOnlyCoordinates = true;
            ReadOnlyValidChrs = readOnlyValidChrs;
            maxLinesToBeRead = uint.MaxValue;
            HashFunction = HashFunction.One_at_a_Time;
            Initialize();
        }


        /// <summary>
        /// Parse refseq genes presented in tab-delimited text file.
        /// </summary>
        /// <param name="source">Full path of source file name</param>
        /// <param name="species">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        /// <param name="startOffset">If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. </param>
        /// <param name="chrColumn">The coloumn number of chromosome name</param>
        /// <param name="leftEndColumn">The column number of gene start position</param>
        /// <param name="rightEndColumn">The column number of gene stop position</param>
        public RefSeqGenesParser(
            string source,
            Genomes species,
            Assemblies assembly,
            bool readOnlyValidChrs,
            byte startOffset,
            sbyte chrColumn,
            sbyte leftEndColumn,
            sbyte rightEndColumn,
            sbyte refseqIDColumn,
            sbyte officialGeneSymbolColumn,
            sbyte strandColumn,
            uint maxLinesToRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time)
        {
            Source = source;
            Genome = species;
            Assembly = assembly;
            StartOffset = startOffset;
            ChrColumn = chrColumn;
            LeftColumn = leftEndColumn;
            RightColumn = rightEndColumn;
            _refseqIDColumn = refseqIDColumn;
            _officialGeneColumn = officialGeneSymbolColumn;
            _strandColumn = strandColumn;
            _readOnlyCoordinates = true;
            ReadOnlyValidChrs = readOnlyValidChrs;
            HashFunction = hashFunction;
            maxLinesToBeRead = maxLinesToRead;
            Initialize();
        }


        #region .::.         private Variables declaration               .::.

        /// <summary>
        /// Gets and sets the column number of refseq ID.
        /// </summary>
        private sbyte _refseqIDColumn { set; get; }

        /// <summary>
        /// Gets and sets the column number of official gene symbol.
        /// </summary>
        private sbyte _officialGeneColumn { set; get; }

        /// <summary>
        /// Gets and sets the column number of chromosome stand.
        /// </summary>
        private sbyte _strandColumn { set; get; }

        /// <summary>
        /// If the input file contains refseq ID and official gene symbol, then the 
        /// constructures will set this variable to FALSE which leads to registering
        /// error messages if refseq ID and/or official gene symbol is missing.
        /// <para>
        /// If the input file does NOT contain refseq ID and official gene symbol then
        /// a constructure that does not specifies these columns needs to be used. In 
        /// that case, this variable will be set to TRUE which avoids registering error
        /// messages for missing refseq ID and official gene symbol.
        /// </para>
        /// </summary>
        private bool _readOnlyCoordinates { set; get; }

        #endregion

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter, out string intervalName)
        {
            I rtv = new I();

            #region .::.     Process Refseq ID              .::.

            if (_refseqIDColumn < line.Length)
            {
                rtv.RefSeqID = line[_refseqIDColumn];
            }
            else if (!_readOnlyCoordinates)
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid refseq ID column number");
            }
            intervalName = rtv.RefSeqID;

            #endregion
            #region .::.     Process Official gene symbol   .::.

            if (_officialGeneColumn < line.Length)
            {
                rtv.GeneSymbol = line[_officialGeneColumn];
            }
            else if (!_readOnlyCoordinates)
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid official gene symbol column number");
            }

            #endregion
            #region .::.     Process I Strand               .::.

            rtv.Strand = '*';
            if (_strandColumn != -1 && _strandColumn < line.Length)
                if (char.TryParse(line[_strandColumn], out char strand))
                    rtv.Strand = strand;

            #endregion

            return rtv;
        }

        /// <summary>
        /// Reads the regions presented in source file and generates chromosome-wide statistics regarding regions length and p-values. 
        /// </summary>
        /// <returns>Returns an object of Input_BED_Data class</returns>
        public ParsedRefSeqGenes<I> Parse()
        {
            var parsingResult = (ParsedRefSeqGenes<I>)PARSE();
            Status = "100";
            return parsingResult;
        }
    }
}
