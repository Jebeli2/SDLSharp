namespace SDLSharp.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMapRenderer : IMapCamera
    {
        bool ShowCollision { get; set; }
        void PrepareMap(Map map);
        void Update(double totalTime, double elapsedTime, Map map);
        void Render(SDLRenderer renderer, double totalTime, double elapsedTime, Map map, IList<IMapSprite> front, IList<IMapSprite> back);
        bool MoveCam(float x, float y);
        bool ShiftCam(int dX, int dY);
        bool SetCam(float x, float y);
    }
}
