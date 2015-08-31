using Akka.Actor;
using Delbert.Services;

namespace Delbert.Infrastructure
{
    public class ActorRegistry
    {
        public static ActorEntry Notebook => new ActorEntry("notebook", Props.Create<NotebookActor>());
        public static ActorEntry RootDirectory => new ActorEntry("rootDirectory", Props.Create<RootDirectoryActor>());
    }
}
