namespace SDLSharp.Actors
{
    using SDLSharp.Content;
    using SDLSharp.Events;
    using SDLSharp.Maps;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ActorManager
    {
        private static readonly float M_SQRT2 = MathF.Sqrt(2.0f);
        private static readonly float M_SQRT2INV = 1.0f / M_SQRT2;
        private static readonly int[] directionDeltaX = { -1, -1, -1, 0, 1, 1, 1, 0 };
        private static readonly int[] directionDeltaY = { 1, 0, -1, -1, -1, 0, 1, 1 };
        private static readonly float[] speedMultiplyer = { M_SQRT2INV, 1.0f, M_SQRT2INV, 1.0f, M_SQRT2INV, 1.0f, M_SQRT2INV, 1.0f };

        private readonly List<Actor> actors = new();
        private Actor? player;
        private Map? map;
        private IMapCamera? camera;
        private EventManager? eventManager;

        public ActorManager()
        {

        }

        public IContentManager? ContentManager { get; set; }
        public Map? Map
        {
            get => map;
            set => map = value;
        }

        public IMapCamera? Camera
        {
            get => camera;
            set => camera = value;
        }

        public EventManager? EventManager
        {
            get => eventManager;
            set => eventManager = value;
        }
        public ActorInfo? PlayerInfo { get; set; }
        public Actor? Player => player;
        public IList<IMapSprite> GetLivingSprites()
        {
            return GetSprites(alive: true);
        }
        public IList<IMapSprite> GetDeadSprites()
        {
            return GetSprites(alive: false);
        }

        public IList<IMapSprite> GetSprites(bool alive = true)
        {
            List<IMapSprite> list = new();
            foreach (Actor actor in actors)
            {
                if (actor.Alive == alive)
                {
                    foreach (var sprite in actor.GetSprites())
                    {
                        list.Add(sprite);
                    }
                }
            }
            CalculatePriosIso(list);
            list.Sort();
            return list;
        }

        public void Clear()
        {
            actors.Clear();
        }

        public void SpawnPlayer(Map map)
        {
            if (PlayerInfo != null)
            {
                if ((PlayerInfo.PosX < 0 || PlayerInfo.PosY < 0) && (map.StartPosX >= 0 && map.StartPosY >= 0))
                {
                    PlayerInfo.PosX = map.StartPosX;
                    PlayerInfo.PosY = map.StartPosY;
                }
                Actor? actor = AddActor(PlayerInfo);
                if (actor != null)
                {
                    player = actor;
                    player.DefaultSpeed *= 1.1f;
                    player.IsPlayer = true;
                }
            }
        }
        public void SpawnMapActors(Map map)
        {
            foreach (ActorInfo info in map.ActorInfos)
            {
                Actor? actor = AddActor(info);
                if (actor != null)
                {
                    map.Collision?.Block(actor.PosX, actor.PosY, false);
                    eventManager?.CreateNPCEvent(actor);
                }
            }
        }

        public void MakeCommands(Actor actor, int mouseX, int mouseY)
        {
            if (camera == null) return;
            //if (eventManager == null) return;
            camera.ScreenToMap(mouseX, mouseY, out float mapX, out float mapY);
            mapX = MathUtils.RoundForMap(mapX);
            mapY = MathUtils.RoundForMap(mapY);
            bool hasEvent = eventManager?.HasAnyEventsAt(mapX, mapY) ?? false;
            Actor? mouseEnemy = null;
            Actor? mouseActor = GetActor(mouseX, mouseY);
            bool hasEnemy = mouseEnemy != null;
            bool hasActor = mouseActor != null;
            if (hasEnemy)
            {
                actor.ClearCommands();
                if (!actor.IsAdjacentTo(mouseEnemy))
                {
                    actor.QueueMove(mapX, mapY);
                }
                actor.QueueAttack(mouseEnemy);
            }
            else if (hasEvent)
            {
                actor.ClearCommands();
                actor.QueueMove(mapX, mapY);
                actor.QueueInteract(mapX, mapY);
            }
            else if (hasActor)
            {
                actor.ClearCommands();
                actor.QueueMove(mapX, mapY);
                actor.QueueInteract(mapX, mapY);
            }
            else
            {
                actor.ClearCommands();
                actor.QueueMove(mapX, mapY);
            }
        }

        private void ExecuteCommand(Actor actor, ActorCommand? cmd)
        {
            if (cmd == null) return;
            if (map == null) return;
            if (map.Collision == null) return;
            //if (eventManager == null) return;
            switch (cmd.Action)
            {
                case ActorAction.Move:
                    List<PointF> path = new();
                    if (map.Collision.ComputePath(actor.PosX, actor.PosY, cmd.MapDestX, cmd.MapDestY, path, MovementType.Normal))
                    {
                        actor.Path = path;
                    }
                    else
                    {
                        actor.Stop();
                    }
                    break;
                case ActorAction.Interact:
                    eventManager?.CheckClickEvents(actor.PosX, actor.PosY, cmd.MapDestX, cmd.MapDestY);
                    break;
                case ActorAction.Attack:
                    actor.Attack(cmd.Enemy);
                    break;
            }
        }
        private Actor? AddActor(ActorInfo info)
        {
            Actor? actor = ContentManager?.Load<Actor>(info.Id);
            if (actor != null)
            {
                actor.SetPosition(info.PosX + 0.5f, info.PosY + 0.5f);
                actor.SetDirection(0);
                actor.SetAnimation("stance");

                actors.Add(actor);
            }
            return actor;
        }

        private void UpdatePath(Actor? actor)
        {
            if (actor == null) return;
            if (map == null) return;
            if (map.Collision == null) return;
            var cmd = actor.CurrentCommand;
            if (cmd != null && cmd.Action == ActorAction.Move)
            {
                List<PointF> path = new();
                if (map.Collision.ComputePath(actor.PosX, actor.PosY, cmd.MapDestX, cmd.MapDestY, path, MovementType.Normal))
                {
                    actor.Path = path;
                }
                else
                {
                    actor.Path = null;
                }
            }
        }
        private void FollowPath(Actor actor, double totalTime, double elapsedTime)
        {
            if (map == null) return;
            if (map.Collision == null) return;
            if (actor.Dead) return;
            if (actor.Dying) return;
            if (actor.Path != null && actor.Path.Count > 0)
            {
                PointF src = new PointF(actor.PosX, actor.PosY);
                map.Collision.Unblock(src.X, src.Y);
                PointF dest = actor.Path[^1];
                if (actor.Collided || !map.Collision.IsValidPosition(dest.X, dest.Y, actor.MovementType, actor.CollisionType))
                {
                    UpdatePath(actor);
                    if (actor.Path != null && actor.Path.Count > 0)
                    {
                        dest = actor.Path[^1];
                    }
                }
                int oldDir = actor.Direction;
                int dir = MathUtils.CalcDirection(src.X, src.Y, dest.X, dest.Y);
                if (oldDir != dir)
                {
                    actor.SetDirection(dir);
                }
                if (Move(actor))
                {
                    actor.SetAnimation("run");
                }
                else
                {
                    int prevDirection = actor.Direction;
                    int nextDir = MathUtils.NextBestDirection(prevDirection, dest.X, dest.Y, src.X, src.Y);
                    if (nextDir != prevDirection)
                    {
                        actor.SetDirection(nextDir);
                        if (Move(actor))
                        {
                            actor.SetAnimation("run");
                        }
                        else
                        {
                            actor.Collided = true;
                            actor.SetDirection(prevDirection);
                        }
                    }
                }
                if (actor.IsAt(dest.X, dest.Y))
                {
                    actor.Path?.RemoveAt(actor.Path.Count - 1);
                }
                map.Collision.Block(actor.PosX, actor.PosY, actor.IsPlayer);
            }
            else
            {
                if (actor.IsMoving)
                {
                    actor.Stop();
                }
            }
        }

        private bool Move(Actor entity)
        {
            if (map == null) return false;
            if (map.Collision == null) return false;
            float speed = entity.Speed;
            speed *= speedMultiplyer[entity.Direction];
            float dx = speed * directionDeltaX[entity.Direction];
            float dy = speed * directionDeltaY[entity.Direction];
            float x = entity.PosX;
            float y = entity.PosY;
            bool fullMove = map.Collision.Move(ref x, ref y, dx, dy, MovementType.Normal, CollisionType.Player);
            if (fullMove)
            {
                entity.HasMoved = true;
                entity.Collided = false;
                x = MathF.Round(x, 5);
                y = MathF.Round(y, 5);
                entity.SetPosition(x, y);
            }
            return fullMove;
        }

        public void Update(double totalTime, double elapsedTime)
        {
            foreach (var actor in actors)
            {
                ExecuteCommand(actor, actor.GetNextCommand());
                FollowPath(actor, totalTime, elapsedTime);
                actor.Update(totalTime, elapsedTime);
            }
        }
        public Actor? GetActor(int mouseX, int mouseY)
        {
            if (map == null) return null;
            if (camera == null) return null;
            camera.ScreenToMap(mouseX, mouseY, out float mapX, out float mapY);
            foreach (var a in GetNearActors(mapX, mapY, 4))
            {
                camera.MapToScreen(a.PosX, a.PosY, out int sx, out int sy);
                foreach (var sprite in a.GetSprites())
                {
                    int rx = sx - sprite.OffsetX;
                    int ry = sy - sprite.OffsetY;
                    if (mouseX > rx && mouseY > ry && mouseX < rx + sprite.Width && mouseY < ry + sprite.Height)
                    {
                        return a;
                    }
                }
            }
            return null;
        }
        private IEnumerable<Actor> GetNearActors(float x, float y, float distance)
        {
            foreach (var entity in actors)
            {
                float dx = MathF.Abs(entity.PosX - x);
                float dy = MathF.Abs(entity.PosY - y);
                if (dx <= distance && dy <= distance)
                {
                    yield return entity;
                }
            }
        }


        private static void CalculatePriosIso(IEnumerable<IMapSprite> r)
        {
            foreach (var it in r)
            {
                uint tilex = (uint)(Math.Floor(it.MapPosX));
                uint tiley = (uint)(Math.Floor(it.MapPosY));
                int commax = (int)((it.MapPosX - tilex) * (2 << 16));
                int commay = (int)((it.MapPosY - tiley) * (2 << 16));
                long p1 = tilex + tiley;
                p1 <<= 54;
                long p2 = tilex;
                p2 <<= 42;
                long p3 = commax + commay;
                p3 <<= 16;
                it.Prio = it.BasePrio + (p1 + p2 + p3);
            }
        }
    }
}
