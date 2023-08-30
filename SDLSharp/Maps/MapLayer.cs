namespace SDLSharp.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MapLayer
    {
        private string name = string.Empty;
        private byte alpha = 255;
        private bool visible;
        private int width;
        private int height;
        private int tileWidth;
        private int tileHeight;
        private float offsetX;
        private float offsetY;
        private int[,] tiles;
        private LayerType layerType;

        public MapLayer(string name, int width, int height, int tileWidth, int tileHeight)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            tiles = new int[width, height];
        }

        public LayerType Type
        {
            get => layerType;
            set => layerType = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public byte Alpha
        {
            get => alpha;
            set => alpha = value;
        }

        public bool Visible
        {
            get => visible;
            set => visible = value;
        }

        public float OffsetX
        {
            get => offsetX;
            set => offsetX = value;
        }

        public float OffsetY
        {
            get => offsetY;
            set => offsetY = value;
        }

        public int Width => width;
        public int Height => height;

        public int TileWidth
        {
            get => tileWidth;
            set => tileWidth = value;
        }
        public int TileHeight
        {
            get => tileHeight;
            set => tileHeight = value;
        }

        public int this[int x, int y]
        {
            get => tiles[x, y];
            set => tiles[x, y] = value;
        }
    }
}
