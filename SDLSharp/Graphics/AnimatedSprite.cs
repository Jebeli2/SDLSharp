namespace SDLSharp.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AnimatedSprite : ISprite
    {
        private readonly List<ISprite> sprites;
        private readonly List<double> frameDurations;
        private int currentFrame;
        private double currentDuration;
        private bool backwards;
        private int timesPlayed;

        private readonly AnimationType animationType;
        private IImage? image;
        private int x;
        private int y;
        private int width;
        private int height;
        private int offsetX;
        private int offsetY;

        public AnimatedSprite(AnimationType animationType = AnimationType.Looped)
        {
            sprites = new();
            frameDurations = new();
            this.animationType = animationType;
        }

        public AnimatedSprite(AnimatedSprite other)
        {
            sprites = new(other.sprites);
            frameDurations = new(other.frameDurations);
            animationType = other.animationType;
        }
        public AnimationType AnimationType => animationType;
        public IImage Image => image!;
        public int X => x;
        public int Y => y;
        public int Width => width;
        public int Height => height;
        public int OffsetX => offsetX;
        public int OffsetY => offsetY;
        public Rectangle SourceRect { get => new(x, y, width, height); }
        public ISprite? CurrentSprite
        {
            get
            {
                if (sprites.Count > 0) return sprites[currentFrame];
                return null;
            }
        }

        public bool IsFinished
        {
            get { return timesPlayed > 0; }
        }

        public int CurrentFrame => currentFrame;
        public int FrameCount => Math.Max(1, sprites.Count);

        public bool Update(double totalTime, double elapsedTime)
        {
            if (frameDurations.Count > 0)
            {
                currentDuration += elapsedTime;
                if (currentDuration >= frameDurations[currentFrame])
                {
                    
                    AdvanceFrame();
                    return true;
                }
            }
            return false;
        }

        private void AdvanceFrame()
        {
            switch (animationType)
            {
                case AnimationType.PlayOnce:
                    if (currentFrame < FrameCount - 1)
                    {
                        currentFrame++;
                    }
                    else
                    {
                        timesPlayed++;
                    }
                    break;
                case AnimationType.Looped:
                    currentFrame = (currentFrame + 1) % FrameCount;
                    if (currentFrame == 0)
                    {
                        timesPlayed++;
                    }
                    break;
                case AnimationType.BackForth:
                    if (backwards)
                    {
                        if (currentFrame > 0)
                        {
                            currentFrame--;
                        }
                        else
                        {
                            currentFrame = 0;
                            backwards = false;
                            timesPlayed++;
                        }
                    }
                    else
                    {
                        if (currentFrame < FrameCount - 1)
                        {
                            currentFrame++;
                        }
                        else
                        {
                            currentFrame = FrameCount - 1;
                            backwards = true;
                        }
                    }
                    break;
                default:
                    currentFrame = 0;
                    break;
            }
            currentDuration = 0;
            UpdateSpriteValues(CurrentSprite);
        }

        private void UpdateSpriteValues(ISprite? sprite)
        {
            if (sprite != null)
            {
                image = sprite.Image;
                x = sprite.X;
                y = sprite.Y;
                width = sprite.Width;
                height = sprite.Height;
                offsetX = sprite.OffsetX;
                offsetY = sprite.OffsetY;
            }
            else
            {
                x = 0;
                y = 0;
                width = 0;
                height = 0;
                offsetX = 0;
                offsetY = 0;
            }
        }

        public void AddFrame(IImage image, double duration, int clipX, int clipY, int clipW, int clipH, int offsetX, int offsetY)
        {
            this.image = image;
            x = clipX;
            y = clipY;
            width = clipW;
            height = clipH;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            sprites.Add(new Sprite(image, clipX, clipY, clipW, clipH, offsetX, offsetY));
            frameDurations.Add(duration);

        }
    }
}
