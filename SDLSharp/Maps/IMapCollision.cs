namespace SDLSharp.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMapCollision
    {
        int Width { get; }
        int Height { get; }
        int[,] ColMap { get; }
        bool Move(ref float x, ref float y, float stepX, float stepY, MovementType movementType, CollisionType collisionType);
        void Block(float x, float y, bool isAlly);
        void Unblock(float x, float y);
        bool LineOfSight(float x1, float y1, float x2, float y2);
        bool ComputePath(float x1, float y1, float x2, float y2, IList<PointF> path, MovementType movementType, int limit = 0);
        bool IsValidPosition(float x, float y, MovementType movementType, CollisionType collisionType);
        bool IsOutsideMap(float x, float y);
        bool IsWall(float x, float y);

    }
}
