namespace SDLSharp.Events
{
    using SDLSharp.Actors;
    using SDLSharp.Content.Flare;
    using SDLSharp.Graphics;
    using SDLSharp.Maps;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    public class PowerManager : IPowerManager
    {
        private readonly IMapEngine engine;
        private readonly List<Power> powers = new();
        private readonly List<Hazard> hazards = new();
        private Hazard? lastHazard;

        public PowerManager(IMapEngine engine)
        {
            this.engine = engine;
        }

        public bool Initialized => powers.Count > 0;

        public IList<Power> Powers
        {
            get { return powers; }
            set
            {
                powers.Clear();
                if (value != null)
                {
                    powers.AddRange(value);
                    foreach (var p in powers)
                    {
                        p.CleanUpName();
                    }
                }
            }
        }

        public Power? GetPower(int index)
        {
            if (index >= 0 && index < powers.Count)
            {
                return powers[index];
            }
            return null;
        }
        public void Clear()
        {
            hazards.Clear();
            lastHazard = null;
        }
        public bool Activate(Power power, Actor source, PointF target)
        {
            PointF newTarget = target;
            if (power.LockTargetToDirection)
            {
                float dist = MathUtils.Distance(source.PosX, source.PosY, target.X, target.Y);
                int dir = MathUtils.CalcDirection(source.PosX, source.PosY, target.X, target.Y);
                newTarget = MathUtils.CalcVector(source.PosX, source.PosY, dir, dist);
            }
            switch (power.Type)
            {
                case PowerType.Fixed:
                    return Fixed(power, source, newTarget);
                case PowerType.Missile:
                    return Missile(power, source, newTarget);
                case PowerType.Repeater:
                    return Repeater(power, source, newTarget);
                case PowerType.Spawn:
                    return Spawn(power, source, newTarget);
                case PowerType.Transform:
                    return Transform(power, source, newTarget);
                case PowerType.Block:
                    return Block(power, source, newTarget);
            }
            return false;
        }

        private bool Fixed(Power power, Actor source, PointF target)
        {
            power.AdjustSource(source, target);
            if (power.UseHazard)
            {
                int delayIterator = 0;
                for (int i = 0; i < power.Count; i++)
                {
                    Hazard haz = InitHazard(power, source, target);
                    haz.DelayFrames = delayIterator;
                    delayIterator += power.Delay;
                }
            }
            Buff(power, source, target);
            PlaySound(power);
            return true;
        }
        private bool Missile(Power power, Actor source, PointF target)
        {
            power.AdjustSource(source, target);
            PointF src;
            if (power.StartingPos == StartingPos.Target)
            {
                src = target;
            }
            else
            {
                src = new PointF(source.PosX, source.PosY);
            }
            float theta = MathUtils.CalcTheta(src.X, src.Y, target.X, target.Y);
            int delayIterator = 0;
            for (int i = 0; i < power.Count; i++)
            {
                Hazard haz = InitHazard(power, source, target);
                float offsetAngle = ((1.0f - (float)(power.Count)) / 2 + (float)(i)) * ((float)(power.MissileAngle) * (float)(Math.PI) / 180.0f);
                float variance = 0.0f;
                if (power.AngleVariance != 0)
                {
                    variance = (float)(Math.Pow(-1.0f, (MathUtils.Rand() % 2) - 1) * (MathUtils.Rand() % power.AngleVariance) * (float)Math.PI / 180.0f);
                }
                float alpha = theta + offsetAngle + variance;
                float speedVar = 0;
                if (power.SpeedVariance != 0)
                {
                    speedVar = ((power.SpeedVariance * 2.0f * (float)(MathUtils.Rand())) / (float)0x7FFF) - power.SpeedVariance;
                }
                haz.BaseSpeed += speedVar;
                //haz SetAngle(alpha);
                haz.DelayFrames = delayIterator;
                delayIterator += power.Delay;
            }

            return true;
        }
        private bool Repeater(Power power, Actor source, PointF target)
        {
            return true;
        }
        private bool Spawn(Power power, Actor source, PointF target)
        {
            return true;
        }
        private bool Transform(Power power, Actor source, PointF target)
        {
            return true;
        }
        private bool Block(Power power, Actor source, PointF target)
        {
            return true;
        }
        private bool Buff(Power power, Actor source, PointF target)
        {
            return true;
        }
        private void PlaySound(Power power)
        {

        }
        public Hazard InitHazard(Power power, Actor source, PointF target)
        {
            Hazard haz = new Hazard();
            haz.Power = power;
            haz.Source = source;
            if (power.SourceType == SourceType.Invalid)
            {
                if (source.IsPlayer) haz.SourceType = SourceType.Hero;
                else if (!source.IsEnemy) haz.SourceType = SourceType.Ally;
                else haz.SourceType = SourceType.Enemy;
            }
            else
            {
                haz.SourceType = power.SourceType;
            }
            haz.BaseLifespan = power.Lifespan;
            haz.Lifespan = power.Lifespan;
            haz.OnFloor = power.OnFloor;
            LoadAnimation(haz);
            if (power.Directional)
            {
                haz.Directional = true;
                haz.Direction = MathUtils.CalcDirection(source.PosX, source.PosY, target.X, target.Y);
            }
            else if (power.VisualRandom > 0)
            {
                haz.Direction = MathUtils.Rand() % power.VisualRandom;
                haz.Direction += power.VisualOption;
            }
            else if (power.VisualOption > 0)
            {
                haz.Direction = power.VisualOption;
            }
            haz.CompleteAnimation = power.CompleteAnimation;
            haz.MultiTarget = power.MultiTarget;
            haz.MultiHit = power.MultiHit;
            haz.AimAssist = power.AimAssist;
            haz.BaseSpeed = power.Speed;
            haz.MovementType = power.MovementType;
            haz.WallReflect = power.WallReflect;
            haz.Radius = power.Radius;
            haz.ExpireWithCaster = power.ExpireWithCaster;
            haz.WallPower = GetPower(power.WallPower);
            haz.WallPowerChance = power.WallPowerChance;
            haz.PostPower = GetPower(power.PostPower);
            haz.PostPowerChance = power.PostPowerChance;
            haz.IgnoreZeroDamage = power.IgnoreZeroDamage;
            haz.Active = !power.NoAttack;
            haz.Beacon = power.Beacon;
            haz.NoAggro = power.NoAggro;
            switch (power.StartingPos)
            {
                case StartingPos.Source:
                    haz.Pos = new PointF(source.PosX, source.PosY);
                    break;
                case StartingPos.Target:
                    haz.Pos = MathUtils.ClampDistance(power.TargetRange, new PointF(source.PosX, source.PosY), target);
                    break;
                case StartingPos.Melee:
                    haz.Pos = MathUtils.CalcVector(source.PosX, source.PosY, source.Direction, source.MeleeRange);
                    break;
            }
            if (power.SfxHitEnable)
            {
                haz.SfxHit = power.SfxHit;
                haz.SfxHitEnable = true;
            }
            haz.DmgMin = (int)(5.0f / power.Count);
            haz.DmgMax = (int)(10.0f / power.Count);
            if (haz.DmgMin < 1) haz.DmgMin = 1;
            if (haz.DmgMax < 2) haz.DmgMax = 2;
            hazards.Add(haz);
            lastHazard = haz;
            return haz;
        }

        private void LoadAnimation(Hazard haz)
        {
            if (haz != null && haz.Power != null)
            {
                var animSet = engine.ContentManager?.Load<AnimationSet>(haz.Power.AnimationName);
                if (animSet != null)
                {
                    var anim = animSet.GetAnimation("");
                    if (anim != null)
                    {
                        haz.Animation = anim;
                    }
                }
            }
        }

        public IList<Power> LoadPowers(string name)
        {
            List<Power> powers = new();
            byte[]? data = engine.ContentManager?.FindContent(name);
            if (data != null)
            {
                using FileParser infile = new FileParser(engine.ContentManager, name, data);
                InternalLoadPowers(infile, powers);
            }
            return powers;
        }

        private static void InternalLoadPowers(FileParser infile, List<Power> list)
        {
            int inputId = 0;
            Power? pow = null;
            bool skippingEntry = false;
            bool clearPostEffects = true;
            while (infile.Next())
            {
                if (infile.MatchSectionKey("power", "id"))
                {
                    pow = null;
                    inputId = infile.GetIntVal();
                    if (inputId > 0)
                    {
                        while (list.Count < inputId + 1)
                        {
                            list.Add(new Power());
                        }
                        pow = list[inputId];
                        pow.Id = inputId;
                        if (!pow.IsEmpty)
                        {
                            SDLLog.Error(LogCategory.APPLICATION, $"Overwriting Power {inputId}");
                        }
                        pow.IsEmpty = false;
                        clearPostEffects = true;
                    }
                }
                else if (pow != null && infile.MatchSectionKey("power", "name")) { pow.Name = infile.GetStrVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "description")) { pow.Description = infile.GetStrVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "type")) { pow.Type = infile.GetEnumValue<PowerType>(); }
                else if (pow != null && infile.MatchSectionKey("power", "source_type")) { pow.SourceType = infile.GetEnumValue<SourceType>(); }
                else if (pow != null && infile.MatchSectionKey("power", "new_state"))
                {
                    if ("instant".Equals(infile.Val)) pow.NewState = PowerState.Instant;
                    else
                    {
                        pow.NewState = PowerState.Attack;
                        pow.AttackAnim = infile.GetStrVal();
                    }
                }
                else if (pow != null && infile.MatchSectionKey("power", "animation")) { pow.AnimationName = infile.GetStrVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "passive")) { pow.Passive = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "buff")) { pow.IsBuff = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "directional")) { pow.Directional = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "visual_random")) { pow.VisualRandom = infile.GetIntVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "visual_option")) { pow.VisualOption = infile.GetIntVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "complete_animation")) { pow.CompleteAnimation = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "lock_target_to_direction")) { pow.LockTargetToDirection = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "use_hazard")) { pow.UseHazard = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "no_attack")) { pow.NoAttack = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "no_aggro")) { pow.NoAggro = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "cooldown")) { pow.Cooldown = FileParser.ParseDurationMS(infile.GetStrVal()); }
                else if (pow != null && infile.MatchSectionKey("power", "lifespan")) { pow.Lifespan = FileParser.ParseDurationMS(infile.GetStrVal()); }
                else if (pow != null && infile.MatchSectionKey("power", "face")) { pow.Facing = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "floor")) { pow.OnFloor = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "expire_with_caster")) { pow.ExpireWithCaster = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "delay")) { pow.Delay = FileParser.ParseDurationMS(infile.GetStrVal()); }
                else if (pow != null && infile.MatchSectionKey("power", "speed")) { pow.Speed = infile.GetFloatVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "missile_angle")) { pow.MissileAngle = infile.GetIntVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "angle_variance")) { pow.AngleVariance = infile.GetIntVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "speed_variance")) { pow.SpeedVariance = infile.GetIntVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "count")) { pow.Count = infile.GetIntVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "ignore_zero_damage")) { pow.IgnoreZeroDamage = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "spawn_type")) { pow.SpawnType = infile.GetStrVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "target_neighbor")) { pow.TargetNeighbor = infile.GetIntVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "beacon")) { pow.Beacon = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "wall_power")) { }
                else if (pow != null && infile.MatchSectionKey("power", "post_power")) { }
                else if (pow != null && infile.MatchSectionKey("power", "pre_power")) { }

                else if (pow != null && infile.MatchSectionKey("power", "wall_reflect")) { pow.WallReflect = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "radius")) { pow.Radius = infile.GetFloatVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "target_range")) { pow.TargetRange = infile.GetFloatVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "multitarget")) { pow.MultiTarget = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "multihit")) { pow.MultiHit = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "aim_assist")) { pow.AimAssist = infile.GetBoolVal(); }
                else if (pow != null && infile.MatchSectionKey("power", "starting_pos")) { pow.StartingPos = infile.GetEnumValue<StartingPos>(); }

            }

        }
    }
}
