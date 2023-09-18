namespace SDLSharp.Events
{
    using SDLSharp.Actors;
    using SDLSharp.Content;
    using SDLSharp.Graphics;
    using SDLSharp.Maps;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class Power : Resource
    {
        private AnimationSet? animationSet;
        //public string Name { get; set; }
        public bool IsEmpty { get; set; }
        public string Description { get; set; }
        public PowerType Type { get; set; }
        public int Id { get; set; }
        public string AttackAnim { get; set; }
        public string AnimationName { get; set; }
        public PowerState NewState { get; set; }
        public StartingPos StartingPos { get; set; }
        public bool Facing { get; set; }
        public bool Directional { get; set; }
        public int VisualRandom { get; set; }
        public int VisualOption { get; set; }
        public bool CompleteAnimation { get; set; }
        public bool LockTargetToDirection { get; set; }
        public bool UseHazard { get; set; }
        public bool NoAttack { get; set; }
        public int Cooldown { get; set; }
        public int Lifespan { get; set; }
        public bool OnFloor { get; set; }
        public float Speed { get; set; }
        public int Count { get; set; }
        public int MissileAngle { get; set; }
        public int AngleVariance { get; set; }
        public int SpeedVariance { get; set; }
        public SDLSound? SfxSound { get; set; }
        public SDLSound? SfxHit { get; set; }
        public bool SfxHitEnable { get; set; }
        public MovementType MovementType { get; set; }
        public bool WallReflect { get; set; }
        public bool ExpireWithCaster { get; set; }
        public float Radius { get; set; }
        public bool MultiTarget { get; set; }
        public bool MultiHit { get; set; }
        public bool AimAssist { get; set; }
        public float TargetRange { get; set; }
        public int Delay { get; set; }
        public int WallPower { get; set; }
        public int WallPowerChance { get; set; }
        public int PostPower { get; set; }
        public int PostPowerChance { get; set; }
        public int PrePower { get; set; }
        public int PrePowerChance { get; set; }
        public List<PostEffect> PostEffects { get; set; }
        public bool IgnoreZeroDamage { get; set; }
        public string SpawnType { get; set; }
        public int TargetNeighbor { get; set; }
        public bool Beacon { get; set; }
        public bool NoAggro { get; set; }
        public bool Passive { get; set; }
        public bool IsBuff { get; set; }
        public SourceType SourceType { get; set; }

        public Power() : this("")
        {

        }
        public Power(string name)
            : base(name, ContentFlags.Data)
        {
            IsEmpty = true;
            SpawnType = "";
            Description = "";
            AttackAnim = "";
            AnimationName = "";
            NewState = PowerState.Invalid;
            Type = PowerType.Invalid;
            Count = 1;
            MovementType = MovementType.Flying;
            WallPowerChance = 100;
            PostPowerChance = 100;
            PrePowerChance = 100;
            PostEffects = new List<PostEffect>();
        }

        public void CleanUpName()
        {
            if (string.IsNullOrEmpty(Name))
            {
                if (!string.IsNullOrEmpty(AnimationName))
                {
                    Name = FileNameToName(AnimationName);
                }
                else if (!string.IsNullOrEmpty(SpawnType))
                {
                    Name = FileNameToName(SpawnType);
                }
            }

        }

        public override string ToString()
        {
            if (IsEmpty) return "{none}";
            StringBuilder sb = new StringBuilder();
            sb.Append("#");
            sb.Append(Id);
            sb.Append(":");
            sb.Append(Type);
            if (!string.IsNullOrEmpty(Name)) { sb.Append(":"); sb.Append(Name); }
            return sb.ToString();
        }

        internal static string FileNameToName(string fn)
        {
            StringBuilder sb = new StringBuilder();
            string n = Path.GetFileNameWithoutExtension(fn);
            var parts = n.Split('_');
            foreach (var s in parts)
            {
                sb.Append(Capitalize(s));
                sb.Append(" ");
            }
            if (sb.Length > 1) sb.Length -= 1;
            return sb.ToString();
        }

        internal static string Capitalize(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (i == 0)
                {
                    sb.Append(char.ToUpperInvariant(s[i]));
                }
                else
                {
                    sb.Append(char.ToLowerInvariant(s[i]));
                }
            }
            return sb.ToString();
        }
        internal void AdjustSource(Actor source, PointF target)
        {
            if (Facing)
            {
                source.SetDirection(MathUtils.CalcDirection(source.PosX, source.PosY, target.X, target.Y));
            }
            if (NewState == PowerState.Attack)
            {
                source.SetAnimation(AttackAnim);
            }
            else if (NewState == PowerState.Instant)
            {

            }
            else if (NewState == PowerState.Invalid)
            {
                if (Type == PowerType.Block)
                {
                    source.SetAnimation("block");
                }
            }
        }

        internal AnimationSet? GetAnimationSet(IContentManager? contentManager)
        {
            if (contentManager != null && animationSet == null && !string.IsNullOrEmpty(AnimationName))
            {
                animationSet = contentManager.Load<AnimationSet>(AnimationName);
            }
            return animationSet;
        }
    }
}
