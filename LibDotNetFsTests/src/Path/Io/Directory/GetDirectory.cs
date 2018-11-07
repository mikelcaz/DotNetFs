using Xunit;
using DotNetFs;
using DotNetFs.Io;

using OldPath = System.IO.Path;

namespace DotNetFsTests
{
    public class GetDirectory
    {
        [Fact]
        public void DirectoryInfoTest()
        {
            var directory = "data/directory";
            var file = Path.Combine(directory, "file");

            Assert.False(DosFile.Exists(directory));
            Assert.True(Directory.Exists(directory));

            Assert.True(DosFile.Exists(file));
            Assert.False(Directory.Exists(file));

            var pathFromDirectory = OldPath.GetFullPath
            (
                Directory.GetDirectory(directory)
                .Name
            );
            var pathFromFile = OldPath.GetFullPath
            (
                Directory.GetDirectory(file)
                .Name
            );

            Assert.Equal(
                pathFromDirectory,
                pathFromFile);
        }
    }
}
