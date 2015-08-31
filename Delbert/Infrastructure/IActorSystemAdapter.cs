using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Dispatch;
using Akka.Event;
using Akka.Serialization;

namespace Delbert.Infrastructure
{
    public interface IActorSystemAdapter
    {
        IActorRef DeadLetters { get; }
        Dispatchers Dispatchers { get; }
        EventStream EventStream { get; }
        ILoggingAdapter Log { get; }
        Mailboxes Mailboxes { get; }
        string Name { get; }
        IScheduler Scheduler { get; }
        Serialization Serialization { get; }
        Settings Settings { get; }
        Task TerminationTask { get; }

        IActorRef ActorOf(Props props, string name = null);
        ActorSelection ActorSelection(string actorPath);
        ActorSelection ActorSelection(Akka.Actor.ActorPath actorPath);
        void AwaitTermination();
        bool AwaitTermination(TimeSpan timeout);
        bool AwaitTermination(TimeSpan timeout, CancellationToken cancellationToken);
        void Dispose();
        object GetExtension(IExtensionId extensionId);
        T GetExtension<T>() where T : class, IExtension;
        bool HasExtension(Type t);
        bool HasExtension<T>() where T : class, IExtension;
        object RegisterExtension(IExtensionId extension);
        void Shutdown();
        void Stop(IActorRef actor);
        bool TryGetExtension(Type extensionType, out object extension);
        bool TryGetExtension<T>(out T extension) where T : class, IExtension;
    }
}