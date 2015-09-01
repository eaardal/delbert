using System.IO;

namespace Delbert.Messages
{
    internal class RootDirectoryChanged
    {
        public RootDirectoryChanged(DirectoryInfo directory)
        {
            Directory = directory;
        }

        public DirectoryInfo Directory { get; }
    }
}