namespace SDLSharp.Events
{
    using SDLSharp.Actors;
    using SDLSharp.Maps;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EventManager : IEventManager
    {
        private readonly List<Event> events = new();
        private readonly List<Event> delayedEvents = new();
        private double shakyCamDuration;
        private float tooltipMapX;
        private float tooltipMapY;
        private int tooltipOffsetX;
        private int tooltipOffsetY;
        private string travelMap = "";
        private int travelX = -1;
        private int travelY = -1;
        private bool travel = false;
        private readonly IMapEngine engine;

        internal EventManager(IMapEngine engine)
        {
            this.engine = engine;
            InteractRange = 2.0f;
        }
        //public Map? Map
        //{
        //    get { return map; }
        //    set { SetMap(value); }
        //}
        //public IMapCamera? Camera
        //{
        //    get => camera;
        //    set => camera = value;
        //}

        //public Actor? Player
        //{
        //    get => player;
        //    set => player = value;
        //}
        public float InteractRange { get; set; }

        //public TooltipData TooltipData => tooltipData;
        public float TooltipMapX => tooltipMapX;
        public float TooltipMapY => tooltipMapY;
        public int TooltipOffsetX => tooltipOffsetX;
        public int TooltipOffsetY => tooltipOffsetY;

        public string TravelMap => travelMap;
        public int TravelX => travelX;
        public int TravelY => travelY;
        public bool Travel
        {
            get => travel;
            set => travel = false;
        }

        public void Clear()
        {
            events.Clear();
            delayedEvents.Clear();            
        }

        public void AddEvent(Event evt)
        {
            events.Add(evt);
        }

        public void Update(double totalTime, double elapsedTime)
        {
            UpdateEvents(elapsedTime);
            CheckDelayedEvents();
            CheckStaticEvents();
        }

        private void UpdateEvents(double ms)
        {
            int i = events.Count;
            while (i > 0)
            {
                i--;
                Event evt = events[i];
                evt.Update(ms);
                if (evt.RemoveNow) { events.RemoveAt(i); }
            }
        }

        private void CheckDelayedEvents()
        {
            int i = delayedEvents.Count;
            while (i > 0)
            {
                i--;
                Event evt = delayedEvents[i];
                if (!evt.IsInDelay)
                {
                    delayedEvents.RemoveAt(i);
                    ExecuteDelayedEvent(evt);
                }
            }
        }

        private void CheckStaticEvents()
        {
            foreach (var evt in events)
            {
                if (evt.Activation == EventActivation.Static && IsActive(evt))
                {
                    ExecuteEvent(evt);
                }
            }
        }

        public void CheckClickEvents(float posX, float posY, float mapX, float mapY)
        {
            float distance = MathUtils.Distance(posX, posY, mapX, mapY);
            if (distance < InteractRange)
            {
                foreach (Event evt in events.Where(x => IsActive(x) &&
                                                  (x.Activation == EventActivation.Trigger || x.Activation == EventActivation.None) &&
                                                   x.CheckHotSpot(mapX, mapY)).ToList())
                {
                    ExecuteEvent(evt);
                }
            }
        }

        public void CheckPositionEvents(float mapX, float mapY, IList<PointF>? path = null)
        {
            foreach (Event evt in events.Where(x => IsActive(x) &&
                                              (x.Activation == EventActivation.Trigger || x.Activation == EventActivation.None) &&
                                               x.CheckHit(mapX, mapY, path)).ToList())
            {
                ExecuteEvent(evt);
            }
        }

        public void CheckHotSpotEvents(float mapX, float mapY)
        {
            //tooltipData = new TooltipData();
            tooltipOffsetX = 0;
            tooltipOffsetY = 0;
            foreach (var evt in events)
            {
                Rectangle hotSpot = evt.HotSpot;
                if (hotSpot.IsEmpty) continue;
                if (!IsActive(evt)) continue;
                if (hotSpot.Contains((int)mapX, (int)mapY))
                {
                    EventComponent? tt = evt.Components.FirstOrDefault(x => x.Type == EventComponentType.Tooltip);
                    if (tt != null)
                    {
                        tooltipMapX = hotSpot.X;
                        tooltipMapY = hotSpot.Y;
                        //tooltipData.SetText(tt.StringParam);
                    }
                    EventComponent? npcHS = evt.Components.FirstOrDefault(x => x.Type == EventComponentType.NPCHotspot);
                    if (npcHS != null)
                    {
                        tooltipOffsetY = -32;
                    }
                    return;
                }
            }
        }

        public bool HasAnyEventsAt(float mapX, float mapY)
        {
            return events.Where(x => IsActive(x) &&
                (x.Activation == EventActivation.Trigger || x.Activation == EventActivation.None) &&
                x.CheckHotSpot(mapX, mapY)).Any();
        }

        private bool IsActive(Event evt)
        {
            if (evt != null)
            {
                if (evt.IsInCooldown) return false;
                if (evt.IsInDelay) return false;
                //if (!campaignManager.CheckAllRequirements(evt)) return false;
                return true;
            }
            return false;
        }

        public void ExecuteOnLoadEvents()
        {
            foreach (var evt in events)
            {
                if (evt.Activation == EventActivation.Load && IsActive(evt))
                {
                    evt.Repeat = false;
                    ExecuteEvent(evt);
                }
            }
        }

        public void ExecuteOnExitEvents()
        {
            foreach (var evt in events)
            {
                if (evt.Activation == EventActivation.Exit && IsActive(evt))
                {
                    evt.Repeat = false;
                    ExecuteEvent(evt);
                }
            }
        }

        public void ExecuteEvent(Event evt)
        {
            if (IsActive(evt))
            {
                if (evt.Delay > 0)
                {
                    evt.StartDelay();
                    delayedEvents.Add(evt);
                }
                else
                {
                    ExecuteImmediateEvent(evt);
                }
            }
        }

        private void ExecuteImmediateEvent(Event evt)
        {
            SDLLog.Info(LogCategory.APPLICATION, $"Executing Immediate Event {evt}");
            if (!evt.Repeat) evt.RemoveNow = true;
            evt.StartCooldown();
            foreach (var ec in evt.Components)
            {
                ExecuteEventComponent(evt, ec);
            }
        }

        private void ExecuteDelayedEvent(Event evt)
        {
            SDLLog.Info(LogCategory.APPLICATION, $"Executing Delayed Event {evt}");
            if (!evt.Repeat) evt.RemoveNow = true;
            foreach (var ec in evt.Components)
            {
                ExecuteEventComponent(evt, ec);
            }
        }

        private void ExecuteEventComponent(Event evt, EventComponent ec)
        {
            switch (ec.Type)
            {
                case EventComponentType.InterMap:
                    engine.Player?.Stop();
                    TravelTo(ec.StringParam, ec.MapX, ec.MapY);
                    evt.RemoveNow = true;
                    break;
                case EventComponentType.IntraMap:
                    if (engine.Player != null)
                    {
                        engine.Player.Stop();
                        engine.Player.SetPosition(ec.MapX + 0.5f, ec.MapY + 0.5f);
                        engine.Player.HasMoved = true;
                    }
                    else
                    {
                        evt.RemoveNow = true;
                    }
                    break;
                case EventComponentType.MapMod:
                    engine.Map?.Modify(ec.MapMods);
                    break;
                case EventComponentType.Spawn:
                    //foreach(var spawn in ec.MapSpawns)
                    //{
                    //    enemyManager.SpawnMapSpawn(spawn);
                    //}
                    break;
                case EventComponentType.SoundFX:
                    //var snd = context.Audio.GetSound(ec.StringParam);
                    //if (snd != null)
                    //{
                    //    PointF pos = new PointF();
                    //    bool loop = false;
                    //    if (ec.MapX >= 0 && ec.MapY >= 0)
                    //    {
                    //        if (ec.MapX != 0 && ec.MapY != 0)
                    //        {
                    //            pos.X = ec.MapX + 0.5f;
                    //            pos.Y = ec.MapY + 0.5f;
                    //        }
                    //    }
                    //    else if (evt.Location.X != 0 && evt.Location.Y != 0)
                    //    {
                    //        pos.X = evt.Location.X + 0.5f;
                    //        pos.Y = evt.Location.Y + 0.5f;
                    //    }
                    //    if (evt.Activation == EventActivation.Load)
                    //    {
                    //        loop = true;
                    //    }
                    //    context.Audio.PlaySound(snd, pos, loop);
                    //}
                    break;
                case EventComponentType.VoxIntro:
                    string voxName = GetRandomStringParam(ec);
                    //var vox = context.Audio.GetSound();
                    //if (vox != null)
                    //{
                    //    PointF pos = new PointF();
                    //    if (ec.MapX >= 0 && ec.MapY >= 0)
                    //    {
                    //        if (ec.MapX != 0 && ec.MapY != 0)
                    //        {
                    //            pos.X = ec.MapX + 0.5f;
                    //            pos.Y = ec.MapY + 0.5f;
                    //        }
                    //    }
                    //    else if (evt.Location.X != 0 && evt.Location.Y != 0)
                    //    {
                    //        pos.X = evt.Location.X + 0.5f;
                    //        pos.Y = evt.Location.Y + 0.5f;
                    //    }
                    //    context.Audio.PlaySound(vox, pos);
                    //}
                    break;
                case EventComponentType.Music:
                    //var mus = context.Audio.GetMusic(ec.StringParam);
                    //if (mus != null)
                    //{
                    //    context.Audio.PlayMusic(mus, true);
                    //}
                    break;
                case EventComponentType.ShakyCam:
                    shakyCamDuration = ec.IntParam;
                    break;
                case EventComponentType.Power:
                    //var power = context.Application.PowerManager.GetPower(ec.IntParam);
                    //power.Activate(context.Application.EntityManager, evt);
                    break;
                case EventComponentType.SetStatus:
                    //campaignManager.SetStatus(ec.StringParams);
                    break;
                case EventComponentType.UnsetStatus:
                    //campaignManager.UnsetStatus(ec.StringParams);
                    break;
            }
        }

        private void TravelTo(string map, int x, int y)
        {
            travelMap = map;
            travelX = x;
            travelY = y;
            travel = true;

        }
        private string GetRandomStringParam(EventComponent ec)
        {
            if (ec != null && ec.StringParams != null)
            {
                int cnt = ec.StringParams.Count;
                if (cnt < 1) return "";
                if (cnt == 1) { return ec.StringParams[0]; }
                return ec.StringParams[MathUtils.Rand() % cnt];
            }
            return "";
        }

        public void AddMapEvents(Map map)
        {
            foreach (EventInfo info in map.EventInfos)
            {
                AddEvent(info);
            }
        }

        private void AddEvent(EventInfo info)
        {
            AddEvent(CreateEvent(info));
        }

        public void CreateNPCEvent(Actor npc)
        {
            Event evt = new Event();
            evt.Activation = EventActivation.Trigger;
            evt.Repeat = true;
            evt.Location = new Rectangle((int)npc.PosX, (int)npc.PosY, 1, 1);
            evt.HotSpot = new Rectangle((int)npc.PosX, (int)npc.PosY, 1, 1);
            evt.AddComponent(EventComponentType.Tooltip, npc.DisplayName);
            //if (npc.VoxIntros.Count > 0) { evt.AddComponent(EventComponentType.VoxIntro, npc.VoxIntros); }
            var npcHS = evt.AddComponent(EventComponentType.NPCHotspot);
            npcHS.MapX = evt.Location.X;
            npcHS.MapY = evt.Location.Y;
            npcHS.MapWidth = 1;
            npcHS.MapHeight = 1;
            AddEvent(evt);
        }

        private Event CreateEvent(EventInfo info)
        {
            Event evt = new Event
            {
                Activation = info.Activation,
                Location = info.Location,
                HotSpot = info.HotSpot,
                ReachableFrom = info.ReachableFrom,
                Repeat = info.Repeat,
                Cooldown = info.Cooldown,
                Delay = info.Delay
            };
            EventComponent ec;
            if (!string.IsNullOrEmpty(info.SoundFX))
            {
                ec = evt.AddComponent(EventComponentType.SoundFX, info.SoundFX);
                ec.MapX = info.SoundX;
                ec.MapY = info.SoundY;
            }
            if (info.ShakyCam > 0)
            {
                evt.AddComponent(EventComponentType.ShakyCam, info.ShakyCam);
            }
            if (!string.IsNullOrEmpty(info.Msg))
            {
                evt.AddComponent(EventComponentType.Msg, info.Msg);
            }
            if (!string.IsNullOrEmpty(info.Music))
            {
                evt.AddComponent(EventComponentType.Music, info.Music);
            }
            if (!string.IsNullOrEmpty(info.ToolTip))
            {
                evt.AddComponent(EventComponentType.Tooltip, info.ToolTip);
            }
            if (info.MapMods != null && info.MapMods.Count > 0)
            {
                ec = evt.AddComponent(EventComponentType.MapMod);
                ec.MapMods = info.MapMods;
            }
            if (info.MapSpawns != null && info.MapSpawns.Count > 0)
            {
                ec = evt.AddComponent(EventComponentType.Spawn);
                ec.MapSpawns = info.MapSpawns;
            }
            if (!string.IsNullOrEmpty(info.Map) && info.MapX >= 0 && info.MapY >= 0)
            {
                ec = evt.AddComponent(EventComponentType.InterMap, info.Map);
                ec.MapX = info.MapX;
                ec.MapY = info.MapY;
            }
            else if (info.TeleportX > 0 && info.TeleportY > 0)
            {
                ec = evt.AddComponent(EventComponentType.IntraMap);
                ec.MapX = info.TeleportX;
                ec.MapY = info.TeleportY;
            }
            else if (info.PowerId > 0)
            {
                evt.AddComponent(EventComponentType.Power, info.PowerId);
            }
            if (info.SetStatus != null && info.SetStatus.Count > 0)
            {
                evt.AddComponent(EventComponentType.SetStatus, info.SetStatus);
            }
            if (info.UnsetStatus != null && info.UnsetStatus.Count > 0)
            {
                evt.AddComponent(EventComponentType.UnsetStatus, info.UnsetStatus);
            }
            if (info.RequiresStatus != null && info.RequiresStatus.Count > 0)
            {
                evt.AddComponent(EventComponentType.RequiresStatus, info.RequiresStatus);
            }
            if (info.RequiresNotStatus != null && info.RequiresNotStatus.Count > 0)
            {
                evt.AddComponent(EventComponentType.RequiresNotStatus, info.RequiresNotStatus);
            }
            return evt;
        }
    }
}
