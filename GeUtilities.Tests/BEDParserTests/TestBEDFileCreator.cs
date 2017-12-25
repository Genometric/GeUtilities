// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests
{
    class TestBEDFileCreator : IDisposable
    {
        private string _testFile;
        public string TestFilePath { get { return _testFile; } }

        public TestBEDFileCreator(string peak)
        {
            _testFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            using (FileStream fs = File.Create(_testFile))
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(peak);
        }

        public TestBEDFileCreator(
            string chr = "chr1",
            string left = "10",
            string right = "20",
            string name = "GeUtilities_00",
            string value = "100.0",
            int headerLineCount = 0,
            int peaksCount = 1)
        {
            _testFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            using (FileStream fs = File.Create(_testFile))
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
            File.Delete(_testFile);
        }
    }
}
