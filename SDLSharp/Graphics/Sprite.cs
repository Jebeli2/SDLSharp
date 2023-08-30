namespace SDLSharp.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Sprite : ImageRegion, ISprite
    {
        private readonly int offsetX;
        private readonly int offsetY;
        public Sprite(IImage image)
         : this(image, 0, 0, image.Width, image.Height, 0, 0)
        {

        }
        public Sprite(IImage image, int x, int y, int width, int height, int offsetX = 0, int offsetY = 0)
            : base(image, x, y, width, height)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        public Sprite(ISprite other)
            : base(other)
        {
            offsetX = other.OffsetX;
            offsetY = other.OffsetY;
        }
        public int OffsetX { get => offsetX; }
        public int OffsetY { get => offsetY; }

        public virtual bool Update(double totalTime, double elapsedTime)
        {
            return false;
        }
    }
}
