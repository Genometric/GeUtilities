// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.ReferenceGenomes;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class RefSeqGenesParser<I> : Parser<I, IntervalStats>
        where I : IGene, new()
    {
        #region .::.         private properties         .::.

        /// <summary>
        /// Gets and sets the column number of refseq ID.
        /// </summary>
        private sbyte _refSeqIDColumn { set; get; }

        /// <summary>
        /// Gets and sets the column number of official gene symbol.
        /// </summary>
        private sbyte _officialGeneColumn { set; get; }

        #endregion

        /// <summary>
        /// Parse refseq genes presented in tab-delimited text file.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        public RefSeqGenesParser(
            string sourceFilePath,
            Assemblies assembly = Assemblies.Unknown,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue) :
            this(sourceFilePath: sourceFilePath,
                assembly: assembly,
                readOnlyValidChrs: readOnlyValidChrs,
                startOffset: 0,
                chrColumn: 0,
                leftEndColumn: 1,
                rightEndColumn: 2,
                refSeqIDColumn: 3,
                officialGeneSymbolColumn: 4,
                strandColumn: -1,
                maxLinesToRead: maxLinesToBeRead,
                hashFunction: HashFunction.One_at_a_Time
                )

        { }


        /// <summary>
        /// Parse refseq genes presented in tab-delimited text file.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        /// <param name="startOffset">If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. </param>
        /// <param name="chrColumn">The column number of chromosome name</param>
        /// <param name="leftEndColumn">The column number of gene start position</param>
        /// <param name="rightEndColumn">The column number of gene stop position</param>
        public RefSeqGenesParser(
            string sourceFilePath,
            byte chrColumn,
            byte leftEndColumn,
            sbyte rightEndColumn,
            sbyte refSeqIDColumn,
            sbyte officialGeneSymbolColumn,
            sbyte strandColumn = -1,
            Assemblies assembly = Assemblies.Unknown,
            byte startOffset = 0,
            bool readOnlyValidChrs = true,
            uint maxLinesToRead = uint.MaxValue,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            base(sourceFilePath: sourceFilePath,
                assembly: assembly,
                startOffset: startOffset,
                chrColumn: chrColumn,
                leftEndColumn: leftEndColumn,
                rightEndColumn: rightEndColumn,
                strandColumn: strandColumn,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToBeRead: maxLinesToRead,
                hashFunction: hashFunction,
                data: new ParsedRefSeqGenes<I>())
        {
            _refSeqIDColumn = refSeqIDColumn;
            _officialGeneColumn = officialGeneSymbolColumn;
        }

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter)
        {
            I rtv = new I
            {
                Left = left,
                Right = right
            };

            if (_refSeqIDColumn >= 0)
            {
                if (_refSeqIDColumn < line.Length)
                    rtv.RefSeqID = line[_refSeqIDColumn];
                else
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid refseq ID column number");
            }

            if (_officialGeneColumn >= 0)
            {
                if (_officialGeneColumn < line.Length)
                    rtv.GeneSymbol = line[_officialGeneColumn];
                else
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid official gene symbol column number");
            }

            return rtv;
        }

        /// <summary>
        /// Reads the regions presented in source file and generates chromosome-wide statistics regarding regions length and p-values. 
        /// </summary>
        /// <returns>Returns an object of Input_BED_Data class</returns>
        public new ParsedRefSeqGenes<I> Parse()
        {
            var parsingResult = (ParsedRefSeqGenes<I>)base.Parse();
            Status = "100";
            return parsingResult;
        }
    }
}
