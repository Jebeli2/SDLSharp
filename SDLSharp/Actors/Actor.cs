namespace SDLSharp.Actors
{
    using SDLSharp.Content;
    using SDLSharp.Graphics;
    using SDLSharp.Maps;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Actor : Resource
    {
        private readonly List<ActorCommand> commandQueue = new();
        private ActorCommand? currentCommand;
        private List<PointF>? path;
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
            VoxIntros = new List<string>();
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
        public IList<string> VoxIntros { get; set; }

        public MovementType MovementType => movementType;
        public CollisionType CollisionType
        {
            get { return IsPlayer ? CollisionType.Player : CollisionType.Normal; }
        }
        public ActorCommand? CurrentCommand => currentCommand;
        public IList<PointF>? Path
        {
            get { return path; }
            set
            {
                if (value != null && value.Count > 0)
                {
                    path = new List<PointF>(value);
                }
                else
                {
                    path = null;
                }
            }
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
                case "spawn":
                    if (HasAnimationFinished)
                    {
                        return true;
                    }
                    else
                    {
                        int frame = visual.Frame;
                        int count = visual.FrameCount;
                        if (frame >= count)
                        {
                            return true;
                        }
                    }
                    //if (!HasAnimationFinished)
                    //{
                    //    visual.Update(10, 160);
                    //    visual.Update(10, 160);
                    //    visual.Update(10, 160);
                    //    visual.Update(10, 160);
                    //    visual.Update(10, 160);
                    //    visual.Update(10, 160);
                    //    visual.Update(10, 160);
                    //    return true;
                    //}
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

        public ActorCommand? GetNextCommand()
        {
            if (currentCommand == null && commandQueue.Count > 0)
            {
                currentCommand = commandQueue[0];
                commandQueue.RemoveAt(0);
                //Logger.Info($"Next Command {currentCommand} for Entity {this}, {commandQueue.Count} Commands remaining");
                return currentCommand;
            }
            return null;
        }

        public void ClearCommands(ActorAction action)
        {
            //Logger.Info($"Clearing {action} Commands for Entity {this}");
            if (currentCommand != null && currentCommand.Action == action)
            {
                currentCommand = null;
            }
            for (int i = commandQueue.Count - 1; i >= 0; i--)
            {
                if (commandQueue[i].Action == action)
                {
                    commandQueue.RemoveAt(i);
                }
            }
        }
        public void ClearCommands()
        {
            //Logger.Info($"Clearing Commands for Entity {this}");
            currentCommand = null;
            commandQueue.Clear();
        }
        public void QueueCommand(ActorCommand command)
        {
            commandQueue.Add(command);
            //Logger.Info($"Queued Command {command} for Entity {this}");
        }

        public void ReplaceCommand(ActorCommand command)
        {
            ClearCommands(command.Action);
            commandQueue.Add(command);
        }

        public void QueueMove(float destX, float destY)
        {
            QueueCommand(new ActorCommand { Action = ActorAction.Move, MapDestX = destX, MapDestY = destY });
        }

        public void QueueInteract(float destX, float destY)
        {
            QueueCommand(new ActorCommand { Action = ActorAction.Interact, MapDestX = destX, MapDestY = destY });
        }

        public void QueueAttack(Actor? enemy)
        {
            QueueCommand(new ActorCommand { Action = ActorAction.Attack, Enemy = enemy });
        }

        public void ReplaceMove(float destX, float destY)
        {
            ReplaceCommand(new ActorCommand { Action = ActorAction.Move, MapDestX = destX, MapDestY = destY });
        }
        public void ReplaceInteract(float destX, float destY)
        {
            ReplaceCommand(new ActorCommand { Action = ActorAction.Interact, MapDestX = destX, MapDestY = destY });
        }

        public void ReplaceAttack(Actor? enemy)
        {
            ReplaceCommand(new ActorCommand { Action = ActorAction.Attack, Enemy = enemy });
        }

        public void Stop()
        {
            SetAnimation("stance");
            path = null;
            ClearCommands(ActorAction.Move);
        }

        public void Face(Actor? other)
        {
            if (other != null)
            {
                SetDirection(MathUtils.CalcDirection(PosX, PosY, other.PosX, other.PosY));
            }
        }

        public void Attack(Actor? other)
        {
            //enemy = null;
            if (IsAdjacentTo(other))
            {
                Face(other);
                SetAnimation("swing");
                //manager.PlaySound(this, SfxType.Attack, "swing");
                //enemy = other;
                // temp
                //enemy?.TakeHit(this);
            }
        }
        public void TakeHit(Actor? other)
        {
            //enemy = null;
            if (IsAdjacentTo(other))
            {
                Face(other);
                //enemy = other;
                //hitCount++;
                // temp
                //if (hitCount > 2 && !IsPlayer)
                //{
                //    if (Utils.Rand() % 3 > 1)
                //    {
                //        CritDie();
                //    }
                //    else
                //    {
                //        Die();
                //    }
                //}
                //else
                //{
                //    SetAnimation("hit");
                //    manager.PlaySound(this, SfxType.Hit);
                //}
            }
        }

        public void Die()
        {
            SetAnimation("die");
            //manager.PlaySound(this, SfxType.Die);
            Dying = true;
            //Dead = true;
        }

        public void CritDie()
        {
            SetAnimation("critdie");
            //manager.PlaySound(this, SfxType.CritDie);
            Dying = true;
            //Dead = true;
        }

        public bool LoadAnimations(IContentManager? contentManager, IDictionary<string, string> animationParts, IDictionary<int, IList<string>> layerOrder)
        {
            if (contentManager != null && animationParts.Count > 0)
            {
                if (animationParts.Count > 1 && layerOrder.Count >= animationParts.Count)
                {
                    var animSets = new Dictionary<string, AnimationSet>();
                    foreach (var kvp in animationParts)
                    {
                        AnimationSet? animSet = contentManager?.Load<AnimationSet>(kvp.Value);
                        if (animSet != null)
                        {
                            animSets[kvp.Key] = animSet;
                        }
                    }
                    Visual = new MultiPartVisual(animSets, layerOrder);
                }
                else
                {
                    foreach (var kvp in animationParts)
                    {
                        AnimationSet? animSet = contentManager?.Load<AnimationSet>(kvp.Value);
                        if (animSet != null)
                        {
                            Visual = new AnimationSetVisual(animSet);
                            break;
                        }
                    }
                }
            }
            return Visual != null;
        }

    }
}
