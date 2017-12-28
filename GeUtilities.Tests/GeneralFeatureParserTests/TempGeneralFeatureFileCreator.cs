// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace GeUtilities.Tests.GeneralFeatureParserTests
{
    public class TempGeneralFeatureFileCreator : IDisposable
    {
        private string _tempFilePath;
        public string TempFilePath { get { return _tempFilePath; } }

        public TempGeneralFeatureFileCreator(string line)
        {
            _tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".gtf";
            using (FileStream fs = File.Create(_tempFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(line);
        }

        public void Dispose()
        {
            File.Delete(_tempFilePath);
        }
    }
}
