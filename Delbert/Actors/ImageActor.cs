using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Actors
{
    public class ImageActor : LoggingReceiveActor
    {
        private readonly string[] validImageExtensions = {"jpg", "jpeg", "bmp", "png"};

        public ImageActor(ILogger log) : base(log)
        {
            Receive<GetImagesInDirectory>(msg => OnGetImagesInDirectory(msg));
        }

        private void OnGetImagesInDirectory(GetImagesInDirectory msg)
        {
            var images = GetImages(msg.Directory);

            Sender.Tell(new GetImagesInDirectoryResult(images), Self);
        }

        private ImmutableArray<IBitmapImageAdapter> GetImages(string directory)
        {
            var files = GetImageFiles(directory);

            return CreateImageAdapters(files);
        }

        private IEnumerable<BitmapImage> GetImageFiles(string directory)
        {
            var files = Directory.GetFiles(directory).Where(fileName => fileName.EndsWithAny(validImageExtensions));

            return CreateBitmapImages(files);
        }

        private IEnumerable<BitmapImage> CreateBitmapImages(IEnumerable<string> files)
        {
            return files
                .Select(file => new BitmapImage(new Uri(file)))
                .ToArray();
        }

        private ImmutableArray<IBitmapImageAdapter> CreateImageAdapters(IEnumerable<Image> files, string path, string fileName)
        {
            return files
                .Select(file => new BitmapImageAdapter(file, path, fileName))
                .Cast<IBitmapImageAdapter>()
                .ToImmutableArray();
        }

        public class GetImagesInDirectory
        {
            public string Directory { get; }

            public GetImagesInDirectory(string directory)
            {
                if (directory == null) throw new ArgumentNullException(nameof(directory));
                Directory = directory;
            }
        }

        public class GetImagesInDirectoryResult
        {
            public ImmutableArray<IBitmapImageAdapter> Images { get; }

            public GetImagesInDirectoryResult(ImmutableArray<IBitmapImageAdapter> images)
            {
                Images = images;
            }
        }
    }
}