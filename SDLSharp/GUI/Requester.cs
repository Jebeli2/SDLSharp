using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class Requester : IntuiBox
    {
        private bool pointRel;
        private Window? window;
        private readonly List<Gadget> gadgets = new();

        public Requester() : base()
        {
            SetBorders(4, 4, 4, 4);
        }
        public bool PointRel
        {
            get => pointRel;
            set => pointRel = value;
        }
        public Window? Window => window;


        protected internal override void InvalidateBounds()
        {
            foreach (Gadget gadget in gadgets)
            {
                gadget.InvalidateBounds();
            }
        }
        internal void SetWindow(Window window)
        {
            this.window = window;
            foreach (Gadget gadget in gadgets)
            {
                gadget.SetWindow(window);
            }
        }

        protected override SDLFont? GetFont()
        {
            return font ?? window?.Font;
        }
        public override Rectangle GetBounds()
        {
            if (window != null)
            {
                if (pointRel)
                {
                    Rectangle win = window.GetInnerBounds();
                    Rectangle res = new Rectangle(win.X + win.Width / 2 - Width / 2, win.Y + win.Height / 2 - Height / 2, Width, Height);
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
                    LeftEdge = Math.Max(0, res.X - win.Left);
                    TopEdge = Math.Max(0, res.Y - win.Top);
                    return res;
                }
                else
                {
                    return new Rectangle(window.LeftEdge + window.BorderLeft + LeftEdge, window.TopEdge + window.BorderTop + TopEdge, Width, Height);
                }
            }
            return new Rectangle(LeftEdge, TopEdge, Width, Height);
        }

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

        public override void Render(SDLRenderer gfx, IGuiRenderer gui)
        {
            if (window != null)
            {
                if (window.SuperBitmap)
                {
                    gui.RenderRequester(gfx, this, -window.LeftEdge, -window.TopEdge);
                }
                else
                {
                    gui.RenderRequester(gfx, this, 0, 0);
                }
                foreach (Gadget gadget in gadgets)
                {
                    gadget.Render(gfx, gui);
                }
            }
        }

    }
}
