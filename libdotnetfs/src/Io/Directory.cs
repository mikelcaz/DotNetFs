using System;
using System.IO;

namespace DotNetFs.Io
{
    public static class Directory
    {
        public static bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static DirectoryInfo GetDirectory()
        {
            throw new NotImplementedException("Get Directory not implemented yet");
        }
    }
}
