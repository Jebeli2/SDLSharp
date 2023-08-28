using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class Requester
    {
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private int borderTop;
        private int borderLeft;
        private int borderRight;
        private int borderBottom;
        private bool pointRel;
        private Window? window;
        private readonly List<Gadget> gadgets = new();

        public Requester()
        {
            borderTop = 4;
            borderLeft = 4;
            borderRight = 4;
            borderBottom = 4;
        }
        public int LeftEdge
        {
            get => leftEdge;
            set => leftEdge = value;
        }
        public int TopEdge
        {
            get => topEdge;
            set => topEdge = value;
        }
        public int Width
        {
            get => width;
            set => width = value;
        }
        public int Height
        {
            get => height;
            set => height = value;
        }
        public int BorderLeft => borderLeft;
        public int BorderTop => borderTop;
        public int BorderRight => borderRight;
        public int BorderBottom => borderBottom;

        public bool PointRel
        {
            get => pointRel;
            set => pointRel = value;
        }
        public Window? Window => window;

        internal void InvalidateBounds()
        {
            foreach (Gadget gadget in gadgets)
            {
                gadget.InvalidateBounds();
            }
        }
        internal void SetWindow(Window window)
        {
            this.window = window;
        }

        internal void SetBounds(Rectangle bounds)
        {
            SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }
        internal void SetBounds(int x, int y, int w, int h)
        {
            leftEdge = x;
            topEdge = y;
            width = w;
            height = h;
        }
        internal Rectangle GetBounds()
        {
            if (window != null)
            {
                if (pointRel)
                {
                    Rectangle win = window.GetInnerBounds();
                    Rectangle res = new Rectangle(win.X + win.Width / 2 - width / 2, win.Y + win.Height / 2 - height / 2, width, height);
                    if (res.Right > win.Right)
                    {
                        res.X -= res.Right - win.Right;
                        if (res.X < 0) { res.X = 0; }
                    }
                    if (res.Bottom > win.Bottom)
                    {
                        res.Y -= res.Bottom - win.Bottom;
                        if (res.Y < 0) { res.Y = 0; }
                    }
                    leftEdge = Math.Max(0, res.X - win.Left);
                    topEdge = Math.Min(0, res.Y - win.Top);
                    return res;
                }
                else
                {
                    new Rectangle(window.LeftEdge + window.BorderLeft + leftEdge, window.TopEdge + window.BorderTop + topEdge, width, height);
                }
            }
            return new Rectangle(leftEdge, topEdge, width, height);
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

        internal bool Contains(int x, int y)
        {
            return GetBounds().Contains(x, y);
        }
        //internal void AddGadget(Gadget gadget)
        //{
        //    gadgets.Add(gadget);
        //}

        internal int AddGadget(Gadget gadget, int position)
        {
            int result;
            if (position < 0 || position >= gadgets.Count)
            {
                gadgets.Add(gadget);
                result = gadgets.Count;
            }
            else
            {
                gadgets.Insert(position, gadget);
                result = position;
            }
            gadget.SetRequester(this);
            if (gadget.SysGadgetType == SysGadgetType.None)
            {
                gadget.CheckAutoFlags();
            }
            gadget.InvalidateBounds();
            return result;
        }


        public Gadget? FindGadget(int x, int y)
        {
            for (int i = gadgets.Count - 1; i >= 0; i--)
            {
                Gadget gad = gadgets[i];
                if (gad.Enabled && gad.Contains(x, y))
                {
                    return gad;
                }
            }
            return null;
        }

        internal void Render(SDLRenderer gfx, IGuiRenderer renderer)
        {
            if (window != null)
            {
                if (window.SuperBitmap)
                {
                    renderer.RenderRequester(gfx, this, -window.LeftEdge, -window.TopEdge);
                    foreach (Gadget gadget in gadgets)
                    {
                        gadget.Render(gfx, renderer, -window.LeftEdge, -window.TopEdge);
                    }
                }
                else
                {
                    renderer.RenderRequester(gfx, this, 0, 0);
                    foreach (Gadget gadget in gadgets)
                    {
                        gadget.Render(gfx, renderer);
                    }
                }
            }
        }

    }
}
