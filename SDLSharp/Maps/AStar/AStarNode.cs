namespace SDLSharp.Maps.AStar
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AStarNode : IEquatable<AStarNode>, IComparable<AStarNode>
    {
        private static readonly Point[] DIRS = new[]
        {
            new Point(0, -1),
            new Point(1, -1),
            new Point(1, 0),
            new Point(1, 1),
            new Point(0, 1),
            new Point(-1, 1),
            new Point(-1, 0),
            new Point(-1, -1)
        };

        private readonly int x;
        private readonly int y;
        private float g;
        private float h;
        private Point parent;

        public AStarNode()
        {

        }

        public AStarNode(Point p)
        {
            x = p.X;
            y = p.Y;
            parent = new Point();
        }

        public AStarNode(AStarNode other)
        {
            x = other.x;
            y = other.y;
            g = other.g;
            h = other.h;
            parent = other.parent;
        }

        public int X => x;
        public int Y => y;

        public float EstimatedCost
        {
            get { return h; }
            set { h = value; }
        }

        public float ActualCost
        {
            get { return g; }
            set { g = value; }
        }

        public Point Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public float FinalCost => g + h * 2.0f;

        public override int GetHashCode()
        {
            return unchecked(x ^ y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is AStarNode node) return Equals(node);
            return false;
        }

        public bool Equals(AStarNode? other)
        {
            if (other == null) return false;
            return x == other.x && y == other.y;
        }

        public int CompareTo(AStarNode? other)
        {
            if (other == null) return 1;
            return FinalCost.CompareTo(other.FinalCost);
        }

        public IEnumerable<Point> GetNeighbours(int width, int height)
        {
            foreach (var dir in DIRS)
            {
                Point next = new(x + dir.X, y + dir.Y);
                if (next.X >= 0 && next.Y >= 0 && next.X < width && next.Y < height)
                {
                    yield return next;
                }
            }
        }
    }
}
