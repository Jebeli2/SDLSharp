namespace SDLSharp.Actors
{
    using SDLSharp.Content;
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ActorManager
    {
        private readonly List<Actor> actors = new();
        private Actor? player;

        public ActorManager()
        {

        }

        public IContentManager? ContentManager { get; set; }
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
                    player.DefaultSpeed *= 2;
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
                }
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

        public void Update(double totalTime, double elapsedTime)
        {
            foreach (var actor in actors)
            {
                actor.Update(totalTime, elapsedTime);
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
