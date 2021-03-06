// Copyright © 2018 Mikel Cazorla Pérez.

using System;
using System.IO;

using OldPath = System.IO.Path;

// Note: let it be case-sensitive!
// Note: its exhaustivity remains to be seen.
namespace DotNetFs
{
    public static class Path
    {
        /// <summary>
        /// Retrieves a path to the parent directory.
        /// </summary>
        /// <param name="path">
        /// An existing or ficticious path of a file or directory.
        /// </param>
        /// <remarks>
        /// Use it instead of <see cref="System.IO.Path.GetDirectoryName(string)"/>.
        /// </remarks>
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
                return ".";
            }

            // It could be a file or directory path,
            // Path.GetDirectoryName assumes it belongs to a file.
            return candidate;
        }

        internal static string TrimEndDirectorySeparators(this string path)
        {
            return path.TrimEnd(
                OldPath.DirectorySeparatorChar,
                OldPath.AltDirectorySeparatorChar);
        }

        public static string GetNormalized(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(
                    "Paths cannot be null nor white space",
                    paramName: "path");

            var fullPath = OldPath.GetFullPath(path);

            if (
                fullPath.StartsWith(OldPath.DirectorySeparatorChar.ToString())
                || fullPath.StartsWith(OldPath.AltDirectorySeparatorChar.ToString())
            )
            {
                var remainder = fullPath.Substring(1).TrimEndDirectorySeparators();
                if (remainder == string.Empty)
                    return OldPath.GetPathRoot(fullPath);
            }
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                // Special kinds of paths not supported!

                // A GetFullPath overlook? Intended?
                if (System.Text.RegularExpressions.Regex.Matches(fullPath, ":").Count > 1)
                    throw new ArgumentException(
                        "Paths cannot cointain the ':' caracter (but in the drive prefix)",
                        paramName: "path");

                if (
                    fullPath.Length < 2
                    || fullPath[1] != ':'
                )
                {
                    throw new InvalidOperationException($"[Assert] Normalized Windows path has no prefix: '{fullPath}'");
                }

                var prefix = fullPath.Substring(0, 2).ToUpper();
                var remainder = fullPath.Substring(2).TrimEndDirectorySeparators();

                if (remainder == string.Empty)
                    return prefix + OldPath.DirectorySeparatorChar;

                fullPath = prefix + remainder;
            }

            var normalized = fullPath.TrimEndDirectorySeparators();
            if(normalized == string.Empty)
                throw new InvalidOperationException("[Assert] Normalized Windows path is empty");
            return normalized;
        }

        /// <summary>
        /// Tells if the path must belong to an (existing or ficticious)
        /// directory, or if it is uncertain.
        /// </summary>
        /// <remarks>
        /// There are paths that can't be used to name anything but directories.
        ///
        /// E.g., paths like `.`, `..` and those ended by a directory separator
        /// must belong to a directory.
        ///
        /// This method won't check if the path leads to an actual directory.
        /// </remarks>
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

        /// <summary>The same as <see cref="System.IO.Path.GetFileName(string)"/>.</summary>
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
