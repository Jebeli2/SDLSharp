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
            using FileParser infile = new FileParser(name, data);
            Actor? result = LoadActor(infile, name);
            if (result != null)
            {
                SDLLog.Info(LogCategory.APPLICATION, $"Actor loaded from resource '{name}'");
            }
            return result;
        }

        private Actor? LoadActor(FileParser infile, string name)
        {
            Actor? actor = null;
            string actorName = "";
            List<string> categories = new();
            int turnDelay = 0;
            int waypointPause = 60;
            float speed = 3.5f;
            float meleeRange = 1.0f;
            float threatRange = 4.0f;
            float threatRangeFar = 8.0f;
            string? defeatStatus = null;
            IDictionary<string, string> animationParts = new Dictionary<string, string>();
            IDictionary<int, IList<string>> layerOrder = new Dictionary<int, IList<string>>();
            IList<string> voxIntros = new List<string>();
            while (infile.Next())
            {
                switch (infile.Section)
                {
                    case "":
                        switch (infile.Key)
                        {
                            case "name": actorName = infile.GetStrVal(); break;
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
            if (animationParts.Count > 0)
            {
                actor = new Actor(name);
                actor.DisplayName = actorName;
                if (animationParts.Count > 1 && layerOrder.Count >= animationParts.Count)
                {
                    var animSets = new Dictionary<string, AnimationSet>();
                    foreach (var kvp in animationParts)
                    {
                        AnimationSet? animSet = ContentManager?.Load<AnimationSet>(kvp.Value);
                        if (animSet != null)
                        {
                            animSets[kvp.Key] = animSet;
                        }
                    }
                    actor.Visual = new MultiPartVisual(animSets, layerOrder);
                }
                else
                {
                    foreach (var kvp in animationParts)
                    {
                        AnimationSet? animSet = ContentManager?.Load<AnimationSet>(kvp.Value);
                        if (animSet != null)
                        {
                            actor.Visual = new AnimationSetVisual(animSet);
                            break;
                        }
                    }
                }
            }
            return actor;
        }
    }
}
