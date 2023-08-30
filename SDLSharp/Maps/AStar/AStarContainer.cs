namespace SDLSharp.Maps.AStar
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AStarContainer
    {
        private int size;
        private readonly int node_limit;
        private readonly int map_width;
        private readonly int map_height;
        private readonly AStarNode[] nodes;
        private readonly int[,] map_pos;
        public AStarContainer(int _map_width, int _map_height, int _node_limit)
        {
            node_limit = _node_limit;
            map_width = _map_width;
            map_height = _map_height;
            nodes = new AStarNode[node_limit];
            map_pos = new int[map_width, map_height];
            for (int x = 0; x < map_width; x++)
            {
                for (int y = 0; y < map_height; y++)
                {
                    map_pos[x, y] = -1;
                }
            }
        }

        public void Add(AStarNode node)
        {
            if (size >= node_limit) return;
            nodes[size] = node;
            map_pos[node.X, node.Y] = size;
            int m = size;
            while (m != 0)
            {
                if (nodes[m].FinalCost < nodes[m / 2].FinalCost)
                {
                    AStarNode temp = nodes[m / 2];
                    nodes[m / 2] = nodes[m];
                    map_pos[nodes[m / 2].X, nodes[m / 2].Y] = m / 2;
                    nodes[m] = temp;
                    map_pos[nodes[m].X, nodes[m].Y] = m;
                    m = m / 2;
                }
                else
                {
                    break;
                }
            }
            size++;
        }

        public int Size => size;
        public bool IsEmpty => size == 0;
        public bool Exists(Point pos)
        {
            return map_pos[pos.X, pos.Y] != -1;
        }
        public AStarNode GetShortestF()
        {
            return nodes[0];
        }

        public void Remove(AStarNode node)
        {
            int heap_indexv = map_pos[node.X, node.Y] + 1;
            nodes[heap_indexv - 1] = nodes[size - 1];
            map_pos[nodes[heap_indexv - 1].X, nodes[heap_indexv - 1].Y] = heap_indexv - 1;
            size--;
            if (size == 0)
            {
                map_pos[node.X, node.Y] = -1;
                return;
            }
            while (true)
            {
                int heap_indexu = heap_indexv;
                if (2 * heap_indexu + 1 <= size)
                { //if both children exist
                  //Select the lowest of the two children.
                    if (nodes[heap_indexu - 1].FinalCost >= nodes[2 * heap_indexu - 1].FinalCost) heap_indexv = 2 * heap_indexu;
                    if (nodes[heap_indexv - 1].FinalCost >= nodes[2 * heap_indexu].FinalCost) heap_indexv = 2 * heap_indexu + 1;
                }
                else if (2 * heap_indexu <= size)
                { //if only child #1 exists
                  //Check if the F cost is greater than the child
                    if (nodes[heap_indexu - 1].FinalCost >= nodes[2 * heap_indexu - 1].FinalCost) heap_indexv = 2 * heap_indexu;
                }

                if (heap_indexu != heap_indexv)
                { //If parent's F > one or both of its children, swap them
                    AStarNode temp = nodes[heap_indexu - 1];
                    nodes[heap_indexu - 1] = nodes[heap_indexv - 1];
                    map_pos[nodes[heap_indexu - 1].X, nodes[heap_indexu - 1].Y] = (heap_indexu - 1);
                    nodes[heap_indexv - 1] = temp;
                    map_pos[nodes[heap_indexv - 1].X, nodes[heap_indexv - 1].Y] = (heap_indexv - 1);
                }
                else
                {
                    break;//if item <= both children, exit loop
                }

            }
            map_pos[node.X, node.Y] = -1;
        }


        public AStarNode Get(int x, int y)
        {
            return nodes[map_pos[x, y]];
        }

        public void UpdateParent(Point pos, Point parent_pos, float score)
        {
            Get(pos.X, pos.Y).Parent = parent_pos;
            Get(pos.X, pos.Y).ActualCost = score;
            int m = map_pos[pos.X, pos.Y];
            while (m != 0)
            {
                if (nodes[m].FinalCost <= nodes[m / 2].FinalCost)
                {
                    AStarNode temp = nodes[m / 2];
                    nodes[m / 2] = nodes[m];
                    map_pos[nodes[m / 2].X, nodes[m / 2].Y] = (m / 2);
                    nodes[m] = temp;
                    map_pos[nodes[m].X, nodes[m].Y] = (m);
                    m = m / 2;
                }
                else
                    break;
            }
        }
    }
}
