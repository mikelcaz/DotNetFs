// Copyright © 2018 Mikel Cazorla Pérez.

using System;
using System.Collections.Generic;
using Xunit;
using DotNetFs;

using OldPath = System.IO.Path;

namespace DotNetFsTests
{
    // TODO:
    // Naive platform-specific detection.
    public class GetNormalized
    {
        [Theory]
        [InlineData("/", "/.", "/./", "/..", "/../", "/directory/..")]
        [InlineData(
            "/directory",
            // Only trailing duplicated slashes are removed.
            "/directory///",
            "/directory/.", "/directory/otherDirectory/..",
            "/directory/./", "/directory/otherDirectory/../")]
        [InlineData(
            "directory",
            // Only trailing duplicated slashes are removed.
            "directory///",
            "./directory/.", "./directory/otherDirectory/..",
            "./directory/./", "./directory/otherDirectory/../",
            "directory/otherDirectory/..",
            "./directory/otherDirectory/..",
            "directory/./otherDirectory/..")]
        public void NormalizationTest(params string[] paths)
        {
            NormalizationTestHelper(onlyForWindows: false, equal: true, paths: paths);
        }

        [Theory]
        [InlineData("/name", "/NAME")]
        [InlineData("name", "NAME")]
        public void NormalizationFailureTest(params string[] paths)
        {
            NormalizationTestHelper(onlyForWindows: false, equal: false, paths: paths);
        }

        [Theory]
        [InlineData(
            @"\directory",
            // Only trailing duplicated slashes are removed.
            @"\directory\\\",
            @"\directory\.", @"\directory\otherDirectory\..",
            @"\directory\.\", @"\directory\otherDirectory\..\")]
        [InlineData(
            @"directory",
            // Only trailing duplicated slashes are removed.
            @"directory\\\",
            @".\directory\.", @".\directory\otherDirectory\..",
            @".\directory\.\", @".\directory\otherDirectory\..\",
            @"directory\otherDirectory\..",
            @".\directory\otherDirectory\..",
            @"directory\.\otherDirectory\..")]
        public void NormalizationWithBackslashOnWindowsTest(params string[] paths)
        {
            NormalizationTestHelper(onlyForWindows: true, equal: true, paths: paths);
        }

        [Theory]
        [InlineData(@"\name", @"\NAME")]
        public void NormalizationFailureWithBackslashOnWindowsTest(params string[] paths)
        {
            NormalizationTestHelper(onlyForWindows: true, equal: false, paths: paths);
        }

        internal static string CurrentDriveRoot =>
            OldPath.GetPathRoot(
                Environment.CurrentDirectory
            ).ToUpper();

        internal static string CurrentDrivePrefix =>
            CurrentDriveRoot.TrimEnd(
                OldPath.DirectorySeparatorChar,
                OldPath.AltDirectorySeparatorChar
            );

        internal static string AnotherDriveRoot
        {
            get
            {
                var c = OldPath.GetPathRoot(@"C:\");

                if (CurrentDriveRoot != c)
                    return c;

                return OldPath.GetPathRoot(@"A:\");
            }
        }

        public static IEnumerable<object[]> WindowsNormalizationTestData =>
            new List<object[]>
            {
                new object[] {@"C:\", @"c:\", "C:/", "c:/"},
                new object[] {@"C:\fileOrDirectory", @"c:\fileOrDirectory"},
                new object[] {Environment.CurrentDirectory, CurrentDrivePrefix},
                new object[] {CurrentDriveRoot, @"\", @"\.", @"\.\", @"\..", @"\..\", @"\directory\.."},
                new object[] {CurrentDriveRoot, "/", "/.", "/./", "/..", "/../", "/directory/.."}
            };

        public static IEnumerable<object[]> WindowsNormalizationFailureTestData =>
            new List<object[]>
            {
                new object[] {Environment.CurrentDirectory, CurrentDriveRoot},
                new object[] {Environment.CurrentDirectory, AnotherDriveRoot},
                new object[] {@"C:\name", @"C:\NAME"},
                new object[] {@"C:\name", @"c:\NAME"},
                new object[] {CurrentDriveRoot, AnotherDriveRoot},
                new object[] {AnotherDriveRoot, @"\"}
            };

        public static bool IsWindows =>
            Environment.OSVersion.Platform == PlatformID.Win32NT;

        [Theory]
        [MemberData(nameof(WindowsNormalizationTestData))]
        public void WindowsNormalizationTest(params string[] paths)
        {
            NormalizationTestHelper(onlyForWindows: true, equal: true, paths: paths);
        }

        [Theory]
        [MemberData(nameof(WindowsNormalizationFailureTestData))]
        public void WindowsNormalizationFailureTest(params string[] paths)
        {
            NormalizationTestHelper(onlyForWindows: true, equal: false, paths: paths);
        }

        [Fact]
        public void NormalizationOutOfWindowsWhenRootTest()
        {
            if (IsWindows)
                return;

            var normalized = Path.GetNormalized("/");
            Assert.Equal("/", normalized);
        }

        [Fact]
        public void WindowsNormalizationFailureWhenRootTest()
        {
            if (!IsWindows)
                return;

            var normalized = Path.GetNormalized(@"C:\");
            Assert.NotEqual("C:", normalized);

            // Paranoid mode test.
            Assert.NotEqual("c:", normalized);
        }

        [Theory]
        [InlineData(@" C:\")]
        [InlineData("fileOrDirectory:dangerousName")]
        [InlineData(@"C:\fileOrDirectory:dangerousName")]
        public void WindowsWrongDriveLetterTest(string path)
        {
            if (!IsWindows)
                return;

            Assert.Throws<ArgumentException>(() => Path.GetNormalized(path));
        }

        internal delegate void Assertion(params string[] strings);

        internal void AssertEquals(params string[] strings)
        {
            if (strings.Length < 2)
                throw new ArgumentException("Strings cannot be compared if they are less than two.", paramName: "strings");

            var referencePath = strings[0];

            for (int i = 1; i < strings.Length; i++)
                Assert.Equal(referencePath, strings[i]);
        }

        internal void AssertNotEquals(params string[] strings)
        {
            if (strings.Length < 2)
                throw new ArgumentException("Strings cannot be compared if they are less than two.", paramName: "strings");

            for (int j = 0; j < strings.Length; j++)
                for (int i = j + 1; i < strings.Length; i++)
                    Assert.NotEqual(strings[j], strings[i]);
        }

        internal void NormalizationTestHelper(
            bool onlyForWindows,
            bool equal,
            params string[] paths
        )
        {
            if (onlyForWindows && !IsWindows)
                return;

            var normalized = new string[paths.Length];
            for (int i = 0; i < paths.Length; i++)
                normalized[i] = Path.GetNormalized(paths[i]);

            Assertion assertion;
            if (equal)
                assertion = AssertEquals;
            else
                assertion = AssertNotEquals;

            try
            {
                assertion(normalized);
            }
            catch (Exception e)
            {
                for (var i = 0; i < paths.Length; i++)
                {
                    System.Console.WriteLine($"'{paths[i]}' -> '{normalized[i]}'");
                }

                throw e;
            }
        }
    }
}
