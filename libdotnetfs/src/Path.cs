using System;
using System.IO;

// Note: let it be case-sensitive!
// Note: its exhaustivity remains to be seen.
namespace DotNetFs
{
    public static class Path
    {
        public static string GetParentName(string path)
        {
            var candidate = System.IO.Path.GetDirectoryName(path);

            // It seems "a" root path.
            if (candidate == null)
                return System.IO.Path.GetPathRoot(path);

            // Note: it seems that methods from Path think
            // "." and ".." aren't directories.
            var fileName = System.IO.Path.GetFileName(path);

            // It has to be a directory path.
            // TODO: Implement MustBelongToADirectory method.
            if (
                fileName == string.Empty
                || fileName == "."
                || fileName == ".."
            )
            {
                // TODO: Make the proper implementation.
                return System.IO.Path.Combine(path, "..");
            }

            // It has no parent info.
            if (candidate == string.Empty)
            {
                return "..";
            }

            // It could be a file or directory path,
            // Path.GetDirectoryName assumes it's a file one.
            return candidate;
        }

        public static string GetNormalized(string path)
        {
            throw new NotImplementedException("GetNormalized not implemented");
        }

        public static bool MustBelongToADirectory(string path)
        {
            throw new NotImplementedException("MustBelongToADirectory not implemented");
        }

        public static string GetFileName(string path)
        {
            return System.IO.Path.GetFileName(path);
        }

        public static string Combine(string path1, string path2)
        {
            return System.IO.Path.Combine(path1, path2);
        }

        public static string Combine(string path1, string path2, string path3)
        {
            return System.IO.Path.Combine(path1, path2, path3);
        }

        public static string Combine(params string[] paths)
        {
            return System.IO.Path.Combine(paths);
        }
    }
}
