using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class Window
    {
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private int mouseY;
        private int mouseX;
        private int minWidth;
        private int minHeight;
        private int maxWidth;
        private int maxHeight;
        private int borderTop;
        private int borderLeft;
        private int borderRight;
        private int borderBottom;
        private string? title;
        private Screen screen;
        private readonly List<Gadget> gadgets = new();
        private bool active;
        private bool borderless;

        internal Window(NewWindow newWindow, Screen wbScreen)
        {
            leftEdge = newWindow.LeftEdge;
            topEdge = newWindow.TopEdge;
            width = newWindow.Width;
            height = newWindow.Height;
            title = newWindow.Title;
            screen = newWindow.Screen ?? wbScreen;
            minWidth = newWindow.MinWidth;
            minHeight = newWindow.MinHeight;
            maxWidth = newWindow.MaxWidth;
            maxHeight = newWindow.MaxHeight;
            borderLeft = 4;
            borderRight = 4;
            borderBottom = 4;
            borderTop = 4;
            screen.AddWindow(this);
            active = true;
        }
        public int LeftEdge => leftEdge;
        public int TopEdge => topEdge;
        public int Width => width;
        public int Height => height;
        public int MouseX => mouseX;
        public int MouseY => mouseY;
        public string? Title => title;
        public Screen Screen => screen;
        public int MinWidth => minWidth;
        public int MinHeight => minHeight;
        public int MaxWidth => maxWidth;
        public int MaxHeight => maxHeight;

        public int BorderLeft => borderLeft;
        public int BorderTop => borderTop;
        public int BorderRight => borderRight;
        public int BorderBottom => borderBottom;
        public bool Active => active;
        public bool Borderless => borderless;
        internal Rectangle GetBounds()
        {
            return new Rectangle(screen.LeftEdge + leftEdge, screen.TopEdge + topEdge, width, height);
        }

        internal Rectangle GetInnerBounds()
        {
            Rectangle rect = GetBounds();
            rect.X += borderLeft;
            rect.Y += borderTop;
            rect.Width -= (borderLeft + borderRight);
            rect.Height -= (borderTop + borderBottom);
            return rect;
        }
        internal void Render(SDLRenderer gfx, IGuiRenderer renderer)
        {
            RenderWindow(gfx, renderer, 0, 0);
        }

        private void RenderWindow(SDLRenderer gfx, IGuiRenderer renderer, int offsetX, int offsetY)
        {
            renderer.RenderWindow(gfx, this, offsetX, offsetY);
            foreach (Gadget gadget in gadgets)
            {
                gadget.Render(gfx, renderer);
            }
        }

    }
}
