namespace SDLSharp.Events
{
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CampaignManager : ICampaignManager
    {
        private readonly IMapEngine engine;
        private readonly HashSet<string> stati = new();
        private bool activateAllWaypoints;
        public CampaignManager(IMapEngine engine)
        {
            this.engine = engine;
        }

        public bool ActivateAllWaypoints
        {
            get => activateAllWaypoints;
            set => activateAllWaypoints = value;
        }

        public bool CheckAllRequirements(Event evt)
        {

            var requires = evt.Components.Where(x => x.Type == EventComponentType.RequiresStatus);
            foreach(var req in requires)
            {
                if (!CheckRequires(req)) return false;
            }
            var requiresNot = evt.Components.Where(x => x.Type == EventComponentType.RequiresNotStatus);
            foreach(var reqNot in requiresNot)
            {
                if (!CheckRequiresNot(reqNot)) return false;
            }
            return true;
        }

        private bool CheckRequires(EventComponent eventComponent)
        {
            foreach(string s in eventComponent.StringParams)
            {
                if (!CheckStatus(s))
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckRequiresNot(EventComponent eventComponent)
        {
            foreach (string s in eventComponent.StringParams)
            {
                if (CheckStatus(s))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return true;
            if (stati.Contains(status)) return true;
            if (activateAllWaypoints && status.Contains("waypoint")) return true;
            return false;
        }

        public void SetStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return;
            if (!stati.Contains(status))
            {
                stati.Add(status);
                SDLLog.Verbose(LogCategory.APPLICATION, $"Status {status} set");
            }
        }
        public void UnsetStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return;
            if (stati.Contains(status))
            {
                stati.Remove(status);
                SDLLog.Verbose(LogCategory.APPLICATION, $"Status {status} cleared");
            }
        }

    }
}
