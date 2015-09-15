using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actor.Dsl;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Actors
{
    internal class NotebookActor : LoggingReceiveActor
    {
        private readonly ISectionFacade _section;
        private readonly IRootDirectoryFacade _rootDirectory;

        public NotebookActor(ISectionFacade section, IRootDirectoryFacade rootDirectory, ILogger logger) : base(logger)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            if (rootDirectory == null) throw new ArgumentNullException(nameof(rootDirectory));
            _section = section;
            _rootDirectory = rootDirectory;

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

                var notebooksWithSections = await SetNotebookSections(notebooks);

                Sender.Tell(new GetNotebooksResult(notebooksWithSections), Self);
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
            var sectionActor = Context.ActorOf(ActorRegistry.Section);

            return await Task.Run(() =>
            {
                notebooks.ForEach(async notebook =>
                {
                    var sections = await _section.GetSectionsForNotebook(sectionActor, notebook);

                    notebook.AddSections(sections);
                });
                
                return notebooks;
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
            var rootDirectoryActorSelection = Context.ActorSelection(ActorRegistry.RootDirectory);

            return await _rootDirectory.GetRootDirectory(rootDirectoryActorSelection);
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
