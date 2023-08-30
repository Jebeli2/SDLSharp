namespace SDLSharp.Maps
{
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ParallaxLayer
    {
        private IImage? image;
        private string mapLayer = "";
        private float speed;
        private float fixedSpeedX;
        private float fixedSpeedY;
        private float fixedOffsetX;
        private float fixedOffsetY;

        public IImage? Image
        {
            get { return image; }
            set { image = value; }
        }

        public string MapLayer
        {
            get { return mapLayer; }
            set { mapLayer = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float FixedSpeedX
        {
            get { return fixedSpeedX; }
            set { fixedSpeedX = value; }
        }

        public float FixedSpeedY
        {
            get { return fixedSpeedY; }
            set { fixedSpeedY = value; }
        }

        public float FixedOffsetX
        {
            get { return fixedOffsetX; }
            set { fixedOffsetX = value; }
        }

        public float FixedOffsetY
        {
            get { return fixedOffsetY; }
            set { fixedOffsetY = value; }
        }

        public void Update(double totalTime, double elapsedTime)
        {
            if (image != null)
            {
                int width = image.Width;
                int height = image.Height;

                fixedOffsetX += fixedSpeedX;
                fixedOffsetY += fixedSpeedY;

                if (fixedOffsetX > width) { fixedOffsetX -= width; }
                if (fixedOffsetX < -width) { fixedOffsetX += width; }
                if (fixedOffsetY > height) { fixedOffsetY -= height; }
                if (fixedOffsetY < -height) { fixedOffsetY += height; }
            }
        }
    }
}
