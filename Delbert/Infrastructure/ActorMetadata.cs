using System;
using Akka.Actor;

namespace Delbert.Infrastructure
{
    public class ActorMetadata
    {
        public string Name { get; }
        public string Path { get; }
        public ActorMetadata Parent { get; }
        public Type ActorType { get; }

        public ActorMetadata(Type actorType, ActorMetadata parent = null) : this(null, actorType, parent) { }

        public ActorMetadata(string name, Type actorType, ActorMetadata parent = null) : this(name, parent)
        {
            ActorType = actorType;
        }

        public ActorMetadata(string name, ActorMetadata parent = null)
        {
            Name = name;

            var parentPath = parent != null ? parent.Path : "/user";
            Path = $"{parentPath}/{Name}";

            Parent = parent;
        }

        public string NameWithArgs(params object[] args)
        {
            return string.Format(Name, args);
        }
    }
}
