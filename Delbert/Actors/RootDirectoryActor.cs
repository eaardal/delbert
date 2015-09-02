using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
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

        protected override async void PreStart()
        {
            await SetRootDirectoryFromCommandLineArgumentsIfExists();

            base.PreStart();
        }

        private async Task SetRootDirectoryFromCommandLineArgumentsIfExists()
        {
            try
            {
                var cmdArgsActor = Context.ActorOf(ActorRegistry.CommandLineArgsParser);

                var query =
                    await cmdArgsActor.Query(new CommandLineArgsParserActor.GetRootDirectoryFromCommandLineArgs());

                query
                    .WhenResultIs<CommandLineArgsParserActor.GetRootDirectoryFromCommandLineArgsResult>(result =>
                    {
                        if (result.Success)
                        {
                            SetRootDir(result.Directory);
                        }
                    })
                    .WhenResultIs<Failure>(fail => Log.Msg(this, l => l.Error(fail.Exception)));

            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
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
            SetRootDir(msg.RootDirectory);
        }

        private void SetRootDir(DirectoryInfo directory)
        {
            _rootDirectory = directory;

            _messageBus.Publish(new RootDirectoryChanged(_rootDirectory));

            Become(Configured);
        }

        private void OnGetRootDirectory(GetRootDirectory msg)
        {
            Sender.Tell(new GetRootDirectoryResult(_rootDirectory), Self);
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
                CurrentRootDirectory = currentRootDirectory;
                Success = CurrentRootDirectory != null && CurrentRootDirectory.Exists;
            }

            public DirectoryInfo CurrentRootDirectory { get; }
            public bool Success { get; }
        }

        public class NoRootDirectorySet
        {
            
        }

        #endregion
    }
}
