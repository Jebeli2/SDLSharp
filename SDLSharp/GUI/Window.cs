using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
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
        private bool backDrop;
        private bool mouseHover;
        private bool resizable;
        private bool dragable;
        private bool deptharangable;
        private bool closeable;
        private bool maximizable;
        private bool minimizable;
        private int sysGadgetWidth = 32;
        private int sysGadgetHeight = 28;

        private Gadget? dragBar;

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
            dragable = true;
            borderLeft = 4;
            borderRight = 4;
            borderBottom = 4;
            borderTop = 28;
            screen.AddWindow(this);
            InitSysGadgets();
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
        public bool MouseHover => mouseHover;
        public bool BackDrop
        {
            get => backDrop;
            set => backDrop = value;
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

        internal void SetActive(bool active)
        {
            this.active = active;
        }

        internal void SetMouseHover(bool mouseHover)
        {
            this.mouseHover = mouseHover;
        }
        internal bool Contains(int x, int y)
        {
            return x >= leftEdge && y >= topEdge && x - leftEdge <= width && y - topEdge <= height;
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

        internal void MoveWindow(int dX, int dY, bool dragging = false)
        {
            Rectangle newDim = new Rectangle(leftEdge + dX, topEdge + dY, width, height);
            SetBounds(newDim);
            if (dragging) { mouseHover = true; }
            InvalidateBounds();
        }

        internal void InvalidateBounds()
        {
            foreach(Gadget gadget in gadgets)
            {
                gadget.InvalidateBounds();
            }
        }
        private void InitSysGadgets()
        {
            if (dragable)
            {
                dragBar ??= new Gadget
                {
                    LeftEdge = 0,
                    TopEdge = 0,
                    Width = 0,
                    Height = sysGadgetHeight,
                    RelWidth = true,
                    TransparentBackground = true,
                    TopBorder = true,
                    RightBorder = true,
                    LeftBorder = true,
                    SysGadgetType = SysGadgetType.WDragging
                };
                AddGadget(dragBar, 0);
            }
        }

        internal int AddGadget(Gadget gadget, int position)
        {
            if (position < 0 || position >= gadgets.Count)
            {
                gadgets.Add(gadget);
                gadget.SetWindow(this);
                return gadgets.Count;
            }
            else
            {
                gadgets.Insert(position, gadget);
                gadget.SetWindow(this);
                return position;
            }
        }
        internal Gadget? FindGadget(int x, int y)
        {
            //if (InRequest && requests.Count > 0)
            //{
            //    Requester req = requests[^1];
            //    if (req.Contains(x, y))
            //    {
            //        return req.FindGadget(x, y);
            //    }
            //    return InReqFindGadget(x, y);
            //}
            //else
            //{
            for (int i = gadgets.Count - 1; i >= 0; i--)
            {
                Gadget gad = gadgets[i];
                if (gad.Enabled && gad.Contains(x, y))
                {
                    return gad;
                }
            }
            return null;

            //return NormalFindGadget(x, y);
            //}
        }
    }
}
