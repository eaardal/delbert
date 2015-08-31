using System;
using System.IO;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Services
{
    internal class NotebookActor : LoggingReceiveActor
    {
        public NotebookActor(ILogger logger) : base(logger)
        {
            Receive<CreateNotebook>(msg => OnCreateNotebook(msg));
        }

        private void OnCreateNotebook(CreateNotebook message)
        {
            var directory = message.Directory;

            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        internal class CreateNotebook
        {
            public DirectoryInfo Directory { get; }

            public CreateNotebook(DirectoryInfo directory)
            {
                if (directory == null) throw new ArgumentNullException(nameof(directory));
                Directory = directory;
            }
        }
    }

    
}
