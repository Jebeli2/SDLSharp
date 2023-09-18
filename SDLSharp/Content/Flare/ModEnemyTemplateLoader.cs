namespace SDLSharp.Content.Flare;

using SDLSharp.Actors;
using System.Collections.Generic;

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

    private static EnemyTemplate? LoadEnemyTemplate(FileParser infile, string name, EnemyTemplate et)
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
                        case "speed": break;
                        case "turn_delay": break;
                        case "waypoint_pause": break;
                        case "vox_intro": break;
                        case "sfx_attack": break;
                        case "sfx_hit": break;
                        case "sfx_die": break;
                        case "sfx_critdie": break;
                        case "sfx_block": break;
                        case "defeat_status": break;
                        case "melee_range": break;
                        case "threat_range": break;
                        case "suppress_hp": break;
                        case "facing": break;
                        case "lifeform": et.LifeForm = infile.GetBoolVal(); break;
                        case "combat_style": break;
                        case "power": break;
                        case "passive_powers": break;
                        case "power_filter": break;
                        case "stat": break;
                        case "stat_per_level": break;
                        case "chance_pursue": break;
                        case "chance_flee": break;
                        case "flee_duration": break;
                        case "flee_cooldown": break;
                        case "flee_range": break;
                        case "cooldown": break;
                        case "cooldown_hit": break;
                        case "flying": break;
                        case "humanoid": break;
                        case "xp": break;
                        case "loot": break;
                        case "loot_count": break;
                        case "quest_loot": break;
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
                        default: UnknownKey<EnemyTemplate>(name, infile); break;

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
