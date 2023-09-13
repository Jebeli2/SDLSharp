namespace SDLSharp.Maps
{
    using SDLSharp.Content;
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TileSet : Resource
    {
        private readonly List<IImage> images = new();
        private readonly List<ISprite> tiles = new();
        private readonly List<AnimatedSprite> animSprites = new();

        private int maxSizeX;
        private int maxSizeY;
        private byte alpha = 255;
        private double speed = 1.0;

        public TileSet(string name)
            : base(name, ContentFlags.Data)
        {

        }

        public byte Alpha
        {
            get => alpha;
            set
            {
                if (alpha != value)
                {
                    alpha = value;
                    foreach (var image in images)
                    {
                        image.AlphaMod = alpha;
                    }
                }
            }
        }

        public int TileWidth { get; set; } = 64;
        public int TileHeight { get; set; } = 32;
        public int TileCount => tiles.Count;
        public int MaxSizeX => maxSizeX;
        public int MaxSizeY => maxSizeY;
        public ISprite? this[int index] => GetTile(index);
        public ISprite? GetTile(int index)
        {
            if (index >= 0 && index < tiles.Count)
            {
                return tiles[index];
            }
            else
            {
                return null;
            }
        }

        public void Update(double totalTime, double elapsedTime)
        {
            foreach (AnimatedSprite anim in animSprites)
            {
                anim.Update(totalTime, elapsedTime);
            }
        }
        public void AddImage(IImage image)
        {
            images.Add(image);
        }

        public void AddTile(int id, int cx, int cy, int cw, int ch, int ox = 0, int oy = 0)
        {
            AddTile(GetCurrentImage(), id, cx, cy, cw, ch, ox, oy);
        }

        public void AddTile(IImage? image, int id, int cx, int cy, int cw, int ch, int ox = 0, int oy = 0)
        {
            if (image != null)
            {
                AdjustMaxSizeAndTileSize(cw, ch, ox, oy);
                EnsureIndex(id);
                tiles[id] = new Sprite(image, cx, cy, cw, ch, ox, oy);
            }
        }
        public void AddAnimTile(int id, int x, int y, int durationMs)
        {
            ISprite? tile = GetTile(id);
            if (tile != null && tile.Image != null)
            {
                if (tile is AnimatedSprite anim)
                {
                    anim.AddFrame(tile.Image, durationMs * speed, x, y, tile.Width, tile.Height, tile.OffsetX, tile.OffsetY);
                }
                else
                {
                    anim = new AnimatedSprite();
                    animSprites.Add(anim);
                    anim.AddFrame(tile.Image, durationMs * speed, x, y, tile.Width, tile.Height, tile.OffsetX, tile.OffsetY);
                    tiles[id] = anim;
                }
            }
        }
        private IImage? GetCurrentImage()
        {
            if (images.Count > 0)
            {
                return images[^1];
            }
            return null;
        }
        private void AdjustMaxSizeAndTileSize(int width, int height, int offsetX, int offsetY)
        {
            if (TileWidth == 0) TileWidth = width;
            if (TileHeight == 0) TileHeight = height;
            maxSizeX = Math.Max(maxSizeX, width / TileWidth + 1);
            maxSizeY = Math.Max(maxSizeY, height / TileHeight + 1);
        }

        private void EnsureIndex(int index)
        {
            int diff = index + 1 - tiles.Count;
            if (diff > 0)
            {
                tiles.AddRange(new ISprite[diff]);
            }
        }

        protected override void DisposeUnmanaged()
        {
            //foreach (var img in images)
            //{
            //    img.Dispose();
            //}
            images.Clear();
        }
    }
}
