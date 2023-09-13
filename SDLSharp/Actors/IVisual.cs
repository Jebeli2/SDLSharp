namespace SDLSharp.Actors
{
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IVisual
    {
        float PosX { get; }
        float PosY { get; }
        int Direction { get; }
        string Animation { get; }
        IEnumerable<ISprite> CurrentSprites { get; }
        bool HasAnimationFinished { get; }
        bool IsActiveFrame { get; }
        int Frame { get; }  
        int FrameCount { get; }

        bool Update(double totalTime, double elapsedTime);

        void SetPosition(float x, float y);
        void SetDirection(int direction);
        void SetAnimation(string animation);
    }
}
