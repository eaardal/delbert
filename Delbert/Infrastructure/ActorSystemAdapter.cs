using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Dispatch;
using Akka.Event;
using Akka.Serialization;

namespace Delbert.Infrastructure
{
    /// <summary>
    ///     An actor system is a hierarchical group of actors which share common
    ///     configuration, e.g. dispatchers, deployments, remote capabilities and
    ///     addresses. It is also the entry point for creating or looking up actors.
    ///     There are several possibilities for creating actors (see [[Akka.Actor.Props]]
    ///     for details on `props`):
    ///     <code>
    /// system.ActorOf(props, "name");
    /// system.ActorOf(props);
    /// system.ActorOf(Props.Create(typeof(MyActor)), "name");
    /// system.ActorOf(Props.Create(() =&gt; new MyActor(arg1, arg2), "name");
    /// </code>
    ///     Where no name is given explicitly, one will be automatically generated.
    ///     <b>
    ///         <i>Important Notice:</i>
    ///     </b>
    ///     This class is not meant to be extended by user code.
    /// </summary>
    public class ActorSystemAdapter : IDisposable, IActorSystemAdapter
    {
        private readonly ActorSystem _actorSystem;

        public ActorSystemAdapter(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }


        /// <summary>Gets the settings.</summary>
        /// <value>The settings.</value>
        public Settings Settings { get { return _actorSystem.Settings; } }

        /// <summary>Gets the name of this system.</summary>
        /// <value>The name.</value>
        public string Name { get { return _actorSystem.Name; } }

        /// <summary>Gets the serialization.</summary>
        /// <value>The serialization.</value>
        public Serialization Serialization { get { return _actorSystem.Serialization; } }

        /// <summary>Gets the event stream.</summary>
        /// <value>The event stream.</value>
        public EventStream EventStream { get { return _actorSystem.EventStream; } }

        /// <summary>
        ///     Gets the dead letters.
        /// </summary>
        /// <value>The dead letters.</value>
        public IActorRef DeadLetters { get { return _actorSystem.DeadLetters; } }

        /// <summary>Gets the dispatchers.</summary>
        /// <value>The dispatchers.</value>
        public Dispatchers Dispatchers { get { return _actorSystem.Dispatchers; } }

        /// <summary>Gets the mailboxes.</summary>
        /// <value>The mailboxes.</value>
        public Mailboxes Mailboxes { get { return _actorSystem.Mailboxes; } }


        /// <summary>Gets the scheduler.</summary>
        /// <value>The scheduler.</value>
        public IScheduler Scheduler { get { return _actorSystem.Scheduler; } }

        /// <summary>Gets the log</summary>
        public ILoggingAdapter Log { get { return _actorSystem.Log; } }

        /// <summary>
        /// Returns an extension registered to this ActorSystem
        /// </summary>
        public object GetExtension(IExtensionId extensionId)
        {
            return _actorSystem.GetExtension(extensionId);
        }

        /// <summary>
        /// Returns an extension registered to this ActorSystem
        /// </summary>
        public T GetExtension<T>() where T : class, IExtension
        {
            return _actorSystem.GetExtension<T>();
        }

        /// <summary>
        /// Determines whether this instance has the specified extension.
        /// </summary>
        public bool HasExtension(Type t)
        {
            return _actorSystem.HasExtension(t);
        }

        /// <summary>
        /// Determines whether this instance has the specified extension.
        /// </summary>
        public bool HasExtension<T>() where T : class, IExtension
        {
            return _actorSystem.HasExtension<T>();
        }

        /// <summary>
        /// Tries to the get the extension of specified type.
        /// </summary>
        public bool TryGetExtension(Type extensionType, out object extension)
        {
            return TryGetExtension(extensionType, out extension);
        }

        /// <summary>
        /// Tries to the get the extension of specified type.
        /// </summary>
        public bool TryGetExtension<T>(out T extension) where T : class, IExtension
        {
            return TryGetExtension<T>(out extension);
        }


        /// <summary>
        ///     Stop this actor system. This will stop the guardian actor, which in turn
        ///     will recursively stop all its child actors, then the system guardian
        ///     (below which the logging actors reside) and the execute all registered
        ///     termination handlers (<see cref="ActorSystem.RegisterOnTermination" />).
        /// </summary>
        public void Shutdown()
        {
            _actorSystem.Shutdown();
        }

        /// <summary>
        /// Returns a task that will be completed when the system has terminated.
        /// </summary>
        public Task TerminationTask { get { return _actorSystem.TerminationTask; } }

        /// <summary>
        /// Block current thread until the system has been shutdown.
        /// This will block until after all on termination callbacks have been run.
        /// </summary>
        public void AwaitTermination()
        {
            _actorSystem.AwaitTermination();
        }

        /// <summary>
        /// Block current thread until the system has been shutdown, or the specified
        /// timeout has elapsed. 
        /// This will block until after all on termination callbacks have been run.
        /// <para>Returns <c>true</c> if the system was shutdown during the specified time;
        /// <c>false</c> if it timed out.</para>
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Returns <c>true</c> if the system was shutdown during the specified time;
        /// <c>false</c> if it timed out.</returns>
        public bool AwaitTermination(TimeSpan timeout)
        {
            return _actorSystem.AwaitTermination(timeout);
        }

        /// <summary>
        /// Block current thread until the system has been shutdown, or the specified
        /// timeout has elapsed, or the cancellationToken was canceled. 
        /// This will block until after all on termination callbacks have been run.
        /// <para>Returns <c>true</c> if the system was shutdown during the specified time;
        /// <c>false</c> if it timed out, or the cancellationToken was canceled. </para>
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="cancellationToken">A cancellation token that cancels the wait operation.</param>
        /// <returns>Returns <c>true</c> if the system was shutdown during the specified time;
        /// <c>false</c> if it timed out, or the cancellationToken was canceled. </returns>
        public bool AwaitTermination(TimeSpan timeout, CancellationToken cancellationToken)
        {
            return _actorSystem.AwaitTermination(timeout, cancellationToken);
        }


        public void Stop(IActorRef actor)
        {
            _actorSystem.Stop(actor);
        }
        
        public void Dispose()
        {
            _actorSystem.Dispose();
        }
        
        public object RegisterExtension(IExtensionId extension)
        {
            return _actorSystem.RegisterExtension(extension);
        }

        public IActorRef ActorOf(Props props, string name = null)
        {
            return _actorSystem.ActorOf(props, name);
        }

        public ActorSelection ActorSelection(Akka.Actor.ActorPath actorPath)
        {
            return _actorSystem.ActorSelection(actorPath);
        }

        public ActorSelection ActorSelection(string actorPath)
        {
            return _actorSystem.ActorSelection(actorPath);
        }
    }
}