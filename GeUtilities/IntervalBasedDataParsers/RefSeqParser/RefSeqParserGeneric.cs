// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.ReferenceGenomes;

namespace Genometric.GeUtilities.Parsers
{
    public class RefSeqParser<I> : Parser<I, IntervalStats>
        where I : IRefSeq, new()
    {
        #region .::.         private properties         .::.

        /// <summary>
        /// Gets and sets the column number of refseq ID.
        /// </summary>
        private byte _refSeqIDColumn { set; get; }

        /// <summary>
        /// Gets and sets the column number of official gene symbol.
        /// </summary>
        private byte _geneColumn { set; get; }

        #endregion

        /// <summary>
        /// Parse refseq genes presented in tab-delimited text file.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        public RefSeqParser(
            string sourceFilePath,
            Assemblies assembly = Assemblies.Unknown) :
            this(
                sourceFilePath: sourceFilePath,
                assembly: assembly,
                chrColumn: 0,
                leftEndColumn: 1,
                rightEndColumn: 2,
                refSeqIDColumn: 3,
                geneSymbolColumn: 4,
                strandColumn: -1)
        { }


        /// <summary>
        /// Parse refseq genes presented in tab-delimited text file.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="chrColumn">The column number of chromosome name</param>
        /// <param name="leftEndColumn">The column number of gene start position</param>
        /// <param name="rightEndColumn">The column number of gene stop position</param>
        public RefSeqParser(
            string sourceFilePath,
            byte chrColumn,
            byte leftEndColumn,
            sbyte rightEndColumn,
            byte refSeqIDColumn,
            byte geneSymbolColumn,
            sbyte strandColumn = -1,
            Assemblies assembly = Assemblies.Unknown) :
            base(sourceFilePath: sourceFilePath,
                assembly: assembly,
                chrColumn: chrColumn,
                leftEndColumn: leftEndColumn,
                rightEndColumn: rightEndColumn,
                strandColumn: strandColumn,
                data: new RefSeq<I>())
        {
            _refSeqIDColumn = refSeqIDColumn;
            _geneColumn = geneSymbolColumn;
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

            if (_geneColumn >= 0)
            {
                if (_geneColumn < line.Length)
                    rtv.GeneSymbol = line[_geneColumn];
                else
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid official gene symbol column number");
            }

            return rtv;
        }

        /// <summary>
        /// Reads the regions presented in source file and generates chromosome-wide statistics regarding regions length and p-values. 
        /// </summary>
        /// <returns>Returns an object of Input_BED_Data class</returns>
        public new RefSeq<I> Parse()
        {
            var parsingResult = (RefSeq<I>)base.Parse();
            Status = "100";
            return parsingResult;
        }
    }
}
