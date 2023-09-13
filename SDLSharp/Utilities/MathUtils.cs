namespace SDLSharp.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class MathUtils
    {
        private static readonly Random rnd = new();

        public static int Rand()
        {
            return rnd.Next();
        }

        public static int RandBewteen(int min, int max)
        {
            return rnd.Next(max - min) + min;
        }
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            return val;
        }

        public static float RoundForMap(float f)
        {
            return MathF.Round(f * 2, MidpointRounding.AwayFromZero) / 2;
        }
        public static bool DifferentFloat(float val1, float val2, float epsilon = 0.0000001f)
        {
            float diff = MathF.Abs(val1 - val2);
            return diff > epsilon;

            //return val1 != val2;
        }
        public static float Distance(float x0, float y0, float x1, float y1)
        {
            return MathF.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
        }
        public static float CalcTheta(float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float exact_dx = x2 - x1;
            float theta;
            if (exact_dx == 0)
            {
                if (dy > 0.0) theta = MathF.PI / 2.0f;
                else theta = -MathF.PI / 2.0f;
            }
            else
            {
                theta = MathF.Atan(dy / dx);
                if (dx < 0.0 && dy >= 0.0) theta += MathF.PI;
                if (dx < 0.0 && dy < 0.0) theta -= MathF.PI;
            }
            return theta;

        }

        public static int CalcDirection(float x0, float y0, float x1, float y1)
        {
            float theta = CalcTheta(x0, y0, x1, y1);
            float val = theta / (MathF.PI / 4);
            int dir = (int)(((val < 0) ? MathF.Ceiling(val - 0.5f) : MathF.Floor(val + 0.5f)) + 4);
            dir = (dir + 1) % 8;
            if (dir >= 0 && dir < 8)
                return dir;
            else
                return 0;
        }

        public static int NextBestDirection(int direction, float mapx, float mapy, float posx, float posy)
        {
            float dx = Math.Abs(mapx - posx);
            float dy = Math.Abs(mapy - posy);
            switch (direction)
            {
                case 0:
                    if (dy > dx) return 7;
                    else return 1;
                case 1:
                    if (mapy > posy) return 0;
                    else return 2;
                case 2:
                    if (dx > dy) return 1;
                    else return 3;
                case 3:
                    if (mapx < posx) return 2;
                    else return 4;
                case 4:
                    if (dy > dx) return 3;
                    else return 5;
                case 5:
                    if (mapy < posy) return 4;
                    else return 6;
                case 6:
                    if (dx > dy) return 5;
                    else return 7;
                case 7:
                    if (mapx > posx) return 6;
                    else return 0;
            }
            return direction;
        }


        public static float Length(float x, float y)
        {
            return MathF.Sqrt(x * x + y * y);
        }

        public static float Clamp(float v, float min, float max)
        {
            if (v > max) v = max;
            if (v < min) v = min;
            return v;
        }
    }
}
