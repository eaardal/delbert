using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Encoder = System.Text.Encoder;

namespace Delbert.Components.ImageCarousel
{
    public class ImageCarouselViewModel : ScreenViewModel, IImageCarouselViewModel
    {
        private readonly IImageFacade _image;

        public ImageCarouselViewModel(IImageFacade image, IIoC ioc) : base(ioc)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            _image = image;

            Images = new ObservableCollection<Bitmap>();

            MessageBus.Subscribe<PageSelected>(async msg => await OnPageSelected(msg));
        }

        public ObservableCollection<Bitmap> Images { get; set; }

        private async Task OnPageSelected(PageSelected message)
        {
            var directory = message.Page.ParentSection.Directory.FullName;
            var images = await _image.GetImages(directory);

            await DoOnUiDispatcherAsync(() =>
            {
                Images.Clear();
                images.ForEach(i =>
                {
                    var path = i.Path;
                    var fileName = i.FileName;
                    var scaled = resizeImage(path, fileName, 0, 150, (int) i.Width, (int) i.Height);
                    Images.Add(scaled);
                });
            });
        }

        private Bitmap resizeImage(string path, string originalFilename,
                     int canvasWidth, int canvasHeight, 
                     int originalWidth, int originalHeight)
        {
            var image = Image.FromFile(path + originalFilename);

            var thumbnail = new Bitmap(canvasWidth, canvasHeight);
            var graphic = Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            /* ------------------ new code --------------- */

            // Figure out the ratio
            double ratioX = (double)canvasWidth / (double)originalWidth;
            double ratioY = (double)canvasHeight / (double)originalHeight;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero)
            int posX = Convert.ToInt32((canvasWidth - (originalWidth * ratio)) / 2);
            int posY = Convert.ToInt32((canvasHeight - (originalHeight * ratio)) / 2);

            graphic.Clear(Color.White); // white padding
            graphic.DrawImage(image, posX, posY, newWidth, newHeight);
            
            //var info = ImageCodecInfo.GetImageEncoders();
            //var encoderParameters = new EncoderParameters(1)
            //{
            //    Param = { [0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L) }
            //};

            return thumbnail;

            //thumbnail.Save(path + newWidth + "." + originalFilename, info[1],
            //                 encoderParameters);
        }
    }
}
