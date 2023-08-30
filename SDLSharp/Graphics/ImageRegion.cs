namespace SDLSharp.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImageRegion : IImageRegion
    {
        private IImage image;
        private int x;
        private int y;
        private int width;
        private int height;

        public ImageRegion(IImage image)
            : this(image, 0, 0, image.Width, image.Height)
        {

        }
        public ImageRegion(IImage image, int x, int y, int width, int height)
        {
            this.image = image;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public ImageRegion(IImageRegion other)
        {
            image = other.Image;
            x = other.X;
            y = other.Y;
            width = other.Width;
            height = other.Height;
        }
        public IImage Image { get => image; }
        public int X { get => x; }
        public int Y { get => y; }
        public int Width { get => width; }
        public int Height { get => height; }
        public Rectangle SourceRect { get => new(x, y, width, height); }
    }
}
