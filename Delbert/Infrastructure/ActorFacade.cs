using System;
using Akka.Actor;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Infrastructure
{
    public class ActorFacade
    {
        protected ILogger Logger { get; private set; }
        protected IActorSystemAdapter ActorSystem { get; private set; }

        public ActorFacade(IActorSystemAdapter actorSystem, ILogger logger)
        {
            if (actorSystem == null) throw new ArgumentNullException(nameof(actorSystem));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            ActorSystem = actorSystem;
            Logger = logger;
        }

        protected void LogFailure(object answer)
        {
            if (answer is Failure)
            {
                var failure = answer as Failure;
                Logger.Msg(this, l => l.Error(failure.Exception));
            }
        }
    }
}
