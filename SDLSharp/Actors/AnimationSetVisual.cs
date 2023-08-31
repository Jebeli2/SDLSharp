namespace SDLSharp.Actors
{
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AnimationSetVisual : IVisual
    {
        private readonly AnimationSet animationSet;
        private Animation? activeAnimation;

        private float posX;
        private float posY;
        private int direction;
        public AnimationSetVisual(AnimationSet animationSet)
        {
            this.animationSet = animationSet;
            activeAnimation = this.animationSet.GetAnimation("");
            direction = 7;
        }

        public float PosX => posX;

        public float PosY => posY;

        public int Direction => direction;

        public string Animation => activeAnimation?.Name ?? "";

        public IEnumerable<ISprite> CurrentSprites
        {
            get
            {
                ISprite? sprite = activeAnimation?.CurrentSprite;
                if (sprite != null) { return new ISprite[] { sprite }; }
                return Array.Empty<ISprite>();
            }
        }

        public bool HasAnimationFinished => activeAnimation?.IsFinished ?? true;
        public bool IsActiveFrame => activeAnimation?.IsActiveFrame ?? true;

        public void SetAnimation(string animation)
        {
            if (activeAnimation != null && animation == activeAnimation.Name) return;
            activeAnimation = animationSet?.GetAnimation(animation);
            if (activeAnimation != null) { activeAnimation.Direction = direction; }
        }

        public void SetDirection(int direction)
        {
            if (this.direction == direction) return;
            this.direction = direction;
            if (activeAnimation != null) { activeAnimation.Direction = this.direction; }
        }

        public void SetPosition(float x, float y)
        {
            if (posX == x && posY == y) return;
            posX = x;
            posY = y;
        }

        public bool Update(double totalTime, double elapsedTime)
        {
            return activeAnimation?.Update(totalTime, elapsedTime) ?? false;
        }
    }
}
