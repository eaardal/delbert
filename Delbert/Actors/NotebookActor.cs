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
            Receive<GetNotebooks>(msg => OnGetNotebooks(msg));
            Receive<GotCurrentRootDirectoryResult>(msg => OnGotCurrentRootDirectoryResult(msg));
            Receive<SectionsHasBeenSetOnNotebooks>(msg => OnSectionsHasBeenSetOnNotebooks(msg));
        }

        private void OnSectionsHasBeenSetOnNotebooks(SectionsHasBeenSetOnNotebooks msg)
        {
            throw new NotImplementedException();
        }

        private void OnGotCurrentRootDirectoryResult(GotCurrentRootDirectoryResult message)
        {
            var rootDirectory = message.RootDirectory;

            var notebooks = GetNotebooksUnderDirectory(rootDirectory);

            var sections = SetNotebookSections(notebooks);

            Sender.Tell(new GetNotebooksResult(sections), Self);
        }

        #region Message Handlers

        private void OnGetNotebooks(GetNotebooks message)
        {
            try
            {
                GetCurrentRootDirectory();
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

        private void SetNotebookSections(ImmutableArray<NotebookDto> notebooks)
        {
            var sectionActor = Context.ActorOf(ActorRegistry.Section);

            notebooks.ForEach(notebook =>
            {
                _section.GetSectionsForNotebook(sectionActor, notebook).ContinueWith(sectionsTask =>
                {
                    var sections = sectionsTask.Result;

                    notebook.AddSections(sections);

                }).PipeTo(Self);
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

        private void GetCurrentRootDirectory()
        {
            var rootDirectoryActorSelection = Context.ActorSelection(ActorRegistry.RootDirectory);

            _rootDirectory.GetRootDirectory(rootDirectoryActorSelection).PipeTo(Self);
        }

        #region Messages

        internal class SectionsHasBeenSetOnNotebooks
        {
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

        internal class GotCurrentRootDirectoryResult
        {
            public GotCurrentRootDirectoryResult(DirectoryInfo rootDirectory)
            {
                RootDirectory = rootDirectory;
            }

            public DirectoryInfo RootDirectory { get; }
        }

        #endregion
    }
}
