using System;
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

            Receive<GetSectionsForNotebook>(async msg => await OnGetSectionsForNotebook(msg));
        }

        #region Message Handlers

        private async Task OnGetSectionsForNotebook(GetSectionsForNotebook msg)
        {
            try
            {
                var sections = await GetSections(msg.Notebook);

                Sender.Tell(new GetSectionsForNotebookResult(sections), Self);
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }

        #endregion

        private async Task<ImmutableArray<SectionDto>> GetSections(NotebookDto notebook)
        {
            var subDirectories = notebook.Directory.GetDirectories();

            var sections = CreateSectionsFromDirectories(subDirectories);

            var sectionsWithNotebook = SetParentNotebook(notebook, sections);

            return await SetPagesForSections(sectionsWithNotebook);
        }

        private async Task<ImmutableArray<SectionDto>> SetPagesForSections(ImmutableArray<SectionDto> sections)
        {
            var pageActor = Context.ActorOf(ActorRegistry.Page);

            return await Task.Run(() =>
            {
                sections.ForEach(async section =>
                {
                    var pages = await _page.GetPagesForSection(pageActor, section);

                    section.AddPages(pages);
                });

                return sections;
            });
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

        #endregion
    }
}