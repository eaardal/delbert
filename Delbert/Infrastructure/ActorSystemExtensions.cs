using Akka.Actor;

namespace Delbert.Infrastructure
{
    public enum ActorPathType
    {
        Absolute, Relative
    }

    public static class ActorSystemExtensions
    {
        public static ActorSelection ActorSelection(this ActorSystem actorSystem, ActorEntry actorEntry, ActorPathType path = ActorPathType.Absolute)
        {
            switch (path)
            {
                case ActorPathType.Absolute:
                    return actorSystem.ActorSelection(actorEntry.AbsoluteUri);
                case ActorPathType.Relative:
                    return actorSystem.ActorSelection(actorEntry.RelativeUri);
                default:
                    return actorSystem.ActorSelection(actorEntry.AbsoluteUri);
            }
        }

        public static ActorSelection ActorSelection(this IActorSystemAdapter actorSystem, ActorEntry actorEntry, ActorPathType path = ActorPathType.Absolute)
        {
            switch (path)
            {
                case ActorPathType.Absolute:
                    return actorSystem.ActorSelection(actorEntry.AbsoluteUri);
                case ActorPathType.Relative:
                    return actorSystem.ActorSelection(actorEntry.RelativeUri);
                default:
                    return actorSystem.ActorSelection(actorEntry.AbsoluteUri);
            }
        }

        public static ActorSelection ActorSelection(this IUntypedActorContext context, ActorEntry actor, ActorPathType path = ActorPathType.Absolute)
        {
            switch (path)
            {
                case ActorPathType.Absolute:
                    return context.ActorSelection(actor.AbsoluteUri);
                case ActorPathType.Relative:
                    return context.ActorSelection(actor.RelativeUri);
                default:
                    return context.ActorSelection(actor.AbsoluteUri);
            }
        }
    }
}
