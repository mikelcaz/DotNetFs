using System;
using System.Collections.Generic;
using Xunit;
using DotNetFs;

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
            "/directory/.", "/directory/otherDirectory/..",
            "/directory/./", "/directory/otherDirectory/../")]
        [InlineData(
            "directory",
            "./directory/.", "./directory/otherDirectory/..",
            "./directory/./", "./directory/otherDirectory/../",
            "directory/otherDirectory/..",
            "./directory/otherDirectory/..",
            "directory/./otherDirectory/..")]
        public void NormalizationTest(params string[] paths)
        {
            NormalizationTestHelper(onlyForWindows: false, paths: paths);
        }

        // This test is wrong (it cannot guess the current volume from the path)!
        [Theory]
        [InlineData(@"C:\", @"Z:\", @"\")]
        [InlineData(@"C:\", @"\", @"\.", @"\.\", @"\..", @"\..\", @"\directory\..")]
        [InlineData(@"C:\", "C:/", "/", "/.", "/./", "/..", "/../", "/directory/..")]
        [InlineData(
            @"\directory",
            @"\directory\.", @"\directory\otherDirectory\..",
            @"\directory\.\", @"\directory\otherDirectory\..\")]
        [InlineData(
            @"directory",
            @".\directory\.", @".\directory\otherDirectory\..",
            @".\directory\.\", @".\directory\otherDirectory\..\",
            @"directory\otherDirectory\..",
            @".\directory\otherDirectory\..",
            @"directory\.\otherDirectory\..")]
        public void NormalizationAdditionalTestOnWindows(params string[] paths)
        {
            NormalizationTestHelper(onlyForWindows: true, paths: paths);
        }

        internal void NormalizationTestHelper(
            bool onlyForWindows,
            params string[] paths
        )
        {
            if (paths.Length < 2)
                throw new ArgumentException("Paths cannot be compared if they are less than two.");

            var isWindows =
                Environment.OSVersion.Platform == PlatformID.Win32NT;

            if (onlyForWindows && !isWindows)
                return;

            var normalized = new string[paths.Length - 1];
            var referencePath = Path.GetNormalized(paths[0]);
            for (int i = 1; i < paths.Length; i++)
                normalized[i - 1] = Path.GetNormalized(paths[i]);

            foreach (var path in normalized)
                Assert.Equal(referencePath, path);
        }
    }
}
