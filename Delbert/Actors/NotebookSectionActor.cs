using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Actors
{
    public class NotebookSectionActor : LoggingReceiveActor
    {
        public NotebookSectionActor(ILogger log) : base(log)
        {
            Receive<GetSectionsForNotebook>(msg => OnGetSectionsForNotebook(msg));
        }

        #region Message Handlers

        private void OnGetSectionsForNotebook(GetSectionsForNotebook msg)
        {
            try
            {
                var sections = GetSections(msg.Notebook);

                Sender.Tell(new GetSectionsForNotebookResult(sections), Self);
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }

        #endregion

        private ImmutableArray<SectionDto> GetSections(NotebookDto notebook)
        {
            var subDirectories = notebook.Directory.GetDirectories();

            var sections = CreateSectionsFromDirectories(subDirectories);

            sections.ForEach(s => s.Notebook = notebook);

            return sections;
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