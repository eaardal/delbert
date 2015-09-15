using System.IO;

namespace Delbert.Actors.Facades.Abstract
{
    public interface IProcessFacade
    {
        void StartProcessForFile(FileInfo file);
    }
}
