using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Actors
{
    public class SectionActor : LoggingReceiveActor
    {
        private readonly IPageFacade _page;

        public SectionActor(IPageFacade page, ILogger log) : base(log)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            _page = page;

            Receive<GetSectionsForNotebook>(msg =>
            {
                var originalSender = Sender;
                OnGetSectionsForNotebook(msg, originalSender);
            });
            Receive<CreateNewSection>(msg => OnCreateNewSection(msg));
            Receive<Internal.SetPagesForSectionsResult>(msg => OnSetPagesForSectionsResult(msg));
        }

        private void OnSetPagesForSectionsResult(Internal.SetPagesForSectionsResult message)
        {
            var sections = message.Sections;
            var originalSender = message.OriginalSender;

            originalSender.Tell(new GetSectionsForNotebookResult(sections));
        }
        
        private void OnGetSectionsForNotebook(GetSectionsForNotebook message, IActorRef originalSender)
        {
            try
            {
                var notebook = message.Notebook;

                var subDirectories = notebook.Directory.GetDirectories();

                var sections = CreateSectionsFromDirectories(subDirectories);

                var sectionsWithNotebook = SetParentNotebook(notebook, sections);

                SetPagesForSections(sectionsWithNotebook, originalSender);
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }

        private void OnCreateNewSection(CreateNewSection msg)
        {
            var directory = msg.Directory;

            if (!directory.Exists)
            {
                directory.Create();
            }
        }
        
        private void SetPagesForSections(ImmutableArray<SectionDto> sections, IActorRef originalSender)
        {
            var pageActor = Context.ActorOf(ActorRegistry.Page);

            var getPagesForSectionTasks = new List<Task<SectionDto>>();

            sections.ForEach(section =>
            {
                var task = _page.GetPagesForSection(pageActor, section).ContinueWith(pagesTask =>
                {
                    var pages = pagesTask.Result;

                    section.AddPages(pages);

                    return section;
                });

                getPagesForSectionTasks.Add(task);
            });

            Task.WhenAll(getPagesForSectionTasks).ContinueWith(sectionsWithPagesTask =>
            {
                var sectionsWithPages = sectionsWithPagesTask.Result.ToImmutableArray();
                return new Internal.SetPagesForSectionsResult(sectionsWithPages, originalSender);

            }).PipeTo(Self);
        }

        private static ImmutableArray<SectionDto> SetParentNotebook(NotebookDto notebook, ImmutableArray<SectionDto> sections)
        {
            return sections.ForEach(s => s.ParentNotebook = notebook).ToImmutableArray();
        }

        private ImmutableArray<SectionDto> CreateSectionsFromDirectories(DirectoryInfo[] subDirectories)
        {
            return subDirectories.Select(dir => new SectionDto
            {
                Name = GetSectionName(dir),
                Directory = dir

            }).ToImmutableArray();
        }

        private string GetSectionName(DirectoryInfo directory)
        {
            return directory.Name;
        }

        #region Messages

        private class Internal
        {
            internal class SetPagesForSectionsResult
            {
                public SetPagesForSectionsResult(ImmutableArray<SectionDto> sections, IActorRef originalSender)
                {
                    Sections = sections;
                    OriginalSender = originalSender;
                }

                public ImmutableArray<SectionDto> Sections { get; }
                public IActorRef OriginalSender { get; }
            }
        }

        internal class GetSectionsForNotebook
        {
            public NotebookDto Notebook { get; }

            public GetSectionsForNotebook(NotebookDto notebook)
            {
                if (notebook == null) throw new ArgumentNullException(nameof(notebook));
                Notebook = notebook;
            }
        }

        internal class GetSectionsForNotebookResult
        {
            public ImmutableArray<SectionDto> Sections { get; }

            public GetSectionsForNotebookResult(ImmutableArray<SectionDto> sections)
            {
                Sections = sections;
            }
        }

        public class CreateNewSection
        {
            public DirectoryInfo Directory { get; }

            public CreateNewSection(DirectoryInfo directory)
            {
                Directory = directory;
            }
        }

        #endregion
    }
}