namespace SDLSharp.Content.Flare
{
    using SDLSharp.Actors;
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ModActorLoader : ModLoader, IResourceLoader<Actor>
    {
        public ModActorLoader()
            : base("Flare Mod Actor Loader")
        {

        }

        public Actor? Load(string name, byte[]? data)
        {
            using FileParser infile = new FileParser(ContentManager, name, data);
            Actor actor = new Actor(name);
            Actor? result = LoadActor(infile, name, actor);
            if (result != null)
            {
                SDLLog.Info(LogCategory.APPLICATION, $"Actor loaded from resource '{name}'");
            }
            return result;
        }

        private Actor? LoadActor(FileParser infile, string name, Actor actor)
        {
            //List<string> categories = new();
            float speed = 3.5f;
            //float meleeRange = 1.0f;
            //float threatRange = 4.0f;
            //float threatRangeFar = 8.0f;
            //string? defeatStatus = null;
            IDictionary<string, string> animationParts = new Dictionary<string, string>();
            IDictionary<int, IList<string>> layerOrder = new Dictionary<int, IList<string>>();
            while (infile.Next())
            {
                switch (infile.Section)
                {
                    case "":
                        switch (infile.Key)
                        {
                            case "name": actor.DisplayName = infile.GetStrVal(); break;
                            case "speed": speed = infile.GetFloatVal(); break;
                            case "turn_delay": actor.TurnDelay = FileParser.ParseDurationMS(infile.GetStrVal()); break;
                            case "waypoint_pause": actor.WaypointPause = FileParser.ParseDurationMS(infile.GetStrVal()); break;
                            case "vox_intro": actor.VoxIntros.Add(infile.GetStrVal()); break;
                            case "sfx_attack": break;
                            case "sfx_hit": break;
                            case "sfx_die": break;
                            case "sfx_critdie": break;
                            case "sfx_block": break;
                            case "defeat_status": break;
                            case "level": break;
                            case "melee_range": break;
                            case "threat_range": break;
                            case "categories": break;
                            case "animations": animationParts[""] = infile.GetStrVal(); break;
                            case "gfx": animationParts[""] = infile.GetStrVal(); break;
                            case "gfxpart":
                                string part = infile.PopFirstString();
                                string anim = infile.PopFirstString();
                                animationParts[part] = anim;
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
                                layerOrder[layer] = order;
                                break;
                        }
                        break;
                }
            }
            if (actor.LoadAnimations(ContentManager, animationParts, layerOrder))
            {
                actor.DefaultSpeed = speed;
                actor.Speed = speed;
                //actor.DisplayName = actorName;
                //actor.TurnDelay = turnDelay;
                //actor.WaypointPause = waypointPause;
                //actor.DefaultSpeed = speed;
                //actor.Speed = speed;

                return actor;
            }
            return null;
        }

    }
}
