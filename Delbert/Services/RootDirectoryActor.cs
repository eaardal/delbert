using System;
using System.IO;
using System.Threading.Tasks;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Messages;

namespace Delbert.Services
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

        private void OnSetRootDirectory(SetRootDirectory msg)
        {
            _rootDirectory = msg.RootDirectory;

            _messageBus.Publish(new NewRootDirectorySet(_rootDirectory));

            Become(Configured);
        }

        private void Configured()
        {
            Receive<GetRootDirectory>(msg => OnGetRootDirectory(msg));
        }

        private async Task<DirectoryInfo> OnGetRootDirectory(GetRootDirectory msg)
        {
            return await Task.FromResult(_rootDirectory);
        }

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

        #endregion  
    }
}
