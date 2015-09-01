using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Actors
{
    public class PageActor : LoggingReceiveActor
    {
        public PageActor(ILogger log) : base(log)
        {
            Receive<GetPagesForSection>(msg => OnGetPagesForSection(msg));
        }

        #region Message Handlers

        private void OnGetPagesForSection(GetPagesForSection msg)
        {
            try
            {
                var pages = GetPages(msg.Section);

                Sender.Tell(new GetPagesForSectionResult(pages), Self);
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }

        private ImmutableArray<PageDto> GetPages(SectionDto section)
        {
            var files = GetFilesUnderDirectory(section.Directory);

            var pages = CreatePagesFromFiles(files);

            return SetParentSectionAndNotebook(pages, section);
        }

        private ImmutableArray<PageDto> SetParentSectionAndNotebook(ImmutableArray<PageDto> pages, SectionDto section)
        {
            return pages.ForEach(page =>
            {
                page.Section = section;
                page.Notebook = section.Notebook;

            }).ToImmutableArray();
        }

        private ImmutableArray<PageDto> CreatePagesFromFiles(ImmutableArray<FileInfo> files)
        {
            return files.Select(file => new PageDto
            {
                Name = GetPageName(file),
                File = file

            }).ToImmutableArray();
        }

        private string GetPageName(FileInfo file)
        {
            return file.Name;
        }

        private ImmutableArray<FileInfo> GetFilesUnderDirectory(DirectoryInfo directory)
        {
            return directory.GetFiles().ToImmutableArray();
        }

        #endregion

        #region Messages

        internal class GetPagesForSection
        {
            public GetPagesForSection(SectionDto section)
            {
                Section = section;
            }

            public SectionDto Section { get; }
        }

        internal class GetPagesForSectionResult
        {
            public GetPagesForSectionResult(ImmutableArray<PageDto> pages)
            {
                Pages = pages;
            }

            public ImmutableArray<PageDto> Pages { get; }
        }

        #endregion
    }
}
