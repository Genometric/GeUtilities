﻿// Licensed to the Genometric organization (https://github.com/Genometric) under one or more agreements.
// The Genometric organization licenses this file to you under the GNU General Public License v3.0 (GPLv3).
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Genometric.GeUtilities.Tests.Intervals.Parsers.Bed
{
    internal class TempFileCreator : IDisposable
    {
        public string Path { get; }

        public TempFileCreator() : this(new RegionGenerator()) { }

        public TempFileCreator(string peak)
        {
            Path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            FileStream stream = File.Create(Path);
            using (StreamWriter writter = new StreamWriter(stream))
                writter.WriteLine(peak);
        }

        public TempFileCreator(string[] peaks)
        {
            Path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            FileStream stream = File.Create(Path);
            using (StreamWriter writer = new StreamWriter(stream))
                foreach (var peak in peaks)
                    writer.WriteLine(peak);
        }

        public TempFileCreator(RegionGenerator columns, int headerLineCount = 0, int peaksCount = 1)
        {
            Path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".bed";
            FileStream stream = File.Create(Path);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                while (headerLineCount-- > 0)
                    writer.WriteLine(columns.GetSampleHeader());

                while (peaksCount-- > 0)
                {
                    writer.WriteLine(columns.GetSampleLine());
                    if (peaksCount > 0)
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
            File.Delete(Path);
        }
    }
}
