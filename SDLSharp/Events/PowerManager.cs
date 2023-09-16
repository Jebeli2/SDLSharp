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
        private Actor eventActor;

        public PowerManager(IMapEngine engine)
        {
            this.engine = engine;
            eventActor = new Actor("event");
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

        public IEnumerable<Hazard> Hazards => hazards;

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

        public bool Activate(Power power, Event evt)
        {
            float x = evt.Location.X + evt.Location.Width / 2.0f;
            float y = evt.Location.Y + evt.Location.Height / 2.0f;
            eventActor.SetPosition(x, y);
            return Activate(power, eventActor, new PointF(x, y));
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
                haz.SetAngle(alpha);
                haz.DelayFrames = delayIterator;
                delayIterator += power.Delay;
            }
            PlaySound(power);
            return true;
        }
        private bool Repeater(Power power, Actor source, PointF target)
        {
            if (engine.Map?.Collision == null) return false;
            float theta = MathUtils.CalcTheta(source.PosX,source.PosY,target.X,target.Y);
            int delayIterator = 0;
            PlaySound(power);
            PointF locationIterator = new PointF(source.PosX,source.PosY);
            PointF speed = new PointF();
            speed.X = power.Speed * MathF.Cos(theta);
            speed.Y = power.Speed * MathF.Sin(theta);
            Hazard? parentHaz = null;
            for(int i = 0; i < power.Count; i++)
            {
                locationIterator.X += speed.X;
                locationIterator.Y += speed.Y;
                if (engine.Map.Collision.IsValidPosition(locationIterator.X, locationIterator.Y, power.MovementType, CollisionType.Normal))
                {
                    break;
                }
                Hazard haz = InitHazard(power, source, target);
                haz.Pos = locationIterator;
                haz.DelayFrames = delayIterator;
                delayIterator += power.Delay;
                if (i == 0 && power.Count > 1) { parentHaz = haz; }
                else if (parentHaz != null && i > 0)
                {
                    parentHaz.AddChild(haz);
                }
            }
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
                var animSet = haz.Power.GetAnimationSet(engine.ContentManager);
                if (animSet != null)
                {
                    haz.Visual = new AnimationSetVisual(animSet);
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
                InternalLoadPowers(infile, name, powers);
                SDLLog.Info(LogCategory.APPLICATION, $"Loaded {powers.Count(x => !x.IsEmpty)} powers from '{name}'");
            }
            return powers;
        }

        private static void InternalLoadPowers(FileParser infile, string name, List<Power> list)
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
                else
                {
                    switch (infile.Section)
                    {
                        case "power":
                            if (pow != null)
                            {
                                switch (infile.Key)
                                {
                                    case "name": pow.Name = infile.GetStrVal(); break;
                                    case "description": pow.Description = infile.GetStrVal(); break;
                                    case "type": pow.Type = infile.GetEnumValue<PowerType>(); break;
                                    case "source_type": pow.SourceType = infile.GetEnumValue<SourceType>(); break;
                                    case "new_state":
                                        if ("instant".Equals(infile.Val))
                                        {
                                            pow.NewState = PowerState.Instant;
                                        }
                                        else
                                        {
                                            pow.NewState = PowerState.Attack;
                                            pow.AttackAnim = infile.GetStrVal();
                                        }
                                        break;
                                    case "animation": pow.AnimationName = infile.GetStrVal(); break;
                                    case "passive": pow.Passive = infile.GetBoolVal(); break;
                                    case "directional": pow.Directional = infile.GetBoolVal(); break;
                                    case "visual_random": pow.VisualRandom = infile.GetIntVal(); break;
                                    case "visual_option": pow.VisualOption = infile.GetIntVal(); break;
                                    case "complete_animation": pow.CompleteAnimation = infile.GetBoolVal(); break;
                                    case "lock_target_to_direction": pow.LockTargetToDirection = infile.GetBoolVal(); break;
                                    case "use_hazard": pow.UseHazard = infile.GetBoolVal(); break;
                                    case "no_attack": pow.NoAttack = infile.GetBoolVal(); break;
                                    case "no_aggro": pow.NoAggro = infile.GetBoolVal(); break;
                                    case "cooldown": pow.Cooldown = FileParser.ParseDurationMS(infile.GetStrVal()); break;
                                    case "lifespan": pow.Lifespan = FileParser.ParseDurationMS(infile.GetStrVal()); break;
                                    case "face": pow.Facing = infile.GetBoolVal(); break;
                                    case "floor": pow.OnFloor = infile.GetBoolVal(); break;
                                    case "expire_with_caster": pow.ExpireWithCaster = infile.GetBoolVal(); break;
                                    case "delay": pow.Delay = FileParser.ParseDurationMS(infile.GetStrVal()); break;
                                    case "speed": pow.Speed = infile.GetFloatVal(); break;
                                    case "missile_angle": pow.MissileAngle = infile.GetIntVal(); break;
                                    case "angle_variance": pow.AngleVariance = infile.GetIntVal(); break;
                                    case "speed_variance": pow.SpeedVariance = infile.GetIntVal(); break;
                                    case "count": pow.Count = infile.GetIntVal(); break;
                                    case "ignore_zero_damage": pow.IgnoreZeroDamage = infile.GetBoolVal(); break;
                                    case "spawn_type": pow.SpawnType = infile.GetStrVal(); break;
                                    case "spawn_limit": break;
                                    case "spawn_level": break;
                                    case "target_neighbor": pow.TargetNeighbor = infile.GetIntVal(); break;
                                    case "beacon": pow.Beacon = infile.GetBoolVal(); break;
                                    case "wall_power": break;
                                    case "post_power": break;
                                    case "pre_power": break;
                                    case "wall_reflect": pow.WallReflect = infile.GetBoolVal(); break;
                                    case "radius": pow.Radius = infile.GetFloatVal(); break;
                                    case "target_range": pow.TargetRange = infile.GetFloatVal(); break;
                                    case "multitarget": pow.MultiTarget = infile.GetBoolVal(); break;
                                    case "multihit": pow.MultiHit = infile.GetBoolVal(); break;
                                    case "aim_assist": pow.AimAssist = infile.GetBoolVal(); break;
                                    case "starting_pos": pow.StartingPos = infile.GetEnumValue<StartingPos>(); break;
                                    case "icon": break;
                                    case "base_damage": break;
                                    case "replace_by_effect": break;
                                    case "state_duration": break;
                                    case "requires_flags": break;
                                    case "charge_speed": break;
                                    case "relative_pos": break;
                                    case "modifier_critical": break;
                                    case "soundfx_hit": break;
                                    case "soundfx": break;
                                    case "post_effect": break;
                                    case "modifier_damage": break;
                                    case "modifier_accuracy": break;
                                    case "buff": pow.IsBuff = infile.GetBoolVal(); break;
                                    case "buff_teleport": break;
                                    case "buff_party": break;
                                    case "meta_power": break;
                                    case "trait_armor_penetration": break;
                                    case "trait_elemental": break;
                                    case "trait_crits_impaired": break;
                                    case "remove_effect": break;
                                    case "target_categories": break;
                                    case "requires_spawns": break;
                                    case "requires_empty_target": break;
                                    case "requires_los": break;
                                    case "requires_targeting": break;
                                    case "requires_item": break;
                                    case "requires_equipped_item": break;
                                    case "requires_mp": break;
                                    case "requires_hp": break;
                                    case "requires_hpmp_state": break;
                                    case "sacrifice": break;
                                    case "hp_steal": break;
                                    case "mp_steal": break;
                                    case "script": break;
                                    default: ModLoader.UnknownKey(name, infile); break;
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
