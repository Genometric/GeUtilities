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
        /// Sets and gets the column number of feature.
        /// </summary>
        private sbyte _featureColumn;

        /// <summary>
        /// Sets and gets the column number of attribute.
        /// </summary>
        private sbyte _attributeColumn;

        private sbyte _sourceColumn;
        private sbyte _scoreColumn;
        private sbyte _frameColumn;
        private Dictionary<string, int> _features;
        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        public GeneralFeaturesParser(
            string sourceFilePath,
            Assemblies assembly = Assemblies.Unknown,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue,
            byte startOffset = 0,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            this(sourceFilePath: sourceFilePath,
                assembly: assembly,
                readOnlyValidChrs: readOnlyValidChrs,
                startOffset: startOffset,
                chrColumn: 0,
                sourceColumn: 1,
                featureColumn: 2,
                leftEndColumn: 3,
                rightEndColumn: 4,
                scoreColumn: 5,
                strandColumn: 6,
                frameColumn: 7,
                attributeColumn: 8,
                maxLinesToBeRead: maxLinesToBeRead,
                hashFunction: hashFunction)
        { }


        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        /// <param name="genome">This parameter will be used for initializing the chromosome count and sex chromosomes mappings.</param>
        /// <param name="assembly"></param>
        /// <param name="readOnlyValidChrs"></param>
        /// <param name="startOffset">If the source file comes with header, the number of headers lines needs to be specified so that
        /// parser can ignore them. If not specified and header is present, header might be dropped because
        /// of improper format it might have. </param>
        /// <param name="chrColumn">The column number of chromosome name.</param>
        /// <param name="leftEndColumn">The column number of feature start position.</param>
        /// <param name="rightEndColumn">The column number of feature stop position.</param>
        /// <param name="featureColumn">The column number of feature.</param>
        /// <param name="attributeColumn">The column number of a semicolon-separated list of tag-value pairs, providing additional information about each feature..</param>
        public GeneralFeaturesParser(
            string sourceFilePath,
            sbyte chrColumn,
            sbyte sourceColumn,
            sbyte featureColumn,
            byte leftEndColumn,
            sbyte rightEndColumn,
            sbyte scoreColumn,
            sbyte strandColumn,
            sbyte frameColumn,
            sbyte attributeColumn,
            Assemblies assembly = Assemblies.Unknown,
            bool readOnlyValidChrs = true,
            uint maxLinesToBeRead = uint.MaxValue,
            byte startOffset = 0,
            HashFunction hashFunction = HashFunction.One_at_a_Time) :
            base(sourceFilePath: sourceFilePath,
                assembly: assembly,
                startOffset: startOffset,
                chrColumn: chrColumn,
                leftEndColumn: leftEndColumn,
                rightEndColumn: rightEndColumn,
                strandColumn: strandColumn,
                readOnlyValidChrs: readOnlyValidChrs,
                maxLinesToBeRead: maxLinesToBeRead,
                hashFunction: hashFunction,
                data: new ParsedGeneralFeatures<I>())
        {
            _sourceColumn = sourceColumn;
            _featureColumn = featureColumn;
            _scoreColumn = scoreColumn;
            _frameColumn = frameColumn;
            _attributeColumn = attributeColumn;
            _features = new Dictionary<string, int>();
        }

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter)
        {
            I rtv = new I
            {
                Left = left,
                Right = right
            };

            if (_sourceColumn != -1 && _sourceColumn < line.Length)
                rtv.Source = line[_sourceColumn];
            else
                rtv.Source = null;

            if (_scoreColumn != -1 && _scoreColumn < line.Length && double.TryParse(line[_scoreColumn], out double score))
                rtv.Score = score;
            else
                rtv.Score = double.NaN;

            if (_frameColumn != -1 && _frameColumn < line.Length)
                rtv.Frame = line[_frameColumn];
            else
                rtv.Frame = null;

            if (_featureColumn != -1)
                if (_featureColumn < line.Length)
                {
                    rtv.Feature = line[_featureColumn];

                    if (!_features.ContainsKey(rtv.Feature))
                        _features.Add(rtv.Feature, 1);
                    else
                        _features[rtv.Feature]++;
                }
                else
                {
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid feature column number");
                }

            if (_attributeColumn != -1 && _attributeColumn < line.Length)
                rtv.Attribute = line[_attributeColumn];
            else
                rtv.Attribute = null;

            return rtv;
        }

        public new ParsedGeneralFeatures<I> Parse()
        {
            var parsedData = (ParsedGeneralFeatures<I>)base.Parse();
            parsedData.DeterminedFeatures = new Dictionary<string, int>(_features);
            Status = "100";
            return parsedData;
        }
    }
}
