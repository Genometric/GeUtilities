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
            FileStream fs = null;
            try
            {
                fs = File.Create(_tempFilePath);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    fs = null;
                    sw.WriteLine(line);
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        public TempFileCreator(Columns columns, int headerLineCount = 0, int variantsCount = 1)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".vcf";
            FileStream fs = null;
            try
            {
                fs = File.Create(_tempFilePath);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    fs = null;
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
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            File.Delete(_tempFilePath);
        }
    }
}
