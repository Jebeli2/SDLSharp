using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp
{
    public static class SDLGfx
    {
        public static void ClearScreen(this SDLRenderer renderer, Color color)
        {
            renderer.Color = color;
            renderer.ClearScreen();
        }
        public static void DrawRect(this SDLRenderer renderer, int x, int y, int width, int height, Color color)
        {
            renderer.Color = color;
            renderer.DrawRect(new Rectangle(x, y, width, height));
        }

        public static void FillRect(this SDLRenderer renderer, int x, int y, int width, int height, Color color)
        {
            renderer.Color = color;
            renderer.FillRect(new Rectangle(x, y, width, height));
        }

        public static void FillRect(this SDLRenderer renderer, Rectangle rect, Color color)
        {
            renderer.Color = color;
            renderer.FillRect(rect);
        }
        public static void FillVertGradient(this SDLRenderer renderer, int x, int y, int w, int h, Color top, Color bottom)
        {
            FillVertGradient(renderer, new Rectangle(x, y, w, h), top, bottom);
        }

        public static void FillVertGradient(this SDLRenderer renderer, Rectangle rect, Color top, Color bottom)
        {
            renderer.FillColorRect(rect, top, top, bottom, bottom);
        }
        public static void FillVertGradient(this SDLRenderer renderer, RectangleF rect, Color top, Color bottom)
        {
            renderer.FillColorRect(rect, top, top, bottom, bottom);
        }

    }
}
