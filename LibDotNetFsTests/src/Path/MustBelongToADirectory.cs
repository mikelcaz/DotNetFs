// Copyright © 2018 Mikel Cazorla Pérez.

using System;
using Xunit;
using DotNetFs;

namespace DotNetFsTests
{
    public class MustBelongToADirectory
    {
        [Theory]
        [InlineData("/")]
        [InlineData("/directory/")]

        [InlineData(".")]
        [InlineData("..")]
        [InlineData("/directory/.")]
        [InlineData("/directory/..")]

        [InlineData("./")]
        [InlineData("../")]
        [InlineData("/directory/./")]
        [InlineData("/directory/../")]
        public void AbsolutePathBelongs(string path)
        {
            Assert.True(Path.MustBelongToADirectory(path));
        }

        [Theory]
        [InlineData("directory/")]
        [InlineData("./directory/")]
        [InlineData("../directory/")]

        [InlineData("directory/.")]
        [InlineData("directory/..")]
        [InlineData("./directory/.")]
        [InlineData("../directory/..")]

        [InlineData("directory/./")]
        [InlineData("directory/../")]
        [InlineData("./directory/./")]
        [InlineData("../directory/../")]
        public void RelativePathBelongs(string path)
        {
            Assert.True(Path.MustBelongToADirectory(path));
        }

        [Theory]
        // File or directory. It should not check the fs... but it does.
        [InlineData("/usr")]
        [InlineData("/fileOrDirectory")]
        public void AbsolutePathCouldBelong(string path)
        {
            Assert.False(Path.MustBelongToADirectory(path));
        }

        [Theory]
        [InlineData("fileOrDirectory")]
        [InlineData("directory/fileOrDirectory")]
        [InlineData("./directory/fileOrDirectory")]
        [InlineData("../directory/fileOrDirectory")]
        public void RelativePathCouldBelong(string path)
        {
            Assert.False(Path.MustBelongToADirectory(path));
        }

        [Theory]
        [InlineData(@"\")]
        // It should not check the fs... but it does.
        [InlineData(@"Z:\")]
        [InlineData(@"C:\")]
        public void BelongsOnWindowsToo(string path)
        {
            BelongsOnWindows(path);
        }

        [Theory]
        [InlineData("/")]
        // It should not check the fs... but it does.
        [InlineData("Z:/")]
        [InlineData("C:/")]
        public void BelongsOnWindowsTooWithSlashes(string path)
        {
            BelongsOnWindows(path);
        }

        internal void BelongsOnWindows(string path)
        {
            var isNotWindows =
                Environment.OSVersion.Platform != PlatformID.Win32NT;

            if (isNotWindows)
                return;

            Assert.True(Path.MustBelongToADirectory(path));
        }
    }
}
