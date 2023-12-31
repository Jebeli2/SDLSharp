﻿namespace SDLSharp.Content.Flare
{
    using SDLSharp.Actors;
    using SDLSharp.Events;
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ModMapLoader : ModLoader, IResourceLoader<Map>
    {
        public ModMapLoader()
            : base("Flare Mod Map Loader")
        {

        }

        public Map? Load(string name, byte[]? data)
        {
            using FileParser infile = new FileParser(ContentManager, name, data);
            Map? result = LoadMap(infile, name);
            if (result != null)
            {
                result.InitCollision();
                SDLLog.Info(LogCategory.APPLICATION, $"Map loaded from resource '{name}'");
            }
            return result;
        }

        private Map LoadMap(FileParser infile, string name)
        {
            Map? map = null;
            MapLayer? layer = null;
            int width = 0;
            int height = 0;
            int tileWidth = 0;
            int tileHeight = 0;
            string title = "";
            string tileSetName = "";
            string collisionTileSetName = "tileset_collision";
            string parallaxName = "";
            string musicName = "";
            string layerType = "";
            int heroPosX = -1;
            int heroPosY = -1;
            MapProjection projection = MapProjection.Isometric;
            Color bgColor = Color.Black;
            bool initDone = false;
            ActorInfo? npc = null;
            EventInfo? evt = null;
            EnemyInfo? enemy = null;
            while (infile.Next())
            {
                if (!initDone && infile.MatchDifferentSection("header"))
                {
                    map = new Map(name, width, height);
                    map.Projection = projection;
                    map.Title = title;
                    map.Music = musicName;
                    map.BackgroundColor = bgColor;
                    map.StartPosX = heroPosX;
                    map.StartPosY = heroPosY;
                    if (!string.IsNullOrEmpty(tileSetName)) { map.TileSet = ContentManager?.Load<TileSet>(tileSetName); }
                    if (!string.IsNullOrEmpty(collisionTileSetName)) { map.CollisionTileSet = ContentManager?.Load<TileSet>(collisionTileSetName); }
                    if (!string.IsNullOrEmpty(parallaxName)) { map.Parallax = ContentManager?.Load<MapParallax>(parallaxName); }
                    initDone = true;
                }
                if (map != null && infile.MatchNewSection("npc")) { npc = new ActorInfo(); map.AddActorInfo(npc); }
                if (map != null && infile.MatchNewSection("event")) { evt = new EventInfo(); map.AddEventInfo(evt); }
                if (map != null && infile.MatchNewSection("enemy")) { enemy = new EnemyInfo(); map.AddEnemyInfo(enemy); }
                if (map != null && infile.MatchNewSection("enemygroup")) { enemy = new EnemyInfo(); map.AddEnemyInfo(enemy); }
                switch (infile.Section)
                {
                    case "header":
                        switch (infile.Key)
                        {
                            case "width": width = infile.GetIntVal(1); break;
                            case "height": height = infile.GetIntVal(1); break;
                            case "tilewidth": tileWidth = infile.GetIntVal(1); break;
                            case "tileheight": tileHeight = infile.GetIntVal(1); break;
                            case "title": title = infile.GetStrVal(); break;
                            case "tileset": tileSetName = infile.GetStrVal(); break;
                            case "music": musicName = infile.GetStrVal(); break;
                            case "parallax_layers": parallaxName = infile.GetStrVal(); break;
                            case "hero_pos":
                                heroPosX = infile.PopFirstInt();
                                heroPosY = infile.PopFirstInt();
                                break;
                            case "orientation": projection = infile.GetEnumValue(MapProjection.Isometric); break;
                            case "background_color": bgColor = infile.GetColorRGBAValue(); break;
                            default: UnknownKey<Map>(name, infile); break;
                        }
                        break;
                    case "layer":
                        switch (infile.Key)
                        {
                            case "type":
                                layerType = infile.GetStrVal();
                                layer = new MapLayer(layerType, width, height, tileWidth, tileHeight);
                                switch (layerType)
                                {
                                    case "object": layer.Type = LayerType.Object; break;
                                    case "collision": layer.Type = LayerType.Collision; break;
                                    case "background": layer.Type = LayerType.Background; break;
                                    default: layer.Type = LayerType.Unknown; break;
                                }
                                map?.AddLayer(layer);
                                break;
                            case "data":
                                if (layer != null)
                                {
                                    for (int j = 0; j < layer.Height; j++)
                                    {
                                        string val = infile.GetRawLine();
                                        infile.IncrementLineNum();
                                        if (!string.IsNullOrEmpty(val) && !val.EndsWith(","))
                                        {
                                            val += ",";
                                        }
                                        int commaCount = 0;
                                        for (int i = 0; i < val.Length; i++)
                                        {
                                            if (val[i] == ',')
                                            {
                                                commaCount++;
                                            }
                                        }
                                        if (commaCount == layer.Width)
                                        {
                                            for (int i = 0; i < layer.Width; i++)
                                            {
                                                layer[i, j] = FileParser.PopFirstInt(ref val);
                                            }
                                        }
                                    }
                                }
                                break;
                            default: UnknownKey<Map>(name, infile); break;
                        }
                        break;
                    case "npc":
                        if (npc != null)
                        {
                            switch (infile.Key)
                            {
                                case "type": break;
                                case "location": npc.PosX = infile.PopFirstInt(); npc.PosY = infile.PopFirstInt(); break;
                                case "filename": npc.Id = infile.GetStrVal(); break;
                                default: UnknownKey<Map>(name, infile); break;
                            }
                        }
                        break;
                    case "enemy":
                    case "enemygroup":
                        if (enemy != null)
                        {
                            switch (infile.Key)
                            {
                                case "type": enemy.Type = infile.GetStrVal(); break;
                                case "category": enemy.Category = infile.GetStrVal(); break;
                                case "chance": enemy.Chance = infile.GetIntVal(); break;
                                case "location":
                                    enemy.PosX = infile.PopFirstInt();
                                    enemy.PosY = infile.PopFirstInt();
                                    enemy.Width = infile.PopFirstInt();
                                    enemy.Height = infile.PopFirstInt();
                                    break;
                                case "level":
                                    enemy.MinLevel = Math.Max(0, infile.PopFirstInt());
                                    enemy.MaxLevel = Math.Max(enemy.MinLevel, infile.GetIntVal(enemy.MinLevel));
                                    break;
                                case "number":
                                    enemy.MinNumber = Math.Max(0, infile.PopFirstInt());
                                    enemy.MaxNumber = Math.Max(enemy.MinNumber, infile.GetIntVal(enemy.MinNumber));
                                    break;
                                case "direction":
                                    enemy.Direction = FileParser.ParseDirection(infile.GetStrVal());
                                    break;
                                case "waypoints":
                                    break;
                                case "wander_radius":
                                    enemy.WanderRadius = Math.Max(0, infile.GetIntVal());
                                    break;
                                default: UnknownKey<Map>(name, infile); break;
                            }
                        }
                        break;
                    case "event":
                        if (evt != null)
                        {
                            switch (infile.Key)
                            {
                                case "type": evt.Type = infile.GetStrVal(); break;
                                case "activate": evt.Activation = StringToEventActivation(infile.GetStrVal()); break;
                                case "cooldown": evt.Cooldown = FileParser.ParseDurationMS(infile.GetStrVal()); break;
                                case "delay": evt.Delay = FileParser.ParseDurationMS(infile.GetStrVal()); break;
                                case "location": evt.Location = infile.PopFirstRect(); break;
                                case "hotspot":
                                    if (infile.GetStrVal() == "location")
                                    {
                                        evt.HotSpot = evt.Location;
                                    }
                                    else
                                    {
                                        evt.HotSpot = infile.PopFirstRect();
                                    }
                                    break;
                                case "reachable_from": evt.ReachableFrom = infile.PopFirstRect(); break;
                                case "msg": evt.Msg = infile.GetStrVal(); break;
                                case "music": evt.Music = infile.GetStrVal(); break;
                                case "tooltip": evt.ToolTip = infile.GetStrVal(); break;
                                case "book": evt.Book = infile.GetStrVal(); break;
                                case "repeat": evt.Repeat = infile.GetBoolVal(); break;
                                case "stash": evt.Stash = infile.GetBoolVal(); break;
                                case "intermap":
                                    evt.Map = infile.PopFirstString();
                                    evt.MapX = infile.PopFirstInt();
                                    evt.MapY = infile.PopFirstInt();
                                    break;
                                case "intramap":
                                    evt.TeleportX = infile.PopFirstInt();
                                    evt.TeleportY = infile.PopFirstInt();
                                    break;
                                case "shakycam": evt.ShakyCam = FileParser.ParseDurationMS(infile.GetStrVal()); break;
                                case "power": evt.PowerId = infile.PopFirstInt(); break;
                                case "power_path": break;
                                case "power_damage": break;
                                case "spawn": evt.MapSpawns = ParseMapSpawns(infile.GetStrVal()); break;
                                case "mapmod": evt.MapMods = ParseMapMods(infile.GetStrVal()); break;
                                case "soundfx":
                                    evt.SoundFX = infile.PopFirstString();
                                    evt.SoundX = (int)evt.CenterX;
                                    evt.SoundY = (int)evt.CenterY;
                                    if (!string.IsNullOrEmpty(infile.Val)) { evt.SoundX = infile.PopFirstInt(); }
                                    if (!string.IsNullOrEmpty(infile.Val)) { evt.SoundY = infile.PopFirstInt(); }
                                    break;
                                case "requires_level": break;
                                case "requires_status": evt.RequiresStatus = infile.GetStrValues(); break;
                                case "requires_not_status": evt.RequiresNotStatus = infile.GetStrValues(); break;
                                case "requires_item": break;
                                case "requires_not_item": break;
                                case "remove_item": break;
                                case "set_status": evt.SetStatus = infile.GetStrValues(); break;
                                case "unset_status": evt.UnsetStatus = infile.GetStrValues(); break;
                                case "loot": break;
                                case "loot_count": break;
                                case "reward_xp": break;
                                case "cutscene": break;
                                default: UnknownKey<Map>(name, infile); break;
                            }
                        }
                        break;
                }
            }
            return map ?? new Map(name, width, height);
        }

        private static List<MapSpawn> ParseMapSpawns(string value)
        {
            var list = new List<MapSpawn>();
            foreach (string s in value.Split(';'))
            {
                var spawn = ParseMapSpawn(s);
                if (spawn != null)
                {
                    list.Add(spawn);
                }

            }
            return list;
        }

        private static MapSpawn ParseMapSpawn(string value)
        {
            string type = FileParser.PopFirstString(ref value);
            int x = FileParser.PopFirstInt(ref value);
            int y = FileParser.PopFirstInt(ref value);
            return new MapSpawn { Type = type, MapX = x, MapY = y };
        }
        private static List<MapMod> ParseMapMods(string value)
        {
            var list = new List<MapMod>();
            foreach (string s in value.Split(';'))
            {
                var mod = ParseMapMod(s);
                if (mod != null)
                {
                    list.Add(mod);
                }
            }
            return list;
        }

        private static MapMod ParseMapMod(string value)
        {
            string name = FileParser.PopFirstString(ref value);
            int x = FileParser.PopFirstInt(ref value);
            int y = FileParser.PopFirstInt(ref value);
            int v = FileParser.PopFirstInt(ref value);
            return new MapMod { Layer = name, MapX = x, MapY = y, MapValue = v };
        }
        private static EventActivation StringToEventActivation(string str)
        {
            return str switch
            {
                "on_trigger" => EventActivation.Trigger,
                "on_load" => EventActivation.Load,
                "on_mapexit" => EventActivation.Exit,
                "on_leave" => EventActivation.Leave,
                "on_clear" => EventActivation.Clear,
                "static" => EventActivation.Static,
                _ => EventActivation.None,
            };
        }
    }
}
