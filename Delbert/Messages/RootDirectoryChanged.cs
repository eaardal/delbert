using System;
using System.IO;

namespace Delbert.Messages
{
    internal class RootDirectoryChanged
    {
        public DirectoryInfo RootDirectory { get; }

        public RootDirectoryChanged(DirectoryInfo rootDirectory)
        {
            if (rootDirectory == null) throw new ArgumentNullException(nameof(rootDirectory));
            RootDirectory = rootDirectory;
        }
    }
}