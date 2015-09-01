﻿using System;
using System.IO;
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

            SetRootDirectoryFromCommandLineArgumentsIfExists();

            Receive<SetRootDirectory>(msg => OnSetRootDirectory(msg));
        }

        private void SetRootDirectoryFromCommandLineArgumentsIfExists()
        {
            var args = Environment.GetCommandLineArgs();

            int matchIndex;
            if (IsRootDirectoryArgument(args, out matchIndex))
            {
                var rootDirectoryPathIndex = matchIndex + 1;

                var directory = args[rootDirectoryPathIndex].ToDirectoryInfo();

                if (directory.Exists)
                {
                    SetRootDir(directory);
                }
                else
                {
                    Log.Msg(this, l => l.Warning("Given directory does not exist"));
                }
            }
        }

        private bool IsRootDirectoryArgument(string[] args, out int rootDirectoryArgSwitchIndex)
        {
            if (args.Length < 2)
            {
                rootDirectoryArgSwitchIndex = -1;
                return false;
            }

            var validSwitches = new[] { "-rd", "-path" };

            var foundMatch = false;
            var matchIndex = -1;

            for (var i = 0; i < args.Length; i++)
            {
                try
                {
                    var arg = args[i];
                    var nextArg = args[i + 1];

                    if (!string.IsNullOrEmpty(arg) && !string.IsNullOrEmpty(nextArg))
                    {
                        var argSwitch = arg;
                        
                        if (validSwitches.ContainsAny(argSwitch))
                        {
                            foundMatch = true;
                            matchIndex = i;
                            break;
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // Ignore and continue
                }
            }

            rootDirectoryArgSwitchIndex = matchIndex;
            return foundMatch;
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
                Sender.Tell(new NoRootDirectorySet(), Self);
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

        public class NoRootDirectorySet
        {
            
        }

        #endregion
    }
}
