namespace SDLSharp.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICampaignManager
    {
        void SetStatus(string status);
        void UnsetStatus(string status);    
        bool CheckAllRequirements(Event evt);
    }
}
