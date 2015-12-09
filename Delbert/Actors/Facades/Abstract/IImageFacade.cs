using System.Collections.Immutable;
using System.Threading.Tasks;
using Delbert.Components.ImageCarousel;
using Delbert.Infrastructure;

namespace Delbert.Actors.Facades.Abstract
{
    public interface IImageFacade
    {
        Task<ImmutableArray<IBitmapImageAdapter>>  GetImages(string directory);
    }
}