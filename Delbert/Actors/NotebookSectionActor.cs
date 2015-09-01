using System;
using System.Collections.Immutable;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Actors
{
    public class NotebookSectionActor : LoggingReceiveActor
    {
        public NotebookSectionActor(ILogger log) : base(log)
        {
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