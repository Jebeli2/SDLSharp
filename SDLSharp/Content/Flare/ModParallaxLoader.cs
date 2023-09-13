namespace SDLSharp.Content.Flare
{
    using SDLSharp.Graphics;
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ModParallaxLoader : ModLoader, IResourceLoader<MapParallax>
    {
        public ModParallaxLoader()
           : base("Flare Mod Parallax Loader")
        {

        }

        public MapParallax? Load(string name, byte[]? data)
        {
            using FileParser infile = new FileParser(ContentManager, name, data);
            MapParallax? result = LoadParallax(infile, name);
            if (result != null)
            {
                SDLLog.Info(LogCategory.APPLICATION, $"MapParallax loaded from resource '{name}'");
            }
            return result;
        }

        private MapParallax LoadParallax(FileParser infile, string name)
        {
            MapParallax parallax = new MapParallax(name);
            ParallaxLayer? layer = null;
            while (infile.Next())
            {
                if (infile.MatchNewSection("layer")) { layer = parallax.AddLayer(); }
                if (layer != null && infile.MatchSectionKey("layer", "image")) { layer.Image = ContentManager?.Load<SDLTexture>(infile.GetStrVal()); }
                else if (layer != null && infile.MatchSectionKey("layer", "speed")) { layer.Speed = infile.GetFloatVal(); }
                else if (layer != null && infile.MatchSectionKey("layer", "fixed_speed"))
                {
                    layer.FixedSpeedX = infile.PopFirstFloat();
                    layer.FixedSpeedY = infile.PopFirstFloat();
                }
                else if (layer != null && infile.MatchSectionKey("layer", "map_layer")) { layer.MapLayer = infile.Val; }
            }
            return parallax;
        }
    }
}
