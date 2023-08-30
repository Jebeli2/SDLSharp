namespace SDLSharp.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMapCamera
    {
        void ScreenToMap(int x, int y, out float mx, out float my);
        void MapToScreen(float x, float y, out int sx, out int sy);
    }
}
