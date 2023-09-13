namespace SDLSharp.Actors
{
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEnemyManager
    {
        void SpawnMapSpawn(Map map, MapSpawn spawn);
    }
}
