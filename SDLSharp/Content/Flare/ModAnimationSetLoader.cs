namespace SDLSharp.Content.Flare
{
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ModAnimationSetLoader : ModLoader, IResourceLoader<AnimationSet>
    {
        public ModAnimationSetLoader()
            : base("Flare Mod AnimationSet Loader")
        {
        }

        public AnimationSet? Load(string name, byte[]? data)
        {
            using FileParser infile = new FileParser(ContentManager, name, data);
            AnimationSet? result = LoadAnimationSet(infile, name);
            if (result != null)
            {
                SDLLog.Info(LogCategory.APPLICATION, $"AnimationSet loaded from resource '{name}'");
            }
            return result;
        }

        private AnimationSet? LoadAnimationSet(FileParser infile, string name)
        {
            AnimationSet? anims = null;
            Animation? anim = null;
            IImage? image = null;
            string animationName = "";
            int renderSizeX = 0;
            int renderSizeY = 0;
            int renderOffsetX = 0;
            int renderOffsetY = 0;
            int position = 0;
            int frames = 0;
            int duration = 0;
            int index = 0;
            int direction = 0;
            int renderX = 0;
            int renderY = 0;
            int renderWidth = 0;
            int renderHeight = 0;
            List<int> activeFrames = new List<int>();

            AnimationType animationType = AnimationType.None;

            while (infile.Next())
            {
                if (infile.NewSection)
                {
                    if (anims == null && image != null)
                    {
                        anims = new AnimationSet(name);
                    }
                }

                if (infile.MatchSectionKey("", "image"))
                {
                    image = ContentManager?.Load<SDLTexture>(infile.GetStrVal());
                }
                else if (infile.MatchSectionKey("", "render_size"))
                {
                    renderSizeX = infile.PopFirstInt();
                    renderSizeY = infile.PopFirstInt();
                }
                else if (infile.MatchSectionKey("", "render_offset"))
                {
                    renderOffsetX = infile.PopFirstInt();
                    renderOffsetY = infile.PopFirstInt();
                }
                else if (infile.MatchSectionKey("", "blend_mode"))
                {
                    //blendMode = (BlendMode)infile.GetIntVal();
                }
                else if (infile.MatchSectionKey("", "alpha_mod"))
                {
                    //alphaMod = (byte)infile.GetIntVal();
                }
                else if (infile.MatchSectionKey("", "color_mod"))
                {
                    //colorMod = infile.GetColor();
                }
                else if (!infile.MatchSection(""))
                {
                    if (infile.NewSection)
                    {
                        if (anim != null)
                        {
                            anim.SetActiveFrames(activeFrames);
                            anims?.AddAnimation(anim);
                        }
                        animationName = infile.Section;
                        anim = null;
                    }
                    if (infile.MatchKey("position"))
                    {
                        position = infile.PopFirstInt();
                    }
                    else if (infile.MatchKey("frames"))
                    {
                        frames = infile.PopFirstInt();
                    }
                    else if (infile.MatchKey("duration"))
                    {
                        duration = FileParser.ParseDurationMS(infile.GetStrVal());
                    }
                    else if (infile.MatchKey("type"))
                    {
                        animationType = StringToAnimationType(infile.PopFirstString());
                    }
                    else if (infile.MatchKey("active_frame"))
                    {
                        activeFrames.Clear();
                        string nv = infile.PopFirstString();
                        if ("all".Equals(nv, StringComparison.OrdinalIgnoreCase))
                        {
                            activeFrames.Add(-1);
                        }
                        else
                        {
                            while (!string.IsNullOrEmpty(nv))
                            {
                                if (int.TryParse(nv, out var af))
                                {
                                    activeFrames.Add(af);
                                }
                                nv = infile.PopFirstString();
                            }
                        }
                        activeFrames.Sort();

                    }
                    else if (infile.MatchKey("frame"))
                    {
                        if (anim == null)
                        {
                            anim = new Animation(animationName, animationType);
                        }
                        if (image != null)
                        {
                            index = infile.PopFirstInt();
                            direction = FileParser.ParseDirection(infile.PopFirstString());
                            renderX = infile.PopFirstInt();
                            renderY = infile.PopFirstInt();
                            renderWidth = infile.PopFirstInt();
                            renderHeight = infile.PopFirstInt();
                            renderOffsetX = infile.PopFirstInt();
                            renderOffsetY = infile.PopFirstInt();
                            anim.AnimatedSprites[direction].AddFrame(image, duration / (double)frames, renderX, renderY, renderWidth, renderHeight, renderOffsetX, renderOffsetY);
                            if (anim.AnimatedSprites[direction].FrameCount != index + 1)
                            {
                                SDLLog.Error(LogCategory.APPLICATION, $"Frame Index Mismatch {index} in file '{name}'");
                            }
                        }
                        else
                        {
                            SDLLog.Error(LogCategory.APPLICATION, $"No Image for Animation in file '{name}'");
                        }
                    }
                }
            }

            if (anims != null)
            {
                if (anim != null)
                {
                    anims.AddAnimation(anim);
                }
                else if (image != null)
                {
                    anim = new Animation(animationName, animationType);
                    int maxKinds = 8;
                    for (int i = 0; i < frames; i++)
                    {
                        int baseIndex = maxKinds * i;
                        for (int kind = 0; kind < maxKinds; kind++)
                        {
                            anim.AnimatedSprites[kind].AddFrame(image, duration / (double)frames, renderSizeX * (position + i), renderSizeY * kind, renderSizeX, renderSizeY, renderOffsetX, renderOffsetY);
                        }
                    }
                    anims.AddAnimation(anim);

                }
            }

            return anims;
        }

        private static AnimationType StringToAnimationType(string value)
        {
            return value switch
            {
                "play_once" => AnimationType.PlayOnce,
                "looped" => AnimationType.Looped,
                "back_forth" => AnimationType.BackForth,
                _ => AnimationType.None,
            };
        }
    }
}
