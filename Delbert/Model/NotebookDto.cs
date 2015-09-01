using System.IO;
using Delbert.Infrastructure;

namespace Delbert.Model
{
    public class NotebookDto : NotifyPropertyChangedBase
    {
        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
    }
}