// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.TRefSeqParser
{
    public class TempFileCreator : IDisposable
    {
        private readonly string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempFileCreator(string line)
        {
            FileStream fs = null;
            try
            {
                _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".refSeq";
                fs = File.Create(_tempFilePath);
                using (StreamWriter sw = new StreamWriter(fs))
                    sw.WriteLine(line);
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        public TempFileCreator(Columns columns, int headerLineCount = 0, int genesCount = 1)
        {
            FileStream fs = null;
            try
            {
                _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".refSeq";
                fs = File.Create(_tempFilePath);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    while (headerLineCount-- > 0)
                        sw.WriteLine(columns.GetSampleHeader());

                    while (genesCount-- > 0)
                        sw.WriteLine(columns.GetSampleLine());
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
