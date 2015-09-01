using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Actors
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

                notebooks = await SetNotebookSections(notebooks);

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

        private async Task<ImmutableArray<NotebookDto>> SetNotebookSections(ImmutableArray<NotebookDto> notebooks)
        {
            var notebookSectionActor = Context.ActorOf(ActorRegistry.Section);

            return await Task.Run(() =>
            {
                return notebooks.ForEach(async notebook =>
                {
                    var result =
                        await
                            notebookSectionActor.AskWithResultOf<SectionActor.GetSectionsForNotebookResult>(
                                new SectionActor.GetSectionsForNotebook(notebook));

                    notebook.Sections = result.Sections;

                }).ToImmutableArray();
            });
        }

        private ImmutableArray<NotebookDto> GetNotebooksUnderDirectory(DirectoryInfo directory)
        {
            var subDirectories = directory.GetDirectories();

            return CreateNotebooksFromDirectories(subDirectories);
        }

        private ImmutableArray<NotebookDto> CreateNotebooksFromDirectories(DirectoryInfo[] directories)
        {
            return directories.Select(dir => new NotebookDto
            {
                Name = GetNotebookName(dir),
                Directory = dir

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
