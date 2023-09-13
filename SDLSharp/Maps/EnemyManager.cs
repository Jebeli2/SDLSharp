namespace SDLSharp.Maps
{
    using SDLSharp.Actors;
    using SDLSharp.Content;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    //public class EnemyManager
    //{
    //    private readonly List<EnemyGroup> enemyGroups = new();
    //    private readonly List<Enemy> enemies = new();
    //    private readonly ActorManager actorManager;
    //    private Map? map;

    //    public EnemyManager(ActorManager actorManager)
    //    {
    //        this.actorManager = actorManager;
    //    }
    //    public IContentManager? ContentManager { get; set; }
    //    public Map? Map
    //    {
    //        get => map;
    //        set => map = value;
    //    }

    //    public void Clear()
    //    {
    //        enemyGroups.Clear();
    //        enemies.Clear();
    //    }
    //    public void AddEnemyGroup(EnemyGroup eg)
    //    {
    //        enemyGroups.Add(eg);
    //    }

    //    public void SpawnMapEnemies(Map map)
    //    {
    //        Clear();
    //        foreach(var eg in map.EnemyGroups)
    //        {
    //            AddEnemyGroup(eg);
    //        }
    //        SpawnEnemies();
    //    }

    //    public void SpawnMapSpawn(MapSpawn spawn)
    //    {
    //        EnemyGroup eg = new EnemyGroup(spawn.Type);
    //        eg.MinNumber = 1;
    //        eg.MaxNumber = 1;
    //        eg.MinLevel = 1;
    //        eg.MaxLevel = 1;
    //        eg.PosX = spawn.MapX;
    //        eg.PosY = spawn.MapY;
    //        eg.Width = 1;
    //        eg.Height = 1;
    //        SpawnEnemyGroup(eg);
    //    }

    //    public void SpawnEnemies()
    //    {
    //        foreach (var eg in enemyGroups)
    //        {
    //            SpawnEnemyGroup(eg);
    //        }
    //    }

    //    private void SpawnEnemyGroup(EnemyGroup eg)
    //    {
    //        int num = MathUtils.RandBewteen(eg.MinNumber, eg.MaxNumber);
    //        string eName = eg.Name + ".txt";
    //        for (int i = 0; i < num; i++)
    //        {
    //            Enemy? e = ContentManager?.Load<Enemy>(eName);
    //            if (e != null)
    //            {
    //                SetEnemyPosition(e, eg);
    //                e.SetDirection(MathUtils.Rand() % 7);
    //                actorManager.AddActor(e);
    //                e.SetAnimation("spawn");
    //                enemies.Add(e);
    //            }
    //        }
    //    }

    //    private void SetEnemyPosition(Enemy ent, EnemyGroup eg)
    //    {
    //        int x = 0;
    //        int y = 0;
    //        int count = 0;
    //        if (map != null && map.Collision != null)
    //        {

    //            while (!map.Collision.IsValidPosition(x, y, ent.MovementType, ent.CollisionType) && count < 10)
    //            {
    //                x = eg.PosX + MathUtils.RandBewteen(0, eg.Width);
    //                y = eg.PosY + MathUtils.RandBewteen(0, eg.Height);
    //                count++;
    //            }
    //            ent.SetPosition(x, y);
    //        }
    //    }
    //}
}
