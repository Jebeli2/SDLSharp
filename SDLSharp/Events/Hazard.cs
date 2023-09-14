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

        public void Update(float deltaTime)
        {

        }

        public void AddRenderables(List<IMapSprite> r, List<IMapSprite> rDead)
        {

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
