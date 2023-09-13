namespace SDLSharp.Content.Flare
{
    using SDLSharp.Graphics;
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ModTileSetLoader : ModLoader, IResourceLoader<TileSet>
    {

        public ModTileSetLoader()
            : base("Flare Mod TileSet Loader")
        {

        }

        public TileSet? Load(string name, byte[]? data)
        {
            using FileParser infile = new FileParser(ContentManager, name, data);
            TileSet? result = LoadTileSet(infile, name);
            if (result != null)
            {
                SDLLog.Info(LogCategory.APPLICATION, $"TileSet loaded from resource '{name}'");
            }
            return result;
        }

        private TileSet LoadTileSet(FileParser infile, string name)
        {
            TileSet tileSet = new TileSet(name);
            string imgName;
            IImage? img;
            int cx;
            int cy;
            int cw;
            int ch;
            int ox;
            int oy;
            int id;
            while (infile.Next())
            {
                switch (infile.Section)
                {
                    case "tileset":
                    case "":
                        switch (infile.Key)
                        {
                            case "img":
                                imgName = infile.GetStrVal();
                                img = ContentManager?.Load<SDLTexture>(imgName);
                                if (img != null) { tileSet.AddImage(img); }
                                break;
                            case "tile":
                                id = infile.PopFirstInt();
                                cx = infile.PopFirstInt();
                                cy = infile.PopFirstInt();
                                cw = infile.PopFirstInt();
                                ch = infile.PopFirstInt();
                                ox = infile.PopFirstInt();
                                oy = infile.PopFirstInt();
                                tileSet.AddTile(id, cx, cy, cw, ch, ox, oy);
                                break;
                            case "animation":
                                id = infile.PopFirstInt();
                                string repeatVal = infile.PopFirstString(';');
                                while (!string.IsNullOrEmpty(repeatVal))
                                {
                                    int posX = FileParser.PopFirstInt(ref repeatVal);
                                    int posY = FileParser.PopFirstInt(ref repeatVal);
                                    int duration = FileParser.ParseDurationMS(repeatVal);
                                    tileSet.AddAnimTile(id, posX, posY, duration);                                    
                                    repeatVal = infile.PopFirstString(';');
                                }
                                break;
                            default: SDLLog.Warn(LogCategory.APPLICATION, $"Unknown entry in {name}: {infile.Section}-{infile.Key} = {infile.Val}"); break;
                        }
                        break;
                }
            }
            return tileSet;
        }
    }
}
