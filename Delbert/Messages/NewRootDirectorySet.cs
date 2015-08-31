using System;
using System.IO;

namespace Delbert.Messages
{
    internal class NewRootDirectorySet
    {
        public DirectoryInfo RootDirectory { get; }

        public NewRootDirectorySet(DirectoryInfo rootDirectory)
        {
            if (rootDirectory == null) throw new ArgumentNullException(nameof(rootDirectory));
            RootDirectory = rootDirectory;
        }
    }
}