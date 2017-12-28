// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.BEDParserTests
{
    class TempBEDFileCreator : IDisposable
    {
        private string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempBEDFileCreator(string peak)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(peak);
        }

        public TempBEDFileCreator(string[] peaks)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                foreach (var peak in peaks)
                    sw.WriteLine(peak);
        }

        public TempBEDFileCreator(
            string chr = "chr1",
            string left = "10",
            string right = "20",
            string name = "GeUtilities_00",
            string value = "100.0",
            int headerLineCount = 0,
            int peaksCount = 1)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                while (headerLineCount-- > 0)
                    sw.WriteLine("chr\tLeft\tRight\tName\tValue");

                while (peaksCount-- > 0)
                {
                    sw.WriteLine(chr + "\t" + left + "\t" + right + "\t" + name + "\t" + value);
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
