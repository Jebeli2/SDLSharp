namespace SDLSharp.Content.Flare
{
    using SDLSharp.Actors;
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ModEnemyTemplateLoader : ModLoader, IResourceLoader<EnemyTemplate>
    {

        public ModEnemyTemplateLoader()
            : base("Flare Mode Enemy Template Loader")
        {

        }

        public EnemyTemplate? Load(string name, byte[]? data)
        {
            using FileParser infile = new FileParser(ContentManager, name, data);
            EnemyTemplate enemyTemplate = new EnemyTemplate(name);
            return LoadEnemyTemplate(infile, name, enemyTemplate);
        }

        private EnemyTemplate? LoadEnemyTemplate(FileParser infile, string name, EnemyTemplate et)
        {
            while (infile.Next())
            {
                switch (infile.Section)
                {
                    case "":
                        switch (infile.Key)
                        {
                            case "name": et.DisplayName = infile.GetStrVal(); break;
                            case "categories": et.Categories = infile.GetStrValues(); break;
                            case "rarity": et.Rarity = infile.GetStrVal(); break;
                            case "level": et.Level = infile.GetIntVal(); break;
                            case "animations": et.AnimationParts[""] = infile.GetStrVal(); break;
                            case "gfx": et.AnimationParts[""] = infile.GetStrVal(); break;
                            case "gfxpart":
                                string part = infile.PopFirstString();
                                string anim = infile.PopFirstString();
                                et.AnimationParts[part] = anim;
                                break;
                            case "layer":
                                int layer = infile.PopFirstInt();
                                List<string> order = new List<string>();
                                string orderPart = infile.PopFirstString();
                                while (!string.IsNullOrEmpty(orderPart))
                                {
                                    order.Add(orderPart);
                                    orderPart = infile.PopFirstString();
                                }
                                et.LayerOrder[layer] = order;
                                break;

                        }
                        break;
                }
            }
            if (et.AnimationParts.Count > 0)
            {
                return et;
            }
            return null;
        }
    }
}
