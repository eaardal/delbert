using Akka.Actor;

namespace Delbert.Infrastructure
{
    public enum ActorPathType
    {
        Absolute, Relative
    }

    public static class ActorSystemExtensions
    {
        public static ActorSelection ActorSelection(this ActorSystem actorSystem, ActorMetadata actor, ActorPathType path = ActorPathType.Absolute)
        {
            return actorSystem.ActorSelection(actor.Path);
            //switch (path)
            //{
            //    case ActorPathType.Absolute:
            //        return actorSystem.ActorSelection(actor.AbsoluteUri);
            //    case ActorPathType.Relative:
            //        return actorSystem.ActorSelection(actor.RelativeUri);
            //    default:
            //        return actorSystem.ActorSelection(actor.AbsoluteUri);
            //}
        }

        public static ActorSelection ActorSelection(this IActorSystemAdapter actorSystem, ActorMetadata actor, ActorPathType path = ActorPathType.Absolute)
        {
            return actorSystem.ActorSelection(actor.Path);
            //switch (path)
            //{
            //    case ActorPathType.Absolute:
            //        return actorSystem.ActorSelection(actor.AbsoluteUri);
            //    case ActorPathType.Relative:
            //        return actorSystem.ActorSelection(actor.RelativeUri);
            //    default:
            //        return actorSystem.ActorSelection(actor.AbsoluteUri);
            //}
        }

        public static ActorSelection ActorSelection(this IUntypedActorContext context, ActorMetadata actor, ActorPathType path = ActorPathType.Absolute)
        {
            return context.ActorSelection(actor.Path);
            //switch (path)
            //{
            //    case ActorPathType.Absolute:
            //        return context.ActorSelection(actor.Path);
            //    case ActorPathType.Relative:
            //        return context.ActorSelection(actor.RelativeUri);
            //    default:
            //        return context.ActorSelection(actor.AbsoluteUri);
            //}
        }
    }
}
