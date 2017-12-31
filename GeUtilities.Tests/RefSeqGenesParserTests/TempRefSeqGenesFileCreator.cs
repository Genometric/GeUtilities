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
            RefSeqColumns refSeqColumns,
            string chr = "chr1",
            string left = "10",
            string right = "20",
            string refSeqID = "refSeqID_01",
            string geneSymbol = "geneSymbol_01",
            string strand = "*",
            int headerLineCount = 0,
            int genesCount = 1)
        {
            refSeqColumns.GetRefSeqLine(out string line, out string header, chr, left, right, refSeqID, geneSymbol, strand);
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".refSeq";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                while (headerLineCount-- > 0)
                    sw.WriteLine(header);

                while (genesCount-- > 0)
                    sw.WriteLine(line);
            }
        }

        public void Dispose()
        {
            File.Delete(_tempFilePath);
        }
    }
}
