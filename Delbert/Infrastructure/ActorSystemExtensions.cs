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
                    return actorSystem.ActorSelection(actorEntry.AbsoluteUrl);
                case ActorPathType.Relative:
                    return actorSystem.ActorSelection(actorEntry.RelativeUrl);
                default:
                    return actorSystem.ActorSelection(actorEntry.AbsoluteUrl);
            }
        }

        public static ActorSelection ActorSelection(this IActorSystemAdapter actorSystem, ActorEntry actorEntry, ActorPathType path = ActorPathType.Absolute)
        {
            switch (path)
            {
                case ActorPathType.Absolute:
                    return actorSystem.ActorSelection(actorEntry.AbsoluteUrl);
                case ActorPathType.Relative:
                    return actorSystem.ActorSelection(actorEntry.RelativeUrl);
                default:
                    return actorSystem.ActorSelection(actorEntry.AbsoluteUrl);
            }
        }
    }
}
