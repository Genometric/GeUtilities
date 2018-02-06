// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.TVCFParser
{
    public class TempFileCreator : IDisposable
    {
        private string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempFileCreator(string line)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".vcf";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(line);
        }

        public TempFileCreator(Columns columns, int headerLineCount = 0, int variantsCount = 1)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".vcf";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                while (headerLineCount-- > 0)
                    sw.WriteLine(columns.GetSampleHeader());

                while (variantsCount-- > 0)
                {
                    sw.WriteLine(columns.GetSampleLine());
                    if (variantsCount > 0)
                        columns.Position += 10;
                }
            }
        }

        public void Dispose()
        {
            File.Delete(_tempFilePath);
        }
    }
}
