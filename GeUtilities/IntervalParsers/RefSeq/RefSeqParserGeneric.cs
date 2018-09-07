// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalParsers.Model.Columns;

namespace Genometric.GeUtilities.IntervalParsers
{
    public class RefSeqParser<I> : Parser<I, IntervalStats>
        where I : IRefSeq
    {
        /// <summary>
        /// Gets and sets the column number of refseq ID.
        /// </summary>
        private readonly byte _refSeqIDColumn;

        /// <summary>
        /// Gets and sets the column number of official gene symbol.
        /// </summary>
        private readonly byte _geneColumn;

        private readonly IRefSeqConstructor<I> _constructor;

        /// <summary>
        /// Parse refseq genes presented in tab-delimited text file.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name</param>
        public RefSeqParser(RefSeqColumns columns, IRefSeqConstructor<I> constructor) : base(columns)
        {
            _refSeqIDColumn = columns.RefSeqID;
            _geneColumn = columns.GeneSeymbol;
            _constructor = constructor;
        }

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter, string hashSeed)
        {
            string refSeqID = null;
            if (_refSeqIDColumn >= 0)
            {
                if (_refSeqIDColumn < line.Length)
                    refSeqID = line[_refSeqIDColumn];
                else
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid refseq ID column number");
            }

            string geneSymbol = null;
            if (_geneColumn >= 0)
            {
                if (_geneColumn < line.Length)
                    geneSymbol = line[_geneColumn];
                else
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid official gene symbol column number");
            }

            I rtv = _constructor.Construct(left, right, refSeqID, geneSymbol, hashSeed);

            return rtv;
        }

        /// <summary>
        /// Reads the regions presented in source file and generates chromosome-wide statistics regarding regions length and p-values. 
        /// </summary>
        /// <returns>Returns an object of Input_BED_Data class</returns>
        public RefSeq<I> Parse(string sourceFilePath)
        {
            var parsingResult = (RefSeq<I>)Parse(sourceFilePath, new RefSeq<I>());
            Status = "100";
            return parsingResult;
        }
    }
}
