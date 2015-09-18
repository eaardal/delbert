using System;
using System.Collections.Generic;
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
            Receive<GetNotebooks>(msg =>
            {
                var originalSender = Sender;
                OnGetNotebooks(msg, originalSender);
            });

            Receive<Internal.GotCurrentRootDirectoryResult>(msg => OnGotCurrentRootDirectoryResult(msg));
            Receive<Internal.SetNotebookSectionsResult>(msg => OnSectionsHasBeenSetOnNotebooks(msg));
        }

        private void OnSectionsHasBeenSetOnNotebooks(Internal.SetNotebookSectionsResult message)
        {
            var originalSender = message.OriginalSender;
            originalSender.Tell(new GetNotebooksResult(message.Notebooks), Self);
        }

        private void OnGotCurrentRootDirectoryResult(Internal.GotCurrentRootDirectoryResult message)
        {
            var rootDirectory = message.RootDirectory;

            var notebooks = GetNotebooksUnderDirectory(rootDirectory);

            SetNotebookSections(notebooks, message.OriginalSender);
        }
        
        private void OnGetNotebooks(GetNotebooks message, IActorRef originalSender)
        {
            try
            {
                var rootDirectoryActorSelection = Context.ActorSelection(ActorRegistry.RootDirectory);

                _rootDirectory.GetRootDirectory(rootDirectoryActorSelection).ContinueWith(rootDirectoryTask =>
                {
                    var rootDirectory = rootDirectoryTask.Result;

                    return new Internal.GotCurrentRootDirectoryResult(rootDirectory, originalSender);

                }).PipeTo(Self);
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
        
        private void SetNotebookSections(ImmutableArray<NotebookDto> notebooks, IActorRef originalSender)
        {
            var sectionActor = Context.ActorOf(ActorRegistry.Section);

            var getSectionsForNotebookTasks = new List<Task<NotebookDto>>();

            notebooks.ForEach(notebook =>
            {
                var task = _section.GetSectionsForNotebook(sectionActor, notebook).ContinueWith(sectionsTask =>
                {
                    var sections = sectionsTask.Result;

                    notebook.AddSections(sections);

                    return notebook;
                });

                getSectionsForNotebookTasks.Add(task);
            });

            Task.WhenAll(getSectionsForNotebookTasks).ContinueWith(notebooksWithSectionsTask =>
            {
                var notebooksWithSections = notebooksWithSectionsTask.Result.ToImmutableArray();
                return new Internal.SetNotebookSectionsResult(notebooksWithSections, originalSender);

            }).PipeTo(Self);
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
        
        #region Messages

        private class Internal
        {
            internal class GotCurrentRootDirectoryResult
            {
                public GotCurrentRootDirectoryResult(DirectoryInfo rootDirectory, IActorRef originalSender)
                {
                    RootDirectory = rootDirectory;
                    OriginalSender = originalSender;
                }

                public DirectoryInfo RootDirectory { get; }
                public IActorRef OriginalSender { get; }
            }


            internal class SetNotebookSectionsResult
            {
                public SetNotebookSectionsResult(ImmutableArray<NotebookDto> notebooks, IActorRef originalSender)
                {
                    Notebooks = notebooks;
                    OriginalSender = originalSender;
                }

                public ImmutableArray<NotebookDto> Notebooks { get; }
                public IActorRef OriginalSender { get; }
            }
        }

        public class CreateNotebook
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
