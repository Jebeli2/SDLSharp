using SDLSharp.Graphics;
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

        public static void DrawLine(this SDLRenderer renderer, int x1, int y1, int x2, int y2, Color color)
        {
            renderer.Color = color;
            renderer.DrawLine(x1, y1, x2, y2);
        }

        public static void DrawRect(this SDLRenderer renderer, int x, int y, int width, int height, Color color)
        {
            renderer.Color = color;
            renderer.DrawRect(new Rectangle(x, y, width, height));
        }
        public static void DrawRect(this SDLRenderer renderer, Rectangle rect, Color color)
        {
            renderer.Color = color;
            renderer.DrawRect(rect);
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

        public static void DrawSprite(this SDLRenderer renderer, ISprite? sprite, int x, int y)
        {
            if (sprite != null)
            {
                DrawImage(renderer, sprite.Image, sprite.SourceRect, new Rectangle(x - sprite.OffsetX, y - sprite.OffsetY, sprite.Width, sprite.Height));
            }
        }

        public static void DrawImage(this SDLRenderer renderer, IImage? image, Rectangle src, Rectangle dst)
        {
            if (image is SDLTexture tex)
            {
                renderer.DrawTexture(tex, src, dst);
            }
        }
        public static void DrawImage(this SDLRenderer renderer, IImage? image, int x, int y)
        {
            if (image is SDLTexture tex)
            {
                renderer.DrawTexture(tex, new Rectangle(x, y, image.Width, image.Height));
            }
        }

    }
}
