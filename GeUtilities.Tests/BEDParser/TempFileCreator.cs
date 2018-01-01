// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

/// <summary>
/// This namespace contains Tests for both base and BED parsers.
/// </summary>
namespace GeUtilities.Tests.TBEDParser
{
    internal sealed class TempFileCreator : IDisposable
    {
        private string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempFileCreator() : this(new Columns()) { }

        public TempFileCreator(string peak)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(peak);
        }

        public TempFileCreator(string[] peaks)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                foreach (var peak in peaks)
                    sw.WriteLine(peak);
        }

        public TempFileCreator(Columns columns, int headerLineCount = 0, int peaksCount = 1)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                while (headerLineCount-- > 0)
                    sw.WriteLine(columns.GetSampleHeader());

                while (peaksCount-- > 0)
                {
                    sw.WriteLine(columns.GetSampleLine());
                    if (peaksCount > 0)
                    {
                        columns.Peak.Left = columns.Peak.Right + 10;
                        columns.Peak.Right = columns.Peak.Right + 20;
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
