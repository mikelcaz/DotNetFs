using System;
using System.IO;

using OldPath = System.IO.Path;

// Note: let it be case-sensitive!
// Note: its exhaustivity remains to be seen.
namespace DotNetFs
{
    public static class Path
    {
        public static string GetParentName(string path)
        {
            var candidate = OldPath.GetDirectoryName(path);

            // It seems "a" root path.
            if (candidate == null)
                return OldPath.GetPathRoot(path);

            // Note: it seems that methods from Path think
            // "." and ".." aren't directories.
            var fileName = OldPath.GetFileName(path);

            // It has to be a directory path.
            if (
                fileName == string.Empty
                || fileName == "."
                || fileName == ".."
            )
            {
                // TODO: Make the proper implementation.
                return OldPath.Combine(path, "..");
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
            return OldPath.GetFullPath(path).TrimEnd(
                OldPath.DirectorySeparatorChar,
                OldPath.AltDirectorySeparatorChar);
        }

        public static bool MustBelongToADirectory(string path)
        {
            return path == "."
                || path == ".."
                || path.EndsWith($"{OldPath.DirectorySeparatorChar}")
                || path.EndsWith($"{OldPath.DirectorySeparatorChar}.")
                || path.EndsWith($"{OldPath.DirectorySeparatorChar}..")
                || path.EndsWith($"{OldPath.AltDirectorySeparatorChar}")
                || path.EndsWith($"{OldPath.AltDirectorySeparatorChar}.")
                || path.EndsWith($"{OldPath.AltDirectorySeparatorChar}..");
        }

        public static string GetFileName(string path)
        {
            return OldPath.GetFileName(path);
        }

        public static string Combine(string path1, string path2)
        {
            return OldPath.Combine(path1, path2);
        }

        public static string Combine(string path1, string path2, string path3)
        {
            return OldPath.Combine(path1, path2, path3);
        }

        public static string Combine(params string[] paths)
        {
            return OldPath.Combine(paths);
        }
    }
}
