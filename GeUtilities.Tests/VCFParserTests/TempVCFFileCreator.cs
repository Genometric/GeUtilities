// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.VCFParserTests
{
    public class TempVCFFileCreator : IDisposable
    {
        private string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempVCFFileCreator(string line)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".vcf";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(line);
        }

        public TempVCFFileCreator(
            VCFColumns vcfColumns,
            string chr = "chr1",
            string position = "10",
            string id = "20",
            string refBP = "ACGNTU",
            string altBP = "UTNGCA",
            string quality = "quality",
            string filter = "filter",
            string info = "info",
            string strand = "*",
            int headerLineCount = 0,
            int variantsCount = 1)
        {
            string line = "", header = "";
            for (sbyte i = 0; i <= vcfColumns.MaxColumnIndex(); i++)
            {
                if (vcfColumns.ChrColumn == i)
                {
                    line += chr + "\t";
                    header += "chr\t";
                }
                else if (vcfColumns.PositionColumn == i)
                {
                    line += position + "\t";
                    header += "Position\t";
                }
                else if (vcfColumns.IDColumn == i)
                {
                    line += id + "\t";
                    header += "ID\t";
                }
                else if (vcfColumns.RefbbpColumn == i)
                {
                    line += refBP + "\t";
                    header += "Refbp\t";
                }
                else if (vcfColumns.AltbpColumn == i)
                {
                    line += altBP + "\t";
                    header += "Altbp\t";
                }
                else if (vcfColumns.QualityColumn == i)
                {
                    line += quality + "\t";
                    header += "Quality\t";
                }
                else if (vcfColumns.FilterColumn == i)
                {
                    line += filter + "\t";
                    header += "Filter\t";
                }
                else if (vcfColumns.InfoColumn == i)
                {
                    line += info + "\t";
                    header += "Info\t";
                }
                else if (vcfColumns.StrandColumn == i)
                {
                    line += strand + "\t";
                    header += "Strand\t";
                }
                else
                {
                    line += "AbCd\t";
                    header += "aBcD\t";
                }
            }

            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".vcf";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                while (headerLineCount-- > 0)
                    sw.WriteLine(header);

                while (variantsCount-- > 0)
                {
                    sw.WriteLine(line);
                    if (int.TryParse(position, out int cPosition))
                        position = (cPosition + 10).ToString();
                }
            }
        }

        public void Dispose()
        {
            File.Delete(_tempFilePath);
        }
    }
}
