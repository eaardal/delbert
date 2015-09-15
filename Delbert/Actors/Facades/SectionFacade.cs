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
    public class SectionFacade : ActorFacade, ISectionFacade
    {
        public SectionFacade(IActorSystemAdapter actorSystem, ILogger logger) : base(actorSystem, logger)
        {
            
        }

        public void CreateSection(DirectoryInfo directory)
        {
            var actor = ActorSystem.ActorOf(ActorRegistry.Section);

            actor.Tell(new SectionActor.CreateNewSection(directory));
        }

        public async Task<ImmutableArray<SectionDto>> GetSectionsForNotebook(NotebookDto notebook)
        {
            var actor = ActorSystem.ActorOf(ActorRegistry.Section);

            return await GetSectionsForNotebook(actor, notebook);
        }
        
        public async Task<ImmutableArray<SectionDto>> GetSectionsForNotebook(IActorRef actor, NotebookDto notebook)
        {
            var answer = await actor.Ask(new SectionActor.GetSectionsForNotebook(notebook));

            if (answer is SectionActor.GetSectionsForNotebookResult)
            {
                var result = answer as SectionActor.GetSectionsForNotebookResult;

                return result.Sections;
            }

            LogFailure(answer);

            return ImmutableArray<SectionDto>.Empty;
        }
    }
}