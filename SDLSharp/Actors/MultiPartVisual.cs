namespace SDLSharp.Actors
{
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MultiPartVisual : IVisual
    {
        private readonly List<Animation> activeAnimations;
        private readonly IDictionary<int, IList<string>> layerOrder;
        private readonly IDictionary<string, AnimationSet> animationSets;

        private float posX;
        private float posY;
        private int direction;
        private float speed;

        public MultiPartVisual(IDictionary<string, AnimationSet> animationSets, IDictionary<int, IList<string>> layerOrder)
        {
            activeAnimations = new List<Animation>();
            this.animationSets = animationSets;
            this.layerOrder = layerOrder;
            speed = 0.1f;
            direction = 7;

        }
        public float Speed => speed;

        public float PosX => posX;

        public float PosY => posY;

        public int Direction => direction;

        public string Animation => activeAnimations.FirstOrDefault()?.Name ?? "";

        public IEnumerable<ISprite> CurrentSprites
        {
            get
            {
                foreach (ISprite? sprit in activeAnimations.Select(x => x.CurrentSprite))
                {
                    if (sprit != null) yield return sprit;
                }
            }
        }
        public int Frame => activeAnimations.FirstOrDefault()?.Frame ?? 0;
        public int FrameCount => activeAnimations.FirstOrDefault()?.FrameCount ?? 0;

        public bool HasAnimationFinished => activeAnimations.FirstOrDefault()?.IsFinished ?? true;
        public bool IsActiveFrame => activeAnimations.FirstOrDefault()?.IsActiveFrame ?? true;

        public void SetAnimation(string animation)
        {
            if (string.Equals(animation, Animation)) return;
            ChangeAnimation(direction, animation);
        }

        public void SetDirection(int direction)
        {
            if (this.direction == direction) return;
            this.direction = direction;
            ChangeAnimation(this.direction, Animation);
        }

        public void SetPosition(float x, float y)
        {
            if (posX == x && posY == y) return;
            posX = x;
            posY = y;
        }

        public bool Update(double totalTime, double elapsedTime)
        {
            bool changed = false;
            foreach (var a in activeAnimations) { if (a.Update(totalTime, elapsedTime)) { changed = true; }; }
            return changed;
        }

        private void ChangeAnimation(int dir, string name)
        {
            activeAnimations.Clear();
            int count = 0;
            if (layerOrder != null && layerOrder.TryGetValue(dir, out var order))
            {
                var sorted = animationSets.OrderBy(it => order.IndexOf(it.Key));
                foreach (var a in sorted)
                {
                    var anim = a.Value.GetAnimation(name);
                    if (anim != null)
                    {
                        anim.Direction = dir;
                        activeAnimations.Add(anim);
                        count++;
                    }
                }
            }
            else
            {
                foreach (var a in animationSets)
                {
                    var anim = a.Value.GetAnimation(name);
                    if (anim != null)
                    {
                        anim.Direction = dir;
                        activeAnimations.Add(anim);
                        count++;
                    }
                }
            }
            if (count == 0)
            {
                SDLLog.Error(LogCategory.APPLICATION, $"Found no animations {name}.");
            }
        }
    }
}
