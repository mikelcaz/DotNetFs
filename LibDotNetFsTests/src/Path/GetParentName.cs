// Copyright © 2018 Mikel Cazorla Pérez.

using System;
using Xunit;
using DotNetFs;

namespace DotNetFsTests
{
    // TODO:
    // Naive platform-specific detection.

    // TODO:
    // Lines commented out belongs to the proper implementation.
    // Below them there is always a line based on a naive trick.
    // Replace them when the proper implementation is done.
    public class GetParentName
    {
        [Theory]
        [InlineData("/", "/")]
        [InlineData("/", "/fileOrDirectory")]
        // [InlineData("/", "/directory/")]
        [InlineData("/directory/..", "/directory/")]
        [InlineData("/parent", "/parent/fileOrDirectory")]
        // [InlineData("/parent", "/parent/directory/")]
        [InlineData("/parent/directory/..", "/parent/directory/")]
        [InlineData("/parent/directory/./..", "/parent/directory/.")]
        // [InlineData("/parent", "/parent/directory/././")]
        [InlineData("/parent/directory/././..", "/parent/directory/././")]
        // [InlineData("/parent", "/parent/directory/..")]
        [InlineData("/parent/directory/../..", "/parent/directory/..")]
        // [InlineData("/parent", "/parent/directory/../")]
        [InlineData("/parent/directory/../..", "/parent/directory/../")]
        public void AbsolutePathTest(string expected, string path)
        {
            ParentNamePathTest(expected, path, forWindows: false);
        }

        [Theory]
        // [InlineData("..", ".")]
        [InlineData("./..", ".")]
        // [InlineData("..", "./")]
        [InlineData("./..", "./")]
        [InlineData(".", "./fileOrDirectory")]
        // [InlineData(".", "./fileOrDirectory/")]
        [InlineData("./fileOrDirectory/..", "./fileOrDirectory/")]
        // [InlineData(".", "./fileOrDirectory/./.")]
        [InlineData("./fileOrDirectory/././..", "./fileOrDirectory/./.")]
        // [InlineData(".", "./fileOrDirectory/././")]
        [InlineData("./fileOrDirectory/././..", "./fileOrDirectory/././")]
        // [InlineData("./fileOrDirectory", "./fileOrDirectory/..")]
        [InlineData("./fileOrDirectory/../..", "./fileOrDirectory/..")]
        // [InlineData("./fileOrDirectory", "./fileOrDirectory/../")]
        [InlineData("./fileOrDirectory/../..", "./fileOrDirectory/../")]
        [InlineData(".", "fileOrDirectory")]
        public void RelativePathTest(string expected, string path)
        {
            ParentNamePathTest(expected, path, forWindows: false);
        }

        [Theory]
        [InlineData(@"\", @"\")]
        // It should not check the fs... but it does.
        [InlineData(@"Z:\", @"Z:\")]
        [InlineData(@"C:\", @"C:\")]
        [InlineData(@"\", @"\fileOrDirectory")]
        [InlineData(@"C:\", @"C:\fileOrDirectory")]
        // [InlineData(@"\", @"\directory\")]
        [InlineData(@"\directory\..", @"\directory\")]
        [InlineData(@"\parent", @"\parent\fileOrDirectory")]
        // [InlineData(@"\parent", @"\parent\directory\")]
        [InlineData(@"\parent\directory\..", @"\parent\directory\")]
        [InlineData(@"\parent\directory\.\..", @"\parent\directory\.")]
        // [InlineData(@"\parent", @"\parent\directory\.\.\")]
        [InlineData(@"\parent\directory\.\.\..", @"\parent\directory\.\.\")]
        // [InlineData(@"\parent", @"\parent\directory\..")]
        [InlineData(@"\parent\directory\..\..", @"\parent\directory\..")]
        // [InlineData(@"\parent", @"\parent\directory\..\")]
        [InlineData(@"\parent\directory\..\..", @"\parent\directory\..\")]
        public void WindowsAbsolutePathTest(string expected, string path)
        {
            ParentNamePathTest(expected, path, forWindows: true);
        }

        [Theory]
        // [InlineData(@"..", @".")]
        [InlineData(@".\..", @".")]
        // [InlineData(@"..", @".\")]
        [InlineData(@".\..", @".\")]
        [InlineData(@".", @".\fileOrDirectory")]
        // [InlineData(@".", @".\fileOrDirectory\")]
        [InlineData(@".\fileOrDirectory\..", @".\fileOrDirectory\")]
        // [InlineData(@".", @".\fileOrDirectory\.\.")]
        [InlineData(@".\fileOrDirectory\.\.\..", @".\fileOrDirectory\.\.")]
        // [InlineData(@".", @".\fileOrDirectory\.\.\")]
        [InlineData(@".\fileOrDirectory\.\.\..", @".\fileOrDirectory\.\.\")]
        // [InlineData(@".\fileOrDirectory", @".\fileOrDirectory\..")]
        [InlineData(@".\fileOrDirectory\..\..", @".\fileOrDirectory\..")]
        // [InlineData(@".\fileOrDirectory", @".\fileOrDirectory\..\")]
        [InlineData(@".\fileOrDirectory\..\..", @".\fileOrDirectory\..\")]
        [InlineData(".", "fileOrDirectory")]
        public void WindowsRelativePathTest(string expected, string path)
        {
            ParentNamePathTest(expected, path, forWindows: true);
        }

        internal void ParentNamePathTest(
            string expected,
            string path,
            bool forWindows
        )
        {
            var isWindows =
                Environment.OSVersion.Platform == PlatformID.Win32NT;

            if (isWindows != forWindows)
                return;

            var parent = Path.GetParentName(path);
            Assert.Equal(expected, parent);
        }
    }
}
