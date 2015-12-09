using System.Collections.Immutable;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Actors.Facades.Abstract;
using Delbert.Components.ImageCarousel;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Actors.Facades
{
    public class ImageFacade : ActorFacade, IImageFacade
    {
        public ImageFacade(IActorSystemAdapter actorSystem, ILogger logger) : base(actorSystem, logger)
        { }

        public async Task<ImmutableArray<IBitmapImageAdapter>> GetImages(string directory)
        {
            var actor = ActorSystem.ActorOf(ActorRegistry.Image);

            var answer = await actor.Ask(new ImageActor.GetImagesInDirectory(directory));

            if (answer is ImageActor.GetImagesInDirectoryResult)
            {
                var result = answer as ImageActor.GetImagesInDirectoryResult;

                return result.Images;
            }

            LogFailure(answer);

            return ImmutableArray<IBitmapImageAdapter>.Empty;
        }
    }
}
