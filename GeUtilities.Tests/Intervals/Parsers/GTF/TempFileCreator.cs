// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.GTF
{
    public class TempFileCreator : IDisposable
    {
        public string TempFilePath { get; }

        public TempFileCreator(string line)
        {
            TempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".gtf";
            FileStream stream = File.Create(TempFilePath);
            using (StreamWriter writer = new StreamWriter(stream))
                writer.WriteLine(line);
        }

        public TempFileCreator(RegionGenerator columns, int headerLineCount = 0, int featuresCount = 1)
        {
            TempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".gtf";
            FileStream stream = File.Create(TempFilePath);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                while (headerLineCount-- > 0)
                    writer.WriteLine(columns.GetSampleHeader());

                while (featuresCount-- > 0)
                {
                    writer.WriteLine(columns.GetSampleLine());
                    if (featuresCount > 0)
                    {
                        columns.Left = columns.Right + 10;
                        columns.Right = columns.Right + 20;
                    }
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
            File.Delete(TempFilePath);
        }
    }
}
