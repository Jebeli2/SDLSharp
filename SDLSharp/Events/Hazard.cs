namespace SDLSharp.Events
{
    using SDLSharp.Actors;
    using SDLSharp.Graphics;
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Hazard
    {
        private Hazard? parent;
        private List<Hazard> children;
        private List<Actor> entitiesCollided;

        public Hazard()
        {
            MovementType = MovementType.Flying;
            Active = true;
            children = new List<Hazard>();
            entitiesCollided = new List<Actor>();
        }
        public PointF Pos { get; set; }
        public Power? Power { get; set; }
        public int BaseLifespan { get; set; }
        public float Lifespan { get; set; }
        public bool OnFloor { get; set; }
        public bool Directional { get; set; }
        public bool RemoveNow { get; set; }
        public bool HitWall { get; set; }
        public bool WallReflect { get; set; }
        public Animation? Animation { get; set; }
        public int Direction { get; set; }
        public MovementType MovementType { get; set; }
        public bool CompleteAnimation { get; set; }
        public SDLSound? SfxHit { get; set; }
        public bool SfxHitEnable { get; set; }
        public bool SfxHitPlayed { get; set; }
        public float BaseSpeed { get; set; }
        public bool Active { get; set; }
        public int DelayFrames { get; set; }
        public float Radius { get; set; }
        public Actor? Source { get; set; }
        public SourceType SourceType { get; set; }
        public bool MultiTarget { get; set; }
        public bool MultiHit { get; set; }
        public bool AimAssist { get; set; }
        public bool ExpireWithCaster { get; set; }
        public Power? WallPower { get; set; }
        public int WallPowerChance { get; set; }
        public Power? PostPower { get; set; }
        public int PostPowerChance { get; set; }
        public bool IgnoreZeroDamage { get; set; }
        public bool Beacon { get; set; }
        public bool NoAggro { get; set; }
        public int DmgMin { get; set; }
        public int DmgMax { get; set; }

        public void Update(double totalTime, double elapsedTime)
        {
            if (DelayFrames > 0)
            {
                DelayFrames--;
                return;
            }
            if (Lifespan > 0)
            {
                Lifespan -= (float)elapsedTime;
            }
            if (Animation != null)
            {
                Animation.Update(totalTime, elapsedTime);
            }

        }

        public void AddRenderables(List<IMapSprite> r, List<IMapSprite> rDead)
        {
            if (DelayFrames == 0 && Animation != null)
            {
                ISprite? sprite = Animation.CurrentSprite;
                if (sprite != null)
                {
                    MapSprite ms = new MapSprite(sprite);
                    ms.MapPosX = Pos.X;
                    ms.MapPosY = Pos.Y;
                    if (OnFloor)
                    {
                        ms.BasePrio = 0;
                        ms.Prio = 0;
                        rDead.Add(ms);
                    }
                    else
                    {
                        ms.BasePrio = 2;
                        ms.Prio = 2;
                        r.Add(ms);
                    }
                }
            }
        }
        public void AddChild(Hazard haz)
        {
            children.Add(haz);
            haz.parent = this;
        }

        public void AddEntity(Actor e)
        {
            if (parent != null) parent.AddEntity(e);
            else entitiesCollided.Add(e);
        }

        public bool HasEntity(Actor e)
        {
            if (MultiHit) return false;
            if (parent != null) return parent.HasEntity(e);
            else return entitiesCollided.Contains(e);
        }

    }
}
