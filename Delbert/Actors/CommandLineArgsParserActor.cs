using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Actors
{
    class CommandLineArgsParserActor : LoggingReceiveActor
    {
        private readonly IEnvironmentAdapter _environment;

        public CommandLineArgsParserActor(ILogger log, IEnvironmentAdapter environment) : base(log)
        {
            if (environment == null) throw new ArgumentNullException(nameof(environment));
            _environment = environment;

            Receive<GetRootDirectoryFromCommandLineArgs>(msg => OnGetRootDirectoryFromCommandLineArgs(msg));
        }

        private void OnGetRootDirectoryFromCommandLineArgs(GetRootDirectoryFromCommandLineArgs message)
        {
            GetRootDirectoryArgument();
        }
        
        private void GetRootDirectoryArgument()
        {
            var args = _environment.GetCommandLineArgs();

            int argIndex;

            if (HasRootDirectoryArgument(args, out argIndex))
            {
                var rootDirectoryPathIndex = argIndex + 1;

                var directory = args[rootDirectoryPathIndex].ToDirectoryInfo();

                if (!directory.Exists)
                {
                    Log.Msg(this, l => l.Warning("Given directory does not exist"));
                }

                Sender.Tell(new GetRootDirectoryFromCommandLineArgsResult(directory), Self);
            }
        }

        private bool HasRootDirectoryArgument(string[] args, out int rootDirectoryArgIndex)
        {
            if (args.Length < 2)
            {
                rootDirectoryArgIndex = -1;
                return false;
            }

            var validArgs = new[] { "-path" };

            var foundMatch = false;
            var validArgIndex = -1;

            for (var i = 0; i < args.Length; i++)
            {
                try
                {
                    var arg = args[i];
                    var nextArg = args[i + 1];

                    if (string.IsNullOrEmpty(arg) || string.IsNullOrEmpty(nextArg)) continue;

                    if (validArgs.ContainsAny(arg))
                    {
                        foundMatch = true;
                        validArgIndex = i;
                        break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // Ignore and continue
                }
            }

            rootDirectoryArgIndex = validArgIndex;
            return foundMatch;
        }

        #region Messages

        internal class GetRootDirectoryFromCommandLineArgsResult
        {
            public DirectoryInfo Directory { get; }
            public bool Success { get; }

            public GetRootDirectoryFromCommandLineArgsResult(DirectoryInfo directory)
            {
                Directory = directory;
                Success = Directory != null && Directory.Exists;
            }
        }

        internal class GetRootDirectoryFromCommandLineArgs
        {
        }

        #endregion
    }
}
