using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Services
{
    internal class NotebookActor : LoggingReceiveActor
    {
        public NotebookActor(ILogger logger) : base(logger)
        {
            Receive<CreateNotebook>(msg => OnCreateNotebook(msg));
            Receive<GetNotebooks>(async msg => await OnGetNotebooks(msg));
        }

        #region Message Handlers

        private async Task OnGetNotebooks(GetNotebooks message)
        {
            try
            {
                var rootDirectory = await GetCurrentRootDirectory();

                var notebooks = GetNotebooksUnderDirectory(rootDirectory);

                Sender.Tell(new GetNotebooksResult(notebooks), Self);
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }

        private void OnCreateNotebook(CreateNotebook message)
        {
            var directory = message.Directory;

            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        #endregion

        private ImmutableArray<NotebookDto> GetNotebooksUnderDirectory(DirectoryInfo directory)
        {
            var subDirectories = directory.GetDirectories();

            return CreateNotebooksFromDirectories(subDirectories);
        }

        private ImmutableArray<NotebookDto> CreateNotebooksFromDirectories(DirectoryInfo[] directories)
        {
            return directories.Select(d => new NotebookDto
            {
                Name = GetNotebookName(d),
                Directory = d
            }).ToImmutableArray();
        }

        private string GetNotebookName(DirectoryInfo directoryInfo)
        {
            return directoryInfo.Name;
        }

        private async Task<DirectoryInfo> GetCurrentRootDirectory()
        {
            var rootDirectoryActor = Context.ActorSelection(ActorRegistry.RootDirectory);

            var result =
                await
                    rootDirectoryActor.AskWithResultOf<RootDirectoryActor.GetRootDirectoryResult>(
                        new RootDirectoryActor.GetRootDirectory());

            return result.CurrentRootDirectory;
        }

        #region Messages

        internal class CreateNotebook
        {
            public DirectoryInfo Directory { get; }

            public CreateNotebook(DirectoryInfo directory)
            {
                if (directory == null) throw new ArgumentNullException(nameof(directory));
                Directory = directory;
            }
        }

        public class GetNotebooks { }

        public class GetNotebooksResult
        {
            public ImmutableArray<NotebookDto> Notebooks { get; }

            public GetNotebooksResult(ImmutableArray<NotebookDto> notebooks)
            {
                if (notebooks == null) throw new ArgumentNullException(nameof(notebooks));
                Notebooks = notebooks;
            }
        }

        #endregion
    }
}
