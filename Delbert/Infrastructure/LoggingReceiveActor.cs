using System;
using Akka.Actor;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Infrastructure
{
    public abstract class LoggingReceiveActor : ReceiveActor
    {
        protected ILogger Log { get; private set; }

        public LoggingReceiveActor(ILogger log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));
            Log = log;
        }

        protected override bool AroundReceive(Receive receive, object message)
        {
            Log.Msg(this, l => l.Debug("{0} received {1}", GetType().Name, message.GetType().Name));

            return base.AroundReceive(receive, message);
        }
    }
}
