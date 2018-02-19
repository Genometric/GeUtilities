// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using Genometric.GeUtilities.IGenomics;
using Genometric.GeUtilities.IntervalParsers.Model.Columns;
using System.Collections.Generic;

namespace Genometric.GeUtilities.IntervalParsers
{
    public class GTFParser<I> : Parser<I, IntervalStats>
        where I : IGeneralFeature, new()
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

        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        public GTFParser(string sourceFilePath) :
            this(sourceFilePath, new GTFColumns())
        { }


        /// <summary>
        /// Parse General Transfer Format (GTF) format.
        /// </summary>
        /// <param name="sourceFilePath">Full path of source file name.</param>
        public GTFParser(string sourceFilePath, GTFColumns columns) :
            base(sourceFilePath, columns, new GTF<I>())
        {
            _sourceColumn = columns.Source;
            _featureColumn = columns.Feature;
            _scoreColumn = columns.Score;
            _frameColumn = columns.Frame;
            _attributeColumn = columns.Attribute;
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

        public new GTF<I> Parse()
        {
            var parsedData = (GTF<I>)base.Parse();
            parsedData.DeterminedFeatures = new Dictionary<string, int>(_features);
            Status = "100";
            return parsedData;
        }
    }
}
