using System;
using System.Threading.Tasks;
using Akka.Actor;

namespace Delbert.Infrastructure
{
    public interface IActorSelectionAdapter
    {
        IActorRef Anchor { get; }
        SelectionPathElement[] Path { get; set; }
        string PathString { get; }
        void Tell(object message);
        void Tell(object message, IActorRef sender);
        Task<IActorRef> ResolveOne(TimeSpan timeout);
    }

    public class ActorSelectionAdapter : IActorSelectionAdapter
    {
        private readonly ActorSelection _actorSelection;

        public ActorSelectionAdapter(ActorSelection actorSelection)
        {
            if (actorSelection == null) throw new ArgumentNullException(nameof(actorSelection));
            _actorSelection = actorSelection;
        }

        public IActorRef Anchor
        {
            get { return _actorSelection.Anchor; }
        }

        public SelectionPathElement[] Path
        {
            get { return _actorSelection.Path; }
            set { _actorSelection.Path = value; }
        }

        public string PathString
        {
            get { return _actorSelection.PathString; }
        }

        public void Tell(object message)
        {
            _actorSelection.Tell(message);
        }

        public void Tell(object message, IActorRef sender)
        {
            _actorSelection.Tell(message, sender);
        }

        public Task<IActorRef> ResolveOne(TimeSpan timeout)
        {
            return _actorSelection.ResolveOne(timeout);
        }
    }
}
