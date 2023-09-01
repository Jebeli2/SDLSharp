namespace SDLSharp.Events
{
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EventInfo
    {
        public string Type { get; set; }
        public EventActivation Activation { get; set; }
        public bool Stash { get; set; }
        public bool Repeat { get; set; }
        public int Cooldown { get; set; }
        public int Delay { get; set; }
        public Rectangle Location { get; set; }
        public Rectangle HotSpot { get; set; }
        public Rectangle ReachableFrom { get; set; }
        public string Msg { get; set; }
        public string Music { get; set; }
        public string ToolTip { get; set; }
        public string Book { get; set; }
        public string SoundFX { get; set; }
        public int SoundX { get; set; }
        public int SoundY { get; set; }
        public string Map { get; set; }
        public int MapX { get; set; }
        public int MapY { get; set; }
        public int TeleportX { get; set; }
        public int TeleportY { get; set; }
        public int ShakyCam { get; set; }
        public IList<MapSpawn> MapSpawns { get; set; }
        public IList<MapMod> MapMods { get; set; }
        public int PowerId { get; set; }
        public IList<string> SetStatus { get; set; }
        public IList<string> UnsetStatus { get; set; }
        public IList<string> RequiresStatus { get; set; }
        public IList<string> RequiresNotStatus { get; set; }
        public float CenterX
        {
            get
            {
                if (!HotSpot.IsEmpty)
                {
                    return HotSpot.X + HotSpot.Width / 2.0f;
                }
                return Location.X + Location.Width / 2.0f;
            }
        }

        public float CenterY
        {
            get
            {
                if (!HotSpot.IsEmpty)
                {
                    return HotSpot.Y + HotSpot.Height / 2.0f;
                }
                return Location.Y + Location.Height / 2.0f;
            }
        }

        public EventInfo()
        {
            Type = "";
            Msg = "";
            Music = "";
            ToolTip = "";
            Book = "";
            SoundFX = "";
            Map = "";
            MapSpawns = new List<MapSpawn>();
            MapMods = new List<MapMod>();
            SetStatus = new List<string>();
            UnsetStatus = new List<string>();
            RequiresStatus = new List<string>();
            RequiresNotStatus = new List<string>();
            Repeat = true;
            MapX = -1;
            MapY = -1;
            TeleportX = -1;
            TeleportY = -1;
            SoundX = -1;
            SoundY = -1;
        }
    }
}
