namespace SDLSharp.Maps
{
    using SDLSharp.Actors;
    using SDLSharp.Content;
    using SDLSharp.Events;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Map : Resource
    {
        private readonly int width;
        private readonly int height;
        private readonly List<MapLayer> layers = new();
        private Color backgroundColor;
        private TileSet? tileSet;
        private TileSet? collisionTileSet;
        private IMapCollision? collision;
        private MapParallax? parallax;
        private List<ActorInfo> actorInfos = new();
        private List<EventInfo> eventInfos = new();

        public Map(string name, int width, int height)
            : base(name, ContentFlags.Data)
        {
            this.width = width;
            this.height = height;
            backgroundColor = Color.Black;
            Title = name;
            Music = "";
        }
        public int Width => width;
        public int Height => height;
        public string Title { get; set; }
        public string Music { get; set; }
        public MapProjection Projection { get; set; }
        public int StartPosX { get; set; }
        public int StartPosY { get; set; }

        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }
        public TileSet? TileSet
        {
            get => tileSet;
            set => tileSet = value;
        }

        public TileSet? CollisionTileSet
        {
            get => collisionTileSet;
            set => collisionTileSet = value;
        }
        public MapParallax? Parallax
        {
            get => parallax;
            set => parallax = value;
        }
        public IMapCollision? Collision => collision;

        public IEnumerable<ActorInfo> ActorInfos => actorInfos;
        public void AddActorInfo(ActorInfo info)
        {
            actorInfos.Add(info);
        }
        public IEnumerable<EventInfo> EventInfos => eventInfos;

        public void AddEventInfo(EventInfo info)
        {
            eventInfos.Add(info);
        }
        public IEnumerable<MapLayer> Layers => layers;

        public void AddLayer(MapLayer layer)
        {
            layers.Add(layer);
        }

        public bool RemoveLayer(MapLayer layer)
        {
            return layers.Remove(layer);
        }

        public bool RemoveLayerAt(int index)
        {
            if (index >= 0 && index < layers.Count)
            {
                layers.RemoveAt(index);
                return true;
            }
            return false;
        }
        public void InitCollision()
        {
            MapLayer? collisionLayer = layers.FirstOrDefault(x => x.Type == LayerType.Collision);
            if (collisionLayer != null)
            {
                collision = new AStar.MapCollision(collisionLayer);
                layers.Remove(collisionLayer);
            }
            else
            {
                collision = new AStar.MapCollision(width, height);
            }
        }

        public void Modify(IEnumerable<MapMod> mapMods)
        {
            foreach (MapMod mod in mapMods)
            {
                Modify(mod);
            }
        }
        public void Modify(MapMod mapMod)
        {
            if (mapMod.Layer == "collision" && collision != null)
            {
                collision.ColMap[mapMod.MapX, mapMod.MapY] = mapMod.MapValue;
            }
            else
            {
                MapLayer? layer = layers.FirstOrDefault(x => x.Name == mapMod.Layer);
                if (layer != null)
                {
                    layer[mapMod.MapX, mapMod.MapY] = mapMod.MapValue;
                }
            }
        }

        public void Spawn(IEnumerable<MapSpawn> mapSpawns)
        {
            foreach(MapSpawn spawn in mapSpawns)
            {
                Spawn(spawn);
            }
        }

        public void Spawn(MapSpawn mapSpawn)
        {

        }
        protected override void DisposeUnmanaged()
        {
            tileSet?.Dispose();
            tileSet = null;
            collisionTileSet?.Dispose();
            collisionTileSet = null;
        }
    }
}
