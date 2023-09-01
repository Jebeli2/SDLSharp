namespace SDLSharp.Events
{
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EventComponent
    {
        private readonly List<string> stringParams = new List<string>();

        public EventComponent()
        {
            StringParam = "";
            MapMods = new List<MapMod>();
            MapSpawns = new List<MapSpawn>();
        }
        public EventComponentType Type { get; set; }
        public string StringParam { get; set; }
        public int IntParam { get; set; }
        public bool BoolParam
        {
            get { return IntParam != 0; }
            set { if (value) IntParam = 1; else IntParam = 0; }
        }

        public IList<string> StringParams
        {
            get { return stringParams; }
            set
            {
                stringParams.Clear();
                if (value != null)
                {
                    stringParams.AddRange(value);
                }
            }
        }
        public int MapX { get; set; }
        public int MapY { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public IList<MapMod> MapMods { get; set; }
        public IList<MapSpawn> MapSpawns { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Type);
            sb.Append(':');
            switch (Type)
            {
                case EventComponentType.InterMap:
                    sb.Append(StringParam);
                    sb.Append(" (");
                    sb.Append(MapX);
                    sb.Append('/');
                    sb.Append(MapY);
                    sb.Append(')');
                    break;
                case EventComponentType.IntraMap:
                    sb.Append('(');
                    sb.Append(MapX);
                    sb.Append('/');
                    sb.Append(MapY);
                    sb.Append(')');
                    break;
                case EventComponentType.MapMod:
                    sb.Append(MapMods.Count);
                    sb.Append(" tile");
                    if (MapMods.Count > 1)
                    {
                        sb.Append('s');
                    }
                    break;
                case EventComponentType.Spawn:
                    sb.Append(MapSpawns.Count);
                    sb.Append(" spwan");
                    if (MapSpawns.Count > 1)
                    {
                        sb.Append('s');
                    }
                    sb.Append(" (");
                    foreach (var spawn in MapSpawns)
                    {
                        sb.Append(spawn.Type);
                        sb.Append(", ");
                    }
                    if (MapSpawns.Count > 0)
                    {
                        sb.Length -= 2;
                    }
                    sb.Append(')');
                    break;
                case EventComponentType.ShakyCam:
                case EventComponentType.Power:
                    sb.Append(IntParam);
                    break;
                case EventComponentType.Repeat:
                case EventComponentType.Stash:
                    sb.Append(BoolParam ? "true" : "false");
                    break;
                case EventComponentType.Msg:
                case EventComponentType.SoundFX:
                case EventComponentType.Tooltip:
                default:
                    if (StringParams != null && StringParams.Count > 0)
                    {
                        sb.Append(string.Join(",", StringParams));
                    }
                    else
                    {
                        sb.Append(StringParam);

                    }
                    break;
            }
            return sb.ToString();
        }
    }
}
