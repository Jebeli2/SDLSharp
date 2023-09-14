namespace SDLSharp.Events
{
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HazardManager : IHazardManager
    {
        private readonly IMapEngine engine;
        private readonly List<Hazard> hazards = new();
        public HazardManager(IMapEngine engine)
        {
            this.engine = engine;
        }

        public void Update(double totalTime, double elapsedTime)
        {
            for (int i = hazards.Count; i > 0; i--)
            {
                if (hazards[i - 1].Lifespan <= 0) { hazards.RemoveAt(i - 1); }
            }
            CheckNewHazards();
            for (int i = hazards.Count; i > 0; i--)
            {
                Hazard haz = hazards[i - 1];
                haz.Update(totalTime, elapsedTime);
                if (haz.RemoveNow)
                {
                    hazards.RemoveAt(i - 1);
                    continue;
                }
                if (haz.HitWall)
                {
                    haz.HitWall = false;
                }
            }
            bool hit = false;
            for (int i = 0; i < hazards.Count; i++)
            {
                Hazard haz = hazards[i];

            }

        }

        public void AddRenderables(List<IMapSprite> r, List<IMapSprite> rDead)
        {
            foreach (var haz in hazards)
            {
                haz.AddRenderables(r, rDead);
            }
        }

        public void Clear()
        {
            hazards.Clear();
        }


        private void CheckNewHazards()
        {
            foreach(Hazard haz in engine.PowerManager.Hazards)
            {
                hazards.Add(haz);
            }
            engine.PowerManager.Clear();
        }
    }
}
