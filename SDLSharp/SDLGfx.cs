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
        public static void FillVertGradient(this SDLRenderer renderer, RectangleF rect, Color top, Color bottom)
        {
            renderer.FillColorRect(rect, top, top, bottom, bottom);
        }

    }
}
