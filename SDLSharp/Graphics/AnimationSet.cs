namespace SDLSharp.Graphics
{
    using SDLSharp.Content;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AnimationSet : Resource
    {
        private readonly List<Animation> animations = new();
        private Animation? defaultAnimation;
        public AnimationSet(string name)
            : base(name, ContentFlags.Data)
        {
        }
        public void AddAnimation(Animation a)
        {
            animations.Add(a);
            if (animations.Count == 1)
            {
                defaultAnimation = a;
            }
        }

        public Animation? GetAnimation(string name)
        {
            foreach (Animation animation in animations)
            {
                if (name == animation.Name)
                {
                    return new Animation(animation);
                }
            }
            if (defaultAnimation != null)
            {
                return new Animation(defaultAnimation);
            }
            return null;
        }
    }
}
