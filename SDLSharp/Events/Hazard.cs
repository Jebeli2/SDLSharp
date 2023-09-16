namespace SDLSharp.Events
{
    using SDLSharp.Actors;
    using SDLSharp.Graphics;
    using SDLSharp.Maps;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    public class Hazard
    {
        private Hazard? parent;
        private List<Hazard> children;
        private List<Actor> entitiesCollided;
        private IVisual? visual;
        private readonly List<IMapSprite> sprites = new();
        private float angle;

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
        //public Animation? Animation { get; set; }
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
        public PointF Speed { get; set; }
        public IVisual? Visual
        {
            get => visual;
            set
            {
                if (visual != value)
                {
                    visual = value;
                    if (visual != null)
                    {
                        visual.SetPosition(Pos.X, Pos.Y);
                        visual.SetDirection(Direction);
                        visual.SetAnimation("");
                    }
                    InvalidateSprites();
                }
            }
        }

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
            if (visual != null)
            {
                if (visual.Update(totalTime, elapsedTime))
                {
                    InvalidateSprites();
                    PointF prevPos = Pos;
                    if (Speed.X != 0 || Speed.Y != 0)
                    {
                        Pos = new PointF(Pos.X + Speed.X, Pos.Y + Speed.Y);
                        //InvalidateSprites();
                    }
                    //changed = true;
                }
            }
        }

        public void SetAngle(float angle)
        {
            while (angle >= MathF.Tau) { angle -= MathF.Tau; }
            while (angle < 0.0f) { angle += MathF.Tau; }
            this.angle = angle;
            Speed = new PointF(BaseSpeed * MathF.Cos(this.angle), BaseSpeed * MathF.Sin(this.angle));
            if (Power != null && Power.Directional)
            {
                Direction = MathUtils.CalcDirection(Pos.X, Pos.Y, Pos.X + Speed.X, Pos.Y + Speed.Y);
            }
        }

        private void InvalidateSprites()
        {
            sprites.Clear();
        }

        private void UpdateSprites()
        {
            if (sprites.Count == 0)
            {
                sprites.AddRange(GetCurrentSprites());
            }
        }

        private List<IMapSprite> GetCurrentSprites()
        {
            List<IMapSprite> list = new();
            long prio = OnFloor ? 1 : 2;
            if (visual != null)
            {
                foreach (ISprite sprite in visual.CurrentSprites)
                {
                    MapSprite s = new MapSprite(sprite)
                    {
                        MapPosX = Pos.X,
                        MapPosY = Pos.Y,
                        BasePrio = prio
                    };
                    list.Add(s);
                    prio++;
                }
            }
            return list;
        }

        public IEnumerable<IMapSprite> GetSprites()
        {
            UpdateSprites();
            List<IMapSprite> list = new List<IMapSprite>(sprites);
            return list;
        }

        public void AddRenderables(List<IMapSprite> r, List<IMapSprite> rDead)
        {
            if (DelayFrames == 0)
            {
                if (OnFloor)
                {
                    rDead.AddRange(GetSprites());
                }
                else
                {
                    r.AddRange(GetSprites());
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
