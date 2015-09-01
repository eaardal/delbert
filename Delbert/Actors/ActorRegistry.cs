using Akka.Actor;
using Delbert.Infrastructure;

namespace Delbert.Actors
{
    public class ActorRegistry
    {
        public static ActorMetadata Notebook => new ActorMetadata(typeof(NotebookActor));
        public static ActorMetadata RootDirectory => new ActorMetadata("rootDirectory", typeof(RootDirectoryActor));
        public static ActorMetadata NotebookSection => new ActorMetadata(typeof(NotebookSectionActor));
    }
}
