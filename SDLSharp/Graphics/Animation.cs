namespace SDLSharp.Graphics
{
    using SDLSharp.Content;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Animation : Resource
    {
        private IList<AnimatedSprite> sprites = new List<AnimatedSprite>();
        private AnimationType animationType;
        private int direction;
        private readonly List<int> activeFrames = new();

        public Animation(string name, AnimationType animationType = AnimationType.Looped, int numDirections = 8)
         : base(name, ContentFlags.Data)
        {
            this.animationType = animationType;
            SetupDirections(numDirections, animationType);
        }

        internal Animation(Animation other)
            : base(other)
        {
            animationType = other.animationType;
            direction = other.direction;
            foreach (var sprite in other.sprites)
            {
                sprites.Add(new AnimatedSprite(sprite));
            }
        }

        internal IList<AnimatedSprite> AnimatedSprites => sprites;

        public void SetActiveFrames(IList<int> frames)
        {
            activeFrames.Clear();
            if (frames != null)
            {
                if (frames.Count == 1 && frames[0] == -1)
                {
                    for (int i = 0; i < FrameCount; i++)
                    {
                        activeFrames.Add(i);
                    }
                }
                else
                {
                    activeFrames.AddRange(frames);
                }
            }
        }

        public int Frame => sprites[direction].CurrentFrame;
        public int FrameCount => sprites[direction].FrameCount;
        public int Direction
        {
            get { return direction; }
            set
            {
                if (value != direction && value >= 0 && value < sprites.Count)
                {
                    direction = value;
                }
            }
        }

        public bool IsFinished
        {
            get
            {
                return sprites[direction].IsFinished;
            }
        }

        public bool IsActiveFrame
        {
            get
            {
                int cf = sprites[direction].CurrentFrame;
                if (activeFrames.Contains(cf)) return true;
                return sprites[direction].IsFinished;
            }
        }

        public ISprite? CurrentSprite
        {
            get { return sprites[direction].CurrentSprite; }
        }

        public bool Update(double totalTime, double elapsedTime)
        {
            return sprites[direction].Update(totalTime, elapsedTime);
        }

        private void SetupDirections(int numDirections, AnimationType animationType)
        {
            sprites = new AnimatedSprite[numDirections];
            for (int i = 0; i < numDirections; i++)
            {
                sprites[i] = new AnimatedSprite(animationType);
            }
        }
    }
}
