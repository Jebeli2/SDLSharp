namespace SDLSharp.Events
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Event
    {
        private readonly List<EventComponent> eventComponents = new();
        private double cooldownTimer;
        private double delayTimer;
        public IEnumerable<EventComponent> Components => eventComponents;
        public EventActivation Activation { get; set; }
        public bool Repeat { get; set; }
        public int Cooldown { get; set; }
        public int Delay { get; set; }
        public Rectangle Location { get; set; }
        public Rectangle HotSpot { get; set; }
        public Rectangle ReachableFrom { get; set; }
        public bool RemoveNow { get; set; }
        public bool IsInCooldown => cooldownTimer > 0;
        public bool IsInDelay => delayTimer > 0;
        public bool NeedsPathEnd
        {
            get
            {
                if (eventComponents.Any(x => x.Type == EventComponentType.InterMap))
                {
                    return true;
                }
                return false;
            }
        }
        public void Update(double ms)
        {
            if (cooldownTimer > 0) cooldownTimer -= ms;
            if (delayTimer > 0) delayTimer -= ms;
        }

        public void StartCooldown()
        {
            cooldownTimer = Cooldown;
        }

        public void StartDelay()
        {
            delayTimer = Delay;
            cooldownTimer = Cooldown + Delay;
        }
        public bool CheckHotSpot(float mapX, float mapY)
        {
            if (!HotSpot.IsEmpty)
            {
                if (HotSpot.Contains((int)mapX, (int)mapY))
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckHit(float mapX, float mapY, IList<PointF>? path = null)
        {
            if (CheckHit(mapX, mapY))
            {
                if (path != null && path.Count > 0 && NeedsPathEnd)
                {
                    return CheckPathEndsHit(path);
                }
                return true;
            }
            return false;
        }

        private bool CheckPathEndsHit(IList<PointF> path)
        {
            PointF pathEnd = path[0];
            return CheckHit(pathEnd.X, pathEnd.Y);
        }

        private bool CheckHit(float mapX, float mapY)
        {
            if (!Location.IsEmpty)
            {
                if (Location.Contains((int)mapX, (int)mapY))
                {
                    return true;
                }
            }
            return false;
        }

        public EventComponent AddComponent(EventComponent ec)
        {
            eventComponents.Add(ec);
            return ec;
        }
        public EventComponent AddComponent(EventComponentType type, string sp = "")
        {
            return AddComponent(new EventComponent { Type = type, StringParam = sp });
        }
        public EventComponent AddComponent(EventComponentType type, IList<string> sps)
        {
            return AddComponent(new EventComponent { Type = type, StringParams = sps });
        }
        public EventComponent AddComponent(EventComponentType type, int ip)
        {
            return AddComponent(new EventComponent { Type = type, IntParam = ip });
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Activation);
            sb.Append(": (");
            sb.Append(Location.X);
            sb.Append('/');
            sb.Append(Location.Y);
            sb.Append(") ");
            if (Delay > 0)
            {
                sb.Append("Delay: ");
                sb.Append(Delay);
                sb.Append(' ');
            }
            foreach (var ec in eventComponents)
            {
                sb.Append(ec);
                sb.Append(' ');
            }
            sb.Length -= 1;
            return sb.ToString();
        }

    }
}
