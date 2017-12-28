// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.GeneralFeatureParserTests
{
    public class TempGeneralFeatureFileCreator : IDisposable
    {
        private string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempGeneralFeatureFileCreator(string line)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".gtf";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(line);
        }

        public TempGeneralFeatureFileCreator(
            string chr = "chr1",
            string source = "Di4",
            string feature = "Gene",
            string left = "10",
            string right = "20",
            double score = 100.0,
            char strand = '*',
            string frame = "0",
            string attribute = "att1=1;att2=v2",
            int headerLineCount = 0,
            int peaksCount = 1)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".gtf";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                while (headerLineCount-- > 0)
                    sw.WriteLine("chr\tSource\tFeature\tLeft\tRight\tScore\tStrand\tFrame\tAttribute");

                while (peaksCount-- > 0)
                {
                    sw.WriteLine(
                        chr + "\t" + source + "\t" + feature + "\t" + left + "\t" + right + "\t" +
                        score + "\t" + strand + "\t" + frame + "\t" + attribute);
                    if (int.TryParse(left, out int cLeft) && int.TryParse(right, out int cRight))
                    {
                        left = (cRight + 10).ToString();
                        right = (cRight + 20).ToString();
                    }
                }
            }
        }

        public TempGeneralFeatureFileCreator(
            GTFColumns gtfColumns,
            string chr = "chr1",
            string source = "Di4",
            string feature = "Gene",
            string left = "10",
            string right = "20",
            double score = 100.0,
            char strand = '*',
            string frame = "0",
            string attribute = "att1=1;att2=v2",
            int headerLineCount = 0,
            int peaksCount = 1)
        {
            string line = "", header = "";
            for (sbyte i = 0; i <= gtfColumns.MaxColumnIndex(); i++)
            {
                if (gtfColumns.ChrColumn == i)
                {
                    line += chr + "\t";
                    header += "chr\t";
                }
                else if (gtfColumns.SourceColumn == i)
                {
                    line += source + "\t";
                    header += "Source\t";
                }
                else if (gtfColumns.FeatureColumn == i)
                {
                    line += feature + "\t";
                    header += "Feature\t";
                }
                else if (gtfColumns.LeftColumn == i)
                {
                    line += left + "\t";
                    header += "Left\t";
                }
                else if (gtfColumns.RightColumn == i)
                {
                    line += right + "\t";
                    header += "Right\t";
                }
                else if (gtfColumns.ScoreColumn == i)
                {
                    line += score + "\t";
                    header += "Score\t";
                }
                else if (gtfColumns.StrandColumn == i)
                {
                    line += strand + "\t";
                    header += "Strand\t";
                }
                else if (gtfColumns.FrameColumn == i)
                {
                    line += frame + "\t";
                    header += "Frame\t";
                }
                else if (gtfColumns.AttributeColumn == i)
                {
                    line += attribute + "\t";
                    header += "Attribute\t";
                }
                else
                {
                    line += "AbCd\t";
                    header += "aBcD\t";
                }
            }

            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".gtf";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                while (headerLineCount-- > 0)
                    sw.WriteLine(header);

                while (peaksCount-- > 0)
                {
                    sw.WriteLine(line);
                    if (int.TryParse(left, out int cLeft) && int.TryParse(right, out int cRight))
                    {
                        left = (cRight + 10).ToString();
                        right = (cRight + 20).ToString();
                    }
                }
            }
        }

        public void Dispose()
        {
            File.Delete(_tempFilePath);
        }
    }
}
