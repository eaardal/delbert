using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Delbert.Infrastructure
{
    internal class BitmapImageAdapter : IBitmapImageAdapter
    {
        public string Path { get; }
        public double Height { get; }
        public double Width { get; }
        public string FileName { get; }

        public BitmapImageAdapter(Image image, string path, string fileName)
        {
            Path = path;
            Height = image.Height;
            Width = image.Width;
            FileName = fileName;
        }
    }
}