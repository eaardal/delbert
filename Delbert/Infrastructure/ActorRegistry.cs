﻿using Akka.Actor;
using Delbert.Services;

namespace Delbert.Infrastructure
{
    public class ActorRegistry
    {
        public static ActorEntry Notebook => new ActorEntry(typeof(NotebookActor));
        public static ActorEntry RootDirectory => new ActorEntry("rootDirectory", typeof(RootDirectoryActor));
    }
}
