namespace SDLSharp.Actors
{
    using SDLSharp.Maps;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    public class EnemyManager : IEnemyManager
    {
        private readonly IMapEngine engine;
        private readonly List<EnemyTemplate> templates = new();
        internal EnemyManager(IMapEngine engine)
        {
            this.engine = engine;
        }

        public bool Initialized => templates.Count > 0;
        public void AddEnemyTemplates(IEnumerable<string> enemies)
        {
            foreach (var e in enemies)
            {
                //var et = application.Files.GetEntityTemplate(e);
                var et = engine.LoadEnemyTemplate(e);
                if (et != null)
                {
                    SDLLog.Info(LogCategory.APPLICATION, $"Adding Enemy Template {et.Name}");
                    templates.Add(et);
                }
            }
        }
        public void Clear()
        {

        }

        public void SpawnEnemies(Map map)
        {
            foreach (var info in map.EnemyInfos)
            {
                SpawnEnemies(map, info);
            }
        }

        private void SpawnEnemies(Map map, EnemyInfo info)
        {
            int num = MathUtils.RandBewteen(info.MinNumber, info.MaxNumber);
            for (int i = 0; i < num; i++)
            {
                EnemyTemplate? et = FindEnemyTemplate(info);
                if (et != null)
                {
                    Enemy? enemy = BuildEnemy(et);
                    if (enemy != null)
                    {
                        SetEnemyPosition(map, enemy, info);
                        int dir = info.Direction;
                        if (dir < 0) dir = MathUtils.Rand() % 7;
                        enemy.SetDirection(dir);
                        enemy.SetAnimation("spawn");
                        engine.ActorManager.AddActor(enemy);
                    }
                }
            }
        }

        private Enemy? BuildEnemy(EnemyTemplate et)
        {
            Enemy enemy = new Enemy(et.Name);
            if (enemy.LoadAnimations(engine.ContentManager, et.AnimationParts, et.LayerOrder))
            {
                return enemy;
            }
            return null;
        }

        private EnemyTemplate? FindEnemyTemplate(EnemyInfo info)
        {
            List<EnemyTemplate> candidates = new();
            var catMatch = templates.Where(e => e.Categories.Contains(info.Category));
            foreach (var e in catMatch)
            {
                if ((e.Level >= info.MinLevel && e.Level <= info.MaxLevel) || (info.MinLevel == 0 && info.MaxLevel == 0))
                {
                    int addTimes = 0;
                    switch (e.Rarity)
                    {
                        case "common":
                            addTimes = 6;
                            break;
                        case "uncommon":
                            addTimes = 3;
                            break;
                        case "rare":
                            addTimes = 1;
                            break;
                    }
                    for (int j = 0; j < addTimes; j++)
                    {
                        candidates.Add(e);
                    }
                }
            }
            if (candidates.Count > 0)
            {
                return candidates[MathUtils.Rand() % candidates.Count];
            }
            return null;
        }

        private void SetEnemyPosition(Map? map, Enemy ent, EnemyInfo eg)
        {
            if (map == null) return;
            if (map.Collision == null) return;
            int x = -1;
            int y = -1;
            int count = 0;
            while (!map.Collision.IsValidPosition(x, y, ent.MovementType, ent.CollisionType) && count < 10)
            {
                if (count == 0)
                {
                    x = eg.PosX + MathUtils.RandBewteen(0, eg.Width);
                    y = eg.PosY + MathUtils.RandBewteen(0, eg.Height);
                }
                else
                {
                    x = eg.PosX + MathUtils.RandBewteen(-1, eg.Width + 1);
                    y = eg.PosY + MathUtils.RandBewteen(-1, eg.Height + 1);
                }
                count++;
            }
            ent.SetPosition(x + 0.5f, y + 0.5f);
        }
    }
}
