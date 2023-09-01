namespace SDLSharp.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Extensions
    {
        public const float DIST = 0.1f;
        public static bool IsAt(this Actor? entity, float x, float y, float dist = DIST)
        {
            if (entity != null)
            {
                float minx = x - dist;
                float maxx = x + dist;
                float miny = y - dist;
                float maxy = y + dist;
                return entity.PosX > minx && entity.PosX < maxx && entity.PosY > miny && entity.PosY < maxy;
            }
            return false;
        }
    }
}
