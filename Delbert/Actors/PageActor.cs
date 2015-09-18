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
            Receive<CreatePage>(msg => OnCreatePage(msg));
        }

        private void OnCreatePage(CreatePage msg)
        {
            var file = msg.PageFile;

            if (!file.Exists)
            {
                file.Create();
            }
        }

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
            pages.ForEach(page =>
            {
                page.ParentSection = section;
                page.ParentNotebook = section.ParentNotebook;
            });

            return pages;
        }

        private ImmutableArray<PageDto> CreatePagesFromFiles(ImmutableArray<FileInfo> files)
        {
            return files.Select(file => new PageDto
            {
                Name = GetPageName(file),
                File = file,
                Text = ReadFileContents(file)

            }).ToImmutableArray();
        }

        private string ReadFileContents(FileInfo file)
        {
            return file.ReadAllText(Encoding.UTF8);
        }

        private string GetPageName(FileInfo file)
        {
            return file.Name;
        }

        private ImmutableArray<FileInfo> GetFilesUnderDirectory(DirectoryInfo directory)
        {
            return directory.GetFiles().ToImmutableArray();
        }

        #region Messages

        public class CreatePage
        {
            public FileInfo PageFile { get; }

            public CreatePage(FileInfo pageFile)
            {
                PageFile = pageFile;
            }
        }

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
