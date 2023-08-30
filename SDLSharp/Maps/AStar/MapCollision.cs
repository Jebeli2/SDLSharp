namespace SDLSharp.Maps.AStar
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class MapCollision : IMapCollision
    {
        private const int BLOCKS_NONE = 0;
        private const int BLOCKS_ALL = 1;
        private const int BLOCKS_MOVEMENT = 2;
        private const int BLOCKS_ALL_HIDDEN = 3;
        private const int BLOCKS_MOVEMENT_HIDDEN = 4;
        private const int MAP_ONLY = 5;
        private const int MAP_ONLY_ALT = 6;
        private const int BLOCKS_ENTITIES = 7;
        private const int BLOCKS_ENEMIES = 8;

        private const float MIN_TILE_GAP = 0.001f;

        private const int CHECK_SIGHT = 1;
        private const int CHECK_MOVEMENT = 2;

        private readonly int w;
        private readonly int h;
        private readonly int[,] colMap;
        private bool enableAllyCollision = true;
        private static bool useManhattan = false;
        public MapCollision(int width, int height)
        {
            w = width;
            h = height;
            colMap = new int[w, h];
        }
        public MapCollision(MapLayer layer)
        {
            w = layer.Width;
            h = layer.Height;
            colMap = new int[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    colMap[x, y] = layer[x, y];
                }
            }
        }

        public int Width => w;
        public int Height => h;
        public int[,] ColMap => colMap;

        public bool LineOfSight(float x1, float y1, float x2, float y2)
        {
            return LineCheck(x1, y1, x2, y2, CHECK_SIGHT, MovementType.Normal);
        }

        private bool LineCheck(float x1, float y1, float x2, float y2, int checkType, MovementType movementType)
        {
            float x = x1;
            float y = y1;
            float dx = MathF.Abs(x2 - x1);
            float dy = MathF.Abs(y2 - y1);
            float step_x;
            float step_y;
            int steps = (int)MathF.Max(dx, dy);


            if (dx > dy)
            {
                step_x = 1;
                step_y = dy / dx;
            }
            else
            {
                step_y = 1;
                step_x = dx / dy;
            }
            // fix signs
            if (x1 > x2) step_x = -step_x;
            if (y1 > y2) step_y = -step_y;


            if (checkType == CHECK_SIGHT)
            {
                for (int i = 0; i < steps; i++)
                {
                    x += step_x;
                    y += step_y;
                    if (IsWall(x, y))
                        return false;
                }
            }
            else if (checkType == CHECK_MOVEMENT)
            {
                for (int i = 0; i < steps; i++)
                {
                    x += step_x;
                    y += step_y;
                    if (!IsValidPosition(x, y, movementType, CollisionType.Normal))
                        return false;
                }
            }

            return true;
        }

        public bool Move(ref float x, ref float y, float stepX, float stepY, MovementType movementType, CollisionType collisionType)
        {
            bool forceSlide = (stepX != 0 && stepY != 0);
            while (stepX != 0 || stepY != 0)
            {
                float step_x = 0;
                if (stepX > 0)
                {
                    step_x = MathF.Min(MathF.Ceiling(x) - x, stepX);
                    if (step_x <= MIN_TILE_GAP) step_x = MathF.Min(1.0f, stepX);
                }
                else if (stepX < 0)
                {
                    step_x = MathF.Max(MathF.Floor(x) - x, stepX);
                    if (step_x == 0) step_x = MathF.Max(-1.0f, stepX);
                }
                float step_y = 0;
                if (stepY > 0)
                {
                    step_y = MathF.Min(MathF.Ceiling(y) - y, stepY);
                    if (step_y <= MIN_TILE_GAP) step_y = MathF.Min(1.0f, stepY);
                }
                else if (stepY < 0)
                {
                    step_y = MathF.Max(MathF.Floor(y) - y, stepY);
                    if (step_y == 0) step_y = MathF.Max(-1.0f, stepY);
                }
                stepX -= step_x;
                stepY -= step_y;
                if (!SmallStep(ref x, ref y, step_x, step_y, movementType, collisionType))
                {
                    if (forceSlide)
                    {
                        if (!SmallStepForcedSlideAlongGrid(ref x, ref y, step_x, step_y, movementType, collisionType))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!SmallStepForcedSlide(ref x, ref y, step_x, step_y, movementType, collisionType))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void Unblock(float x, float y)
        {
            int tileX = (int)x;
            int tileY = (int)y;
            int block = colMap[tileX, tileY];
            if (block == BLOCKS_ENTITIES || block == BLOCKS_ENEMIES)
            {
                colMap[tileX, tileY] = BLOCKS_NONE;
            }
        }

        public void Block(float x, float y, bool isAlly)
        {
            int tileX = (int)x;
            int tileY = (int)y;
            if (tileX >= 0 && tileY >= 0 && tileX < Width && tileY < Height)
            {
                int block = colMap[tileX, tileY];
                if (block == BLOCKS_NONE)
                {
                    if (isAlly)
                    {
                        colMap[tileX, tileY] = BLOCKS_ENEMIES;
                    }
                    else
                    {
                        colMap[tileX, tileY] = BLOCKS_ENTITIES;
                    }
                }
            }
        }

        public bool IsWall(float x, float y)
        {
            int tileX = (int)x;
            int tileY = (int)y;
            if (IsTileOutsideMap(tileX, tileY)) return true;
            return colMap[tileX, tileY] == BLOCKS_ALL || colMap[tileX, tileY] == BLOCKS_ALL_HIDDEN;
        }
        public bool IsOutsideMap(float tileX, float tileY)
        {
            return IsTileOutsideMap((int)tileX, (int)tileY);
        }
        private bool IsTileOutsideMap(int x, int y)
        {
            return x < 0 || y < 0 || x >= w || y >= h;
        }
        public bool IsValidPosition(float x, float y, MovementType movementType, CollisionType collisionType)
        {
            if (x < 0 || y < 0) return false;
            return IsValidTile((int)x, (int)y, movementType, collisionType);
        }

        private bool IsValidTile(int x, int y, MovementType movementType, CollisionType collisionType)
        {
            if (IsTileOutsideMap(x, y)) return false;
            if (collisionType == CollisionType.Normal)
            {
                if (colMap[x, y] == BLOCKS_ENEMIES) return false;
                if (colMap[x, y] == BLOCKS_ENTITIES) return false;
            }
            else if (collisionType == CollisionType.Player)
            {
                if (colMap[x, y] == BLOCKS_ENEMIES && !enableAllyCollision) return true;
            }
            if (movementType == MovementType.Intangible) return true;
            if (movementType == MovementType.Flying)
            {
                return !(colMap[x, y] == BLOCKS_ALL || colMap[x, y] == BLOCKS_ALL_HIDDEN);
            }
            if (colMap[x, y] == MAP_ONLY || colMap[x, y] == MAP_ONLY_ALT) return true;
            return colMap[x, y] == BLOCKS_NONE;
        }
        private bool SmallStep(ref float x, ref float y, float stepX, float stepY, MovementType movementType, CollisionType collisionType)
        {
            if (IsValidPosition(x + stepX, y + stepY, movementType, collisionType))
            {
                x += stepX;
                y += stepY;
                return true;
            }
            return false;
        }

        private bool SmallStepForcedSlideAlongGrid(ref float x, ref float y, float stepX, float stepY, MovementType movementType, CollisionType collisionType)
        {
            if (IsValidPosition(x + stepX, y, movementType, collisionType))
            {
                if (stepX == 0) return true;
                x += stepX;
            }
            else if (IsValidPosition(x, y + stepY, movementType, collisionType))
            {
                if (stepY == 0) return true;
                y += stepY;
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool SmallStepForcedSlide(ref float x, ref float y, float stepX, float stepY, MovementType movementType, CollisionType collisionType)
        {
            const float epsilon = 0.01f;
            if (stepX != 0)
            {
                float dy = y - MathF.Floor(y);
                if (IsValidTile((int)x, (int)y + 1, movementType, collisionType) &&
                    IsValidTile((int)x + Sgn(stepX), (int)y + 1, movementType, collisionType) &&
                    dy > 0.5f)
                {
                    y += Math.Min(1 - dy + epsilon, MathF.Abs(stepX));
                }
                else if (IsValidTile((int)x, (int)y - 1, movementType, collisionType) &&
                         IsValidTile((int)x + Sgn(stepX), (int)y - 1, movementType, collisionType) &&
                         dy < 0.5f)
                {
                    y -= Math.Min(dy + epsilon, MathF.Abs(stepX));
                }
                else
                {
                    return false;
                }
            }
            else if (stepY != 0)
            {
                float dx = x - MathF.Floor(x);
                if (IsValidTile((int)x + 1, (int)y, movementType, collisionType) &&
                    IsValidTile((int)x + 1, (int)y + Sgn(stepY), movementType, collisionType) &&
                    dx > 0.5f)
                {
                    x += Math.Min(1 - dx + epsilon, MathF.Abs(stepY));
                }
                else if (IsValidTile((int)x - 1, (int)y, movementType, collisionType) &&
                         IsValidTile((int)x - 1, (int)y + Sgn(stepY), movementType, collisionType) &&
                         dx < 0.5f)
                {
                    x -= Math.Min(dx + epsilon, MathF.Abs(stepY));
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private static int Sgn(float f)
        {
            if (f > 0) return 1;
            else if (f < 0) return -1;
            else return 0;
        }

        private static Point MapToCollision(PointF p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        private static PointF CollisionToMap(Point p)
        {
            return new PointF(p.X + 0.5f, p.Y + 0.5f);
        }

        private static float EstimateDistance(Point p1, Point p2)
        {
            if (useManhattan)
            {
                return MathF.Abs(p1.X - p2.X) + MathF.Abs(p1.Y - p2.Y);
            }
            else
            {
                return CalcDist(p1, p2);
            }
        }
        private static float CalcDist(Point p1, Point p2)
        {
            return MathF.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }
        public bool ComputePath(float x1, float y1, float x2, float y2, IList<PointF> path, MovementType movementType, int limit = 0)
        {
            bool destOutsideMap = IsOutsideMap(x2, y2);
            path.Clear();
            if (limit == 0) limit = w * h;
            PointF startPos = new PointF(x1, y1);
            PointF endPos = new PointF(x2, y2);
            Point start = MapToCollision(startPos);
            Point end = MapToCollision(endPos);
            bool targetBlocks = false;
            int targetBlockType = destOutsideMap ? BLOCKS_ALL : colMap[end.X, end.Y];
            CollisionType collisionType = CollisionType.Normal;
            if (targetBlockType == BLOCKS_ENTITIES || targetBlockType == BLOCKS_ENEMIES)
            {
                targetBlocks = true;
                if (!destOutsideMap) { Unblock(end.X, end.Y); }
            }
            Point current = start;
            AStarNode node = new(start)
            {
                ActualCost = 0,
                EstimatedCost = EstimateDistance(start, end),
                Parent = current
            };
            AStarContainer open = new(w, h, limit);
            AStarCloseContainer close = new(w, h, limit);
            open.Add(node);
            while (!open.IsEmpty && close.Size < limit)
            {
                node = open.GetShortestF();
                current.X = node.X;
                current.Y = node.Y;
                close.Add(node);
                open.Remove(node);
                if (current.X == end.X && current.Y == end.Y) break;
                foreach (Point neighbour in node.GetNeighbours(w, h))
                {
                    if (open.Size >= limit) break;
                    if (!IsValidTile(neighbour.X, neighbour.Y, movementType, collisionType)) continue;
                    if (close.Exists(neighbour)) continue;
                    if (!open.Exists(neighbour))
                    {
                        AStarNode newNode = new(neighbour);
                        newNode.ActualCost = node.ActualCost + CalcDist(current, neighbour);
                        newNode.Parent = current;
                        newNode.EstimatedCost = EstimateDistance(neighbour, end);
                        open.Add(newNode);
                    }
                    else
                    {
                        AStarNode i = open.Get(neighbour.X, neighbour.Y);
                        if (node.ActualCost + CalcDist(current, neighbour) < i.ActualCost)
                        {
                            Point pos = new(i.X, i.Y);
                            Point parent_pos = new(node.X, node.Y);
                            open.UpdateParent(pos, parent_pos, node.ActualCost + CalcDist(current, neighbour));
                        }
                    }
                }
            }
            if (!(current.X == end.X && current.Y == end.Y))
            {
                AStarNode? sNode = close.GetShortestH();
                if (sNode != null)
                {
                    node = sNode;
                    current.X = node.X;
                    current.Y = node.Y;
                }
            }
            while (!(current.X == start.X && current.Y == start.Y))
            {
                path.Add(CollisionToMap(current));
                current = close.Get(current.X, current.Y).Parent;
            }

            if (targetBlocks)
            {
                if (!destOutsideMap) { Block(end.X, end.Y, targetBlockType == BLOCKS_ENEMIES); }
                if (path.Count > 0 && !IsValidTile(end.X, end.Y, movementType, collisionType))
                {
                    path.RemoveAt(0);
                }
            }
            return path.Count > 0;
        }
    }
}
