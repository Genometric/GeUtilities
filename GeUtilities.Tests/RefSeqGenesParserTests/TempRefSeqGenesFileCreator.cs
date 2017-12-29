// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.RefSeqGenesParserTests
{
    public class TempRefSeqGenesFileCreator : IDisposable
    {
        private string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempRefSeqGenesFileCreator(string line)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".refSeq";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(line);
        }

        public TempRefSeqGenesFileCreator(
            string chr = "chr1",
            string left = "10",
            string right = "20",
            string refSeqID = "refSeqID_01",
            string geneSymbol = "geneSymbol_01",
            string strand = "*",
            int headerLineCount = 0,
            int genesCount = 1)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".refSeq";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                while (headerLineCount-- > 0)
                    sw.WriteLine("chr\tLeft\tRight\tRefseqID\tOfficialGeneSymbol\tStrand");

                while (genesCount-- > 0)
                {
                    sw.WriteLine(chr + "\t" + left + "\t" + right + "\t" + refSeqID + "\t" + geneSymbol + "\t" + strand);
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
