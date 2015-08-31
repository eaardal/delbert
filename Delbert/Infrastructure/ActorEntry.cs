using System;
using Akka.Actor;

namespace Delbert.Infrastructure
{
    public class ActorEntry
    {
        public string Name { get; private set; }
        public string AbsoluteUri { get; private set; }
        public string RelativeUri { get; private set; }
        public Type ActorType { get; private set; }

        public ActorEntry(string name, string absoluteUri, string relativeUri, Type actorType)
        {
            Name = name;
            AbsoluteUri = absoluteUri;
            RelativeUri = relativeUri;
            ActorType = actorType;
        }

        public ActorEntry(string name, Type actorType)
        {
            Name = name;
            ActorType = actorType;
        }

        public ActorEntry WithActorType(Type type)
        {
            ActorType = type;
            return this;
        }

        public ActorEntry(Type actorType)
        {
            if (actorType == null) throw new ArgumentNullException(nameof(actorType));
            ActorType = actorType;
        }

        public ActorEntry WithName(string name)
        {
            Name = name;
            return this;
        }

        public ActorEntry WithRelativeUrl(string url)
        {
            RelativeUri = url;
            return this;
        }

        public ActorEntry WithAbsoluteUrl(string url)
        {
            AbsoluteUri = url;
            return this;
        }

        public ActorEntry WithNameAsUrl(string baseUrl)
        {
            AbsoluteUri = baseUrl + Name;
            RelativeUri = "user/" + Name;
            return this;
        }
    }
}
