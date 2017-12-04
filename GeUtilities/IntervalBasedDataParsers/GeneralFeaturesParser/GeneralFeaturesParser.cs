// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System.Collections.Generic;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class GeneralFeaturesParser<I> : Parser<I, IntervalStats>
        where I : IGeneralFeature, new()
    {
        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        public GeneralFeaturesParser(
            string source,
            Genomes genome,
            Assemblies assembly,
            bool readOnlyValidChrs=true,
            uint maxLinesToBeRead = uint.MaxValue) : 
            this(source: source, 
                genome: genome,
                assembly: assembly,
                readOnlyValidChrs: readOnlyValidChrs,
                startOffset: 0,
                chrColumn: 0,
                leftEndColumn: 1,
                rightEndColumn: 2,
                strandColumn: -1,
                featureColumn: 3,
                attributeColumn: 4,
                maxLinesToBeRead: maxLinesToBeRead)
        { }


        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        /// <param name="startOffset">If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. </param>
        /// <param name="chrColumn">The coloumn number of chromosome name.</param>
        /// <param name="leftEndColumn">The column number of feature start position.</param>
        /// <param name="rightEndColumn">The column number of feature stop position.</param>
        /// <param name="featureColumn">The column number of feature.</param>
        /// <param name="attributeColumn">The column number of a semicolon-separated list of tag-value pairs, providing additional information about each feature..</param>
        public GeneralFeaturesParser(
            string source,
            Genomes genome,
            Assemblies assembly,
            sbyte chrColumn,
            sbyte leftEndColumn,
            sbyte rightEndColumn,
            sbyte strandColumn,
            byte featureColumn,
            byte attributeColumn,
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
            _featureColumn = featureColumn;
            _attributeColumn = attributeColumn;
        }

        public Dictionary<string, DeterminedFeature> determinedFeatures { set; get; }

        #region .::.         private Variables declaration               .::.

        /// <summary>
        /// Sets and gets the column number of feature.
        /// </summary>
        private byte _featureColumn;

        /// <summary>
        /// Sets and gets the column number of attribute.
        /// </summary>
        private byte _attributeColumn;

        #endregion

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter, out string intervalName)
        {
            I rtv = new I();

            #region .::.     Process Feature         .::.

            if (_featureColumn < line.Length)
            {
                if (!determinedFeatures.ContainsKey(line[_featureColumn]))
                    determinedFeatures.Add(line[_featureColumn], new DeterminedFeature(line[_featureColumn], 0, (byte)determinedFeatures.Count));

                rtv.Feature = determinedFeatures[line[_featureColumn]].Code;
                determinedFeatures[line[_featureColumn]].Count++;
            }
            else
            {
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid feature column number");
            }
            #endregion
            #region .::.     Process Attribute       .::.

            if (_attributeColumn < line.Length)            
                rtv.Attribute = line[_attributeColumn];            
            else            
                rtv.Attribute = null;

            #endregion

            intervalName = "Hamed";
            return rtv;
        }

        public ParsedGeneralFeatures<I> Parse()
        {
            var parsingResult = (ParsedGeneralFeatures<I>)PARSE();
            parsingResult.determinedFeatures = new DeterminedFeature[determinedFeatures.Keys.Count];
            int keyCounter = 0;
            foreach (var key in determinedFeatures)
                parsingResult.determinedFeatures[keyCounter] = key.Value;

            Status = "100";
            return parsingResult;
        }
    }
}
