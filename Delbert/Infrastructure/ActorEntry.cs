using Akka.Actor;

namespace Delbert.Infrastructure
{
    public class ActorEntry
    {
        public string Name { get; private set; }
        public string AbsoluteUrl { get; private set; }
        public string RelativeUrl { get; private set; }
        public Props Props { get; private set; }

        public ActorEntry(string name, string absoluteUrl, string relativeUrl)
        {
            Name = name;
            AbsoluteUrl = absoluteUrl;
        }

        public ActorEntry()
        {
            
        }

        public ActorEntry(string name, Props props)
        {
            Name = name;
            Props = props;
        }

        public ActorEntry WithName(string name)
        {
            Name = name;
            return this;
        }

        public ActorEntry WithRelativeUrl(string url)
        {
            RelativeUrl = url;
            return this;
        }

        public ActorEntry WithAbsoluteUrl(string url)
        {
            AbsoluteUrl = url;
            return this;
        }

        public ActorEntry WithNameAsUrl(string baseUrl)
        {
            AbsoluteUrl = baseUrl + Name;
            RelativeUrl = "user/" + Name;
            return this;
        }
    }
}
