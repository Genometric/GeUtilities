// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.Intervals.Parsers.Model;
using System.Collections.Generic;

namespace Genometric.GeUtilities.Intervals.Parsers
{
    public class GtfParser<I> : Parser<I, IntervalStats>
        where I : IGeneralFeature
    {
        /// <summary>
        /// Sets and gets the column number of feature.
        /// </summary>
        private readonly sbyte _featureColumn;

        /// <summary>
        /// Sets and gets the column number of attribute.
        /// </summary>
        private readonly sbyte _attributeColumn;

        private readonly sbyte _sourceColumn;
        private readonly sbyte _scoreColumn;
        private readonly sbyte _frameColumn;
        private readonly Dictionary<string, int> _features;
        private readonly IGeneralFeatureConstructor<I> _constructor;

        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        public GtfParser(GtfColumns columns, IGeneralFeatureConstructor<I> constructor) : base(columns)
        {
            _sourceColumn = columns.Source;
            _featureColumn = columns.Feature;
            _scoreColumn = columns.Score;
            _frameColumn = columns.Frame;
            _attributeColumn = columns.Attribute;
            _features = new Dictionary<string, int>();
            _constructor = constructor;
        }

        protected override I BuildInterval(int left, int right, string[] line, uint lineCounter, string hashSeed)
        {
            if (!(_scoreColumn != -1 && _scoreColumn < line.Length && double.TryParse(line[_scoreColumn], out double score)))
                score = double.NaN;

            string feature = null;
            if (_featureColumn != -1)
                if (_featureColumn < line.Length)
                {
                    feature = line[_featureColumn];

                    if (!_features.ContainsKey(feature))
                        _features.Add(feature, 1);
                    else
                        _features[feature]++;
                }
                else
                {
                    DropLine("\tLine " + lineCounter.ToString() + "\t:\tInvalid feature column number");
                }

            I rtv = _constructor.Construct(left, right,
                source: _sourceColumn != -1 && _sourceColumn < line.Length ? line[_sourceColumn] : null,
                feature: feature,
                score: score,
                frame: _frameColumn != -1 && _frameColumn < line.Length ? line[_frameColumn] : null,
                attribute: _attributeColumn != -1 && _attributeColumn < line.Length ? line[_attributeColumn] : null,
                hashSeed: hashSeed);

            return rtv;
        }

        public Gtf<I> Parse(string sourceFilePath)
        {
            var parsedData = (Gtf<I>)Parse(sourceFilePath, new Gtf<I>());
            parsedData.DeterminedFeatures = new Dictionary<string, int>(_features);
            Status = "100";
            return parsedData;
        }
    }
}
