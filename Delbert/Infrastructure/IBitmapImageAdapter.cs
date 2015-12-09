using System;

namespace Delbert.Infrastructure
{
    public interface IBitmapImageAdapter
    {
        string Path { get; }
        double Height { get; }
        double Width { get; }
        string FileName { get; }
    }
}