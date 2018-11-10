// Copyright © 2018 Mikel Cazorla Pérez.

using System.IO;

namespace DotNetFs.Io
{
    public static class DosFile
    {
        public static void Move(
            string sourceFileName,
            string destFileName
        )
        {
            File.Move(sourceFileName, destFileName);
        }

        public static void Copy(
            string sourceFileName,
            string destFileName
        )
        {
            File.Copy(sourceFileName, destFileName);
        }

        public static void Copy(
            string sourceFileName,
            string destFileName,
            bool overwrite
        )
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }
    }
}
