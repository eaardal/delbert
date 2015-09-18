using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Actors.Facades;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Messages;

namespace Delbert.Actors
{
    class RootDirectoryActor : LoggingReceiveActor
    {
        private readonly IMessageBus _messageBus;
        private readonly ICommandLineArgsParserFacade _commandLineArgsParser;
        private DirectoryInfo _rootDirectory;

        public RootDirectoryActor(ICommandLineArgsParserFacade commandLineArgsParser, ILogger log, IMessageBus messageBus)
            : base(log)
        {
            if (messageBus == null) throw new ArgumentNullException(nameof(messageBus));
            if (commandLineArgsParser == null) throw new ArgumentNullException(nameof(commandLineArgsParser));

            _messageBus = messageBus;
            _commandLineArgsParser = commandLineArgsParser;

            Receive<SetRootDirectory>(msg => OnSetRootDirectory(msg));
            Receive<SetRootDirectoryFromCommandLineArgumentsIfExists>(msg => OnSetRootDirectoryFromCommandLineArgumentsIfExists(msg));
            Receive<Internal.GetRootDirectoryFromCommandLineResult>(msg => OnGetRootDirectoryFromCommandLineResult(msg));
        }

        private void OnGetRootDirectoryFromCommandLineResult(Internal.GetRootDirectoryFromCommandLineResult message)
        {
            var directory = message.RootDirectory;

            if (directory != null)
            {
                SetRootDir(directory);
            }
        }
        
        private void OnSetRootDirectoryFromCommandLineArgumentsIfExists(SetRootDirectoryFromCommandLineArgumentsIfExists message)
        {
            try
            {
                var commandLineArgsParserActor = Context.ActorOf(ActorRegistry.CommandLineArgsParser);

                _commandLineArgsParser.GetRootDirectoryFromCommandLine(commandLineArgsParserActor)
                    .ContinueWith(rootDirectoryTask =>
                    {
                        var directory = rootDirectoryTask.Result;
                        return new Internal.GetRootDirectoryFromCommandLineResult(directory);

                    }).PipeTo(Self);
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

        private void OnSetRootDirectory(SetRootDirectory msg)
        {
            SetRootDir(msg.RootDirectory);
        }

        private void OnGetRootDirectory(GetRootDirectory msg)
        {
            Sender.Tell(new GetRootDirectoryResult(_rootDirectory), Self);
        }

        private void SetRootDir(DirectoryInfo directory)
        {
            _rootDirectory = directory;

            Become(Configured);

            _messageBus.Publish(new RootDirectoryChanged(_rootDirectory));
        }

        #region Messages

        private class Internal
        {
            internal class GetRootDirectoryFromCommandLineResult
            {
                public GetRootDirectoryFromCommandLineResult(DirectoryInfo rootDirectory)
                {
                    RootDirectory = rootDirectory;
                }

                public DirectoryInfo RootDirectory { get; }
            }
        }

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

        internal class SetRootDirectoryFromCommandLineArgumentsIfExists
        {
        }

        #endregion
    }
}
