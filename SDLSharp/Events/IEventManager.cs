namespace SDLSharp.Events
{
    using SDLSharp.Actors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEventManager
    {
        void CheckClickEvents(float posX, float posY, float mapX, float mapY);
        void CreateNPCEvent(Actor npc);
        bool HasAnyEventsAt(float mapX, float mapY);
    }
}
