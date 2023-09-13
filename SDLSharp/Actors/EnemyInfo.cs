namespace SDLSharp.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EnemyInfo
    {
        public EnemyInfo()
        {
            Type = "";
            Category = "";
            Width = 1;
            Height = 1;
            MinLevel = 0;
            MaxLevel = 0;
            MinNumber = 1;
            MaxNumber = 1;
            Direction = -1;
            Chance = 100;
            WayPoints = new List<PointF>();
        }

        public string Type { get; set; }
        public string Category { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float CenterX => PosX + Width / 2.0f;
        public float CenterY => PosY + Height / 2.0f;
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }
        public int Direction { get; set; }
        public int Chance { get; set; }
        public int WanderRadius { get; set; }
        public IList<PointF> WayPoints { get; set; }
    }
}
