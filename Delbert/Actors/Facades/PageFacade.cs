using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;
using Delbert.Model;

namespace Delbert.Actors.Facades
{
    public class PageFacade : ActorFacade, IPageFacade
    {
        public PageFacade(IActorSystemAdapter actorSystem, ILogger logger) : base(actorSystem, logger)
        {
     
        }

        public async Task<ImmutableArray<PageDto>> GetPagesForSection(SectionDto section)
        {
            var actor = ActorSystem.ActorOf(ActorRegistry.Page);

            return await GetPagesForSection(actor, section);
        }
        
        public async Task<ImmutableArray<PageDto>> GetPagesForSection(IActorRef actor, SectionDto section)
        {
            var answer = await actor.Ask(new PageActor.GetPagesForSection(section));

            if (answer is PageActor.GetPagesForSectionResult)
            {
                var result = answer as PageActor.GetPagesForSectionResult;

                return result.Pages;
            }

            LogFailure(answer);

            return ImmutableArray<PageDto>.Empty;
        }

        public void CreatePage(FileInfo pageFile)
        {
            var pageActor = ActorSystem.ActorOf(ActorRegistry.Page);

            CreatePage(pageActor, pageFile);
        }

        public void CreatePage(IActorRef actor, FileInfo pageFile)
        {
            actor.Tell(new PageActor.CreatePage(pageFile));
        }
    }
}