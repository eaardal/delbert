using System;
using System.Threading.Tasks;

namespace Delbert.Infrastructure
{
    public interface IMessageBus
    {
        /// <summary>
        /// Gets or sets the <see cref="MessageBus.PublishMethod"/>. Is <see cref="MessageBus.PublishMethod.SyncSameThread"/> by default.
        /// If no <see cref="MessageBus.PublishMethod"/> is specified when publishing a message/event, this default is used. 
        /// </summary>
        MessageBus.PublishMethod DefaultPublishMethod { get; set; }

        /// <summary>
        /// Publishes a message/event to subscribers using the <see cref="MessageBus.DefaultPublishMethod"/> specified globally for the <see cref="MessageBus"/>
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to publish</typeparam>
        /// <param name="message">The message/event to publish</param>
        void Publish<TMessage>(TMessage message);

        /// <summary>
        /// Publishes a message/event to subscribers using the <see cref="MessageBus.PublishMethod"/> specified
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to publish</typeparam>
        /// <param name="message">The message/event to publish</param>
        /// <param name="method">The <see cref="MessageBus.PublishMethod"/> to use when publishing the message to subscribers</param>
        void Publish<TMessage>(TMessage message, MessageBus.PublishMethod method);

        /// <summary>
        /// Subscribe to messages/events by registering a callback function
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to subscribe to</typeparam>
        /// <param name="message">The callback function to be called when receiving a published message/event</param>
        void Subscribe<TMessage>(Action<TMessage> message);

        /// <summary>
        /// Unsubscribes a specific callback function to messages of the given type
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to unsubscribe to</typeparam>
        /// <param name="message">The callback function to unsubscribe</param>
        void Unsubscribe<TMessage>(Action<TMessage> message);

        /// <summary>
        /// Unsubscribes all subscriptions of the given type
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to unsubscribe to</typeparam>
        void UnsubscribeAll<TMessage>();
    }

    /// <summary>
    /// A lightweight pub-sub MessageBus that handles publishing messages/events and subscribing to them.
    /// </summary>
    public class MessageBus : IMessageBus
    {
        public delegate void MessageDistributorEventHandler<in TMessage>(TMessage e);

        /// <summary>
        /// The publish method to use when publishing a message/event to subscribers
        /// </summary>
        public enum PublishMethod
        {
            /// <summary>
            /// Publishes the message/event synchronously on the same thread.
            /// </summary>
            SyncSameThread,

            /// <summary>
            /// Publishes the message/event asynchronously on another thread using the <see cref="System.Delegate"/> BeginInvoke() method
            /// </summary>
            BeginInvoke,

            /// <summary>
            /// Publishes the message/event asynchronously by creating and starting a new <see cref="System.Threading.Tasks.Task"/> using Task.Factory.StartNew() which is then handled by the ThreadPool.
            /// The newly created task is not awaited, and is as such a fire-and-forget task.
            /// </summary>
            CreateAndStartTask
        }
        
        /// <summary>
        /// Gets or sets the <see cref="PublishMethod"/>. Is <see cref="PublishMethod.SyncSameThread"/> by default.
        /// If no <see cref="PublishMethod"/> is specified when publishing a message/event, this default is used. 
        /// </summary>
        public PublishMethod DefaultPublishMethod { get; set; }

        /// <summary>
        /// Creates a new MessageBus
        /// </summary>
        public MessageBus()
        {
            DefaultPublishMethod = PublishMethod.BeginInvoke;
        }

        /// <summary>
        /// Publishes a message/event to subscribers using the <see cref="DefaultPublishMethod"/> specified globally for the <see cref="MessageBus"/>
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to publish</typeparam>
        /// <param name="message">The message/event to publish</param>
        public void Publish<TMessage>(TMessage message)
        {
            MessageDistributor<TMessage>.Publish(message, DefaultPublishMethod);
        }

        /// <summary>
        /// Publishes a message/event to subscribers using the <see cref="PublishMethod"/> specified
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to publish</typeparam>
        /// <param name="message">The message/event to publish</param>
        /// <param name="method">The <see cref="PublishMethod"/> to use when publishing the message to subscribers</param>
        public void Publish<TMessage>(TMessage message, PublishMethod method)
        {
            MessageDistributor<TMessage>.Publish(message, method);
        }

        /// <summary>
        /// Subscribe to messages/events by registering a callback function
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to subscribe to</typeparam>
        /// <param name="message">The callback function to be called when receiving a published message/event</param>
        public void Subscribe<TMessage>(Action<TMessage> message)
        {
            MessageDistributor<TMessage>.MessageSent += message.Invoke;
        }

        /// <summary>
        /// Unsubscribes a specific callback function to messages of the given type
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to unsubscribe to</typeparam>
        /// <param name="message">The callback function to unsubscribe</param>
        public void Unsubscribe<TMessage>(Action<TMessage> message)
        {
            MessageDistributor<TMessage>.MessageSent -= message.Invoke;
        }

        /// <summary>
        /// Unsubscribes all subscriptions of the given type
        /// </summary>
        /// <typeparam name="TMessage">The type of message/event to unsubscribe to</typeparam>
        public void UnsubscribeAll<TMessage>()
        {
            MessageDistributor<TMessage>.UnsubscribeAll();
        }

        private class MessageDistributor<TMessage>
        {
            public static event MessageDistributorEventHandler<TMessage> MessageSent;

            public static void UnsubscribeAll()
            {
                MessageSent = null;
            }

            public static void Publish(TMessage message, PublishMethod method)
            {
                switch (method)
                {
                    case PublishMethod.SyncSameThread:
                        DoPublishSynchronouslyOnSameThread(message);
                        break;
                    case PublishMethod.CreateAndStartTask:
                        DoPublishAsynchronouslyByStartingNewTask(message);
                        break;
                    case PublishMethod.BeginInvoke:
                        DoPublishAsynchronouslyByBeginInvoke(message);
                        break;
                }
            }

            private static void DoPublishAsynchronouslyByBeginInvoke(TMessage message)
            {
                if (MessageSent != null)
                {
                    var receivers = MessageSent.GetInvocationList();
                    foreach (var @delegate in receivers)
                    {
                        var receiver = (MessageDistributorEventHandler<TMessage>) @delegate;
                        receiver.BeginInvoke(message, null, null);
                    }
                }
            }

            private static void DoPublishAsynchronouslyByStartingNewTask(TMessage message)
            {
                // Thanks to Iddillian @ http://stackoverflow.com/questions/18881808/raising-events-on-separate-thread

                MessageDistributorEventHandler<TMessage> handler = MessageSent;
                if (handler != null)
                {
                    var invocationList = handler.GetInvocationList();

                    Task.Factory.StartNew(() =>
                    {
                        foreach (MessageDistributorEventHandler<TMessage> h in invocationList)
                        {
                            h.Invoke(message);
                        }
                    });
                }
            }

            private static void DoPublishSynchronouslyOnSameThread(TMessage message)
            {
                if (MessageSent != null)
                    MessageSent.Invoke(message);
            }
        }
    }
}
