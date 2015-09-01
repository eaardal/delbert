using System;
using System.IO;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Messages;

namespace Delbert.Actors
{
    class RootDirectoryActor : LoggingReceiveActor
    {
        private readonly IMessageBus _messageBus;
        private DirectoryInfo _rootDirectory;

        public RootDirectoryActor(ILogger log, IMessageBus messageBus) : base(log)
        {
            if (messageBus == null) throw new ArgumentNullException(nameof(messageBus));
            _messageBus = messageBus;

            Receive<SetRootDirectory>(msg => OnSetRootDirectory(msg));
        }

        #region Become

        private void Configured()
        {
            Receive<GetRootDirectory>(msg => OnGetRootDirectory(msg));
            Receive<SetRootDirectory>(msg => OnSetRootDirectory(msg));
        }

        #endregion

        #region Message Handlers

        private void OnSetRootDirectory(SetRootDirectory msg)
        {
            _rootDirectory = msg.RootDirectory;

            _messageBus.Publish(new NewRootDirectorySet(_rootDirectory));

            Become(Configured);
        }

        private void OnGetRootDirectory(GetRootDirectory msg)
        {
            if (_rootDirectory != null)
            {
                Sender.Tell(new GetRootDirectoryResult(_rootDirectory), Self);
            }
            else
            {
                throw new Exception("Root directory not set before trying to receive it");
            }
        }

        #endregion

        #region Messages

        internal class SetRootDirectory
        {
            public SetRootDirectory(DirectoryInfo rootDirectory)
            {
                if (rootDirectory == null) throw new ArgumentNullException(nameof(rootDirectory));
                RootDirectory = rootDirectory;
            }

            public DirectoryInfo RootDirectory { get; }
        }

        internal class GetRootDirectory { }

        public class GetRootDirectoryResult
        {
            public GetRootDirectoryResult(DirectoryInfo currentRootDirectory)
            {
                if (currentRootDirectory == null) throw new ArgumentNullException(nameof(currentRootDirectory));
                CurrentRootDirectory = currentRootDirectory;
            }

            public DirectoryInfo CurrentRootDirectory { get; }
        }

        #endregion
    }
}
