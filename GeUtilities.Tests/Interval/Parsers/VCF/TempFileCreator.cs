// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.Interval.Parsers.VCF
{
    public class TempFileCreator : IDisposable
    {
        private readonly string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempFileCreator(string line)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".vcf";
            FileStream stream = File.Create(_tempFilePath);
            using (StreamWriter writer = new StreamWriter(stream))
                writer.WriteLine(line);
        }

        public TempFileCreator(RegionGenerator columns, int headerLineCount = 0, int variantsCount = 1)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".vcf";
            FileStream stream = File.Create(_tempFilePath);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                while (headerLineCount-- > 0)
                    writer.WriteLine(columns.GetSampleHeader());

                while (variantsCount-- > 0)
                {
                    writer.WriteLine(columns.GetSampleLine());
                    if (variantsCount > 0)
                        columns.Position += 10;
                }
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
