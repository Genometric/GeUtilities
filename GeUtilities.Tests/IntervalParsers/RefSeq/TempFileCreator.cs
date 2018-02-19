// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.IntervalParsers.RefSeq
{
    public class TempFileCreator : IDisposable
    {
        private readonly string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempFileCreator(string line)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".refSeq";
            FileStream stream = File.Create(_tempFilePath);
            using (StreamWriter writter = new StreamWriter(stream))
                writter.WriteLine(line);
        }

        public TempFileCreator(RegionGenerator columns, int headerLineCount = 0, int genesCount = 1)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".refSeq";
            FileStream stream = File.Create(_tempFilePath);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                while (headerLineCount-- > 0)
                    writer.WriteLine(columns.GetSampleHeader());

                while (genesCount-- > 0)
                    writer.WriteLine(columns.GetSampleLine());
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
