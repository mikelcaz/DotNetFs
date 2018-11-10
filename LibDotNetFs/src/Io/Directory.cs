// Copyright © 2018 Mikel Cazorla Pérez.

using System;
using System.IO;

using OldDirectory = System.IO.Directory;

namespace DotNetFs.Io
{
    public static class Directory
    {
        public static bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static DirectoryInfo GetDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
                return directoryInfo;

            var fileInfo = new FileInfo(path);
            if (!File.Exists(path))
                throw new ArgumentException(
                    "File or directory not found",
                    paramName: path);

            return fileInfo.Directory;
        }
    }
}
