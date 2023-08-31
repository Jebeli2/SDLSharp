namespace SDLSharp.Actors
{
    using SDLSharp.Content;
    using SDLSharp.Graphics;
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Actor : Resource
    {
        private float posX;
        private float posY;
        private int direction;
        private string animation;
        private bool alive;
        private MovementType movementType;
        private IVisual? visual;
        private readonly List<IMapSprite> sprites = new();

        public Actor(string name)
            : base(name, ContentFlags.Data)
        {
            direction = 7;
            movementType = MovementType.Normal;
            animation = "";
            alive = true;
            DisplayName = name;
            DefaultSpeed = 2.0f;
            Cooldown = 333;
        }

        public float PosX => posX;
        public float PosY => posY;
        public bool Alive => alive;
        public bool Collided { get; set; }
        public bool Dead { get; set; }
        public bool Dying { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsNPC { get; set; }
        public bool IsEnemy { get; set; }
        public bool Flying { get; set; }
        public bool Intangible { get; set; }
        public int TurnDelay { get; set; }
        public int WaypointPause { get; set; }

        public MovementType MovementType => movementType;
        public CollisionType CollisionType
        {
            get { return IsPlayer ? CollisionType.Player : CollisionType.Normal; }
        }
        public bool HasMoved { get; set; }
        public string DisplayName { get; set; }
        public int Cooldown { get; set; }
        public float Speed { get; set; }
        public float DefaultSpeed { get; set; }

        public int Direction => direction;
        public bool IsMoving => "run" == animation;
        public bool HasAnimationFinished => visual?.HasAnimationFinished ?? true;
        public bool IsActiveFrame => visual?.IsActiveFrame ?? true;

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
                        visual.SetPosition(posX, posY);
                        visual.SetDirection(direction);
                        visual.SetAnimation(animation);
                    }
                    InvalidateSprites();
                }
            }
        }
        public IEnumerable<IMapSprite> GetSprites()
        {
            UpdateSprites();
            List<IMapSprite> list = new List<IMapSprite>(sprites);
            return list;
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
            long prio = 1;
            if (visual != null)
            {
                foreach (ISprite sprite in visual.CurrentSprites)
                {
                    MapSprite s = new MapSprite(sprite)
                    {
                        MapPosX = posX,
                        MapPosY = posY,
                        BasePrio = prio
                    };
                    list.Add(s);
                    prio++;
                }
            }
            return list;
        }
        public void Update(double totalTime, double elapsedTime)
        {
            Speed = DefaultSpeed / 60;
            bool changed = false;
            if (visual != null)
            {
                if (visual.Update(totalTime, elapsedTime))
                {
                    InvalidateSprites();
                    changed = true;
                }
            }
            if (Dying && HasAnimationFinished && !Dead)
            {
                Dead = true;
            }
            else if (!changed && ShouldRevertToStance())
            {
                SetAnimation("stance");
                changed = true;
            }
        }
        private bool ShouldRevertToStance()
        {
            if (Dead) return false;
            if (visual == null) return false;
            switch (visual.Animation)
            {
                case "stance":
                case "run":
                case "die":
                case "critdie":
                case "dead":
                    return false;
                case "hit":
                    if (IsPlayer)
                    {
                        return HasAnimationFinished;
                    }
                    break;
            }
            return HasAnimationFinished;
        }
        public void SetPosition(float x, float y)
        {
            posX = x;
            posY = y;
            visual?.SetPosition(x, y);
            InvalidateSprites();
        }

        public void SetDirection(int direction)
        {
            this.direction = direction;
            visual?.SetDirection(direction);
            InvalidateSprites();
        }

        public void SetAnimation(string animation)
        {
            this.animation = animation;
            visual?.SetAnimation(animation);
            InvalidateSprites();
        }

        public bool IsAdjacentTo(Actor? other)
        {
            if (other != null)
            {
                int myX = (int)MathF.Floor(PosX);
                int myY = (int)MathF.Floor(PosY);
                int eX = (int)MathF.Floor(other.PosX);
                int eY = (int)MathF.Floor(other.PosY);
                int diffX = Math.Abs(myX - eX);
                int diffY = Math.Abs(myY - eY);
                return diffX <= 1 && diffY <= 1 && ((diffX + diffY) > 0);
            }
            return false;
        }

    }
}
