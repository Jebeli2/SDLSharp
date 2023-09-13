namespace SDLSharp.Maps
{
    using SDLSharp.Actors;
    using SDLSharp.Content;
    using SDLSharp.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMapEngine
    {
        IMapCamera Camera { get; }
        IMapRenderer Renderer { get; }
        IActorManager ActorManager { get; }
        IEventManager EventManager { get; }
        IEnemyManager EnemyManager { get; }

        IContentManager? ContentManager { get; }
        Map? Map { get; }
        Actor? Player { get; }

        EnemyTemplate? LoadEnemyTemplate(string name);
    }
}
