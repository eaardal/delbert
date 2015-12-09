using Akka.Actor;
using Delbert.Infrastructure;

namespace Delbert.Actors
{
    public class ActorRegistry
    {
        public static ActorMetadata Notebook => new ActorMetadata(typeof(NotebookActor));
        public static ActorMetadata RootDirectory => new ActorMetadata("rootDirectory", typeof(RootDirectoryActor));
        public static ActorMetadata Section => new ActorMetadata(typeof(SectionActor));
        public static ActorMetadata Page => new ActorMetadata(typeof(PageActor));
        public static ActorMetadata CommandLineArgsParser => new ActorMetadata(typeof(CommandLineArgsParserActor));
        public static ActorMetadata Process => new ActorMetadata(typeof(ProcessActor));
        public static ActorMetadata Image => new ActorMetadata(typeof(ImageActor));
    }
}
