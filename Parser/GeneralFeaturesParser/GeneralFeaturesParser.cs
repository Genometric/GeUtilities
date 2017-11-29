// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using System.Collections.Generic;

namespace Genometric.GeUtilities.Parsers
{
    public sealed class GeneralFeaturesParser<I, M> : Parser<I, M>
        where I : IInterval<int, M>, new()
        where M : IGeneralFeature, new()
    {
        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="species">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        public GeneralFeaturesParser(
            string source,
            Genomes species,
            Assemblies assembly,
            bool readOnlyValidChrs)
        {
            InitializeDefaultValues();
            _source = source;
            _genome = species;
            _assembly = assembly;
            _readOnlyValidChrs = readOnlyValidChrs;
            Initialize();
        }


        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="source">Full path of source file name.</param>
        /// <param name="species">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
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
            Genomes species,
            Assemblies assembly,
            bool readOnlyValidChrs,
            byte startOffset,
            byte chrColumn,
            byte leftEndColumn,
            byte rightEndColumn,
            byte featureColumn,
            byte attributeColumn, 
            HashFunction hashFunction)
        {
            InitializeDefaultValues();
            _source = source;
            _source = source;
            _genome = species;
            _assembly = assembly;
            _readOnlyValidChrs = readOnlyValidChrs;
            _startOffset = startOffset;
            _chrColumn = chrColumn;
            _leftColumn = leftEndColumn;
            _rightColumn = rightEndColumn;
            _featureColumn = featureColumn;
            _attributeColumn = attributeColumn;
            _hashFunction = hashFunction;
            Initialize();
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

        private void InitializeDefaultValues()
        {
            _chrColumn = 0;
            _leftColumn = 1;
            _rightColumn = 2;
            _featureColumn = 3;
            _attributeColumn = 4;
            maxLinesToBeRead = uint.MaxValue;
            determinedFeatures = new Dictionary<string, DeterminedFeature>();
            _parsingType = ParsingType.GeneralFeatures;
        }

        protected override I ParseLine(string[] line, uint lineCounter, out string intervalName)
        {
            I rtv = new I
            {
                Metadata = new M()
            };

            #region .::.     Process Feature         .::.

            if (_featureColumn < line.Length)
            {
                if (!determinedFeatures.ContainsKey(line[_featureColumn]))
                    determinedFeatures.Add(line[_featureColumn], new DeterminedFeature(line[_featureColumn], 0, (byte)determinedFeatures.Count));

                rtv.Metadata.Feature = determinedFeatures[line[_featureColumn]].Code;
                determinedFeatures[line[_featureColumn]].Count++;
            }
            else
            {
                _dropLine = true;
                DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid feature column number");
            }
            #endregion
            #region .::.     Process Attribute       .::.

            if (_attributeColumn < line.Length)            
                rtv.Metadata.Attribute = line[_attributeColumn];            
            else            
                rtv.Metadata.Attribute = null;

            #endregion

            intervalName = "Hamed";
            return rtv;
        }

        public ParsedGeneralFeatures<int, I, M> Parse()
        {
            var parsingResult = (ParsedGeneralFeatures<int, I, M>)PARSE();
            parsingResult.determinedFeatures = new DeterminedFeature[determinedFeatures.Keys.Count];
            int keyCounter = 0;
            foreach (var key in determinedFeatures)
                parsingResult.determinedFeatures[keyCounter] = key.Value;

            Status = "100";
            return parsingResult;
        }
    }
}
