using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class Gadget
    {
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private bool enabled = true;
        private bool active;
        private bool mouseHover;
        private bool selected;
        private bool noHighlight;
        private GadgetType gadgetType;
        private SysGadgetType sysGadgetType;
        private bool transparentBackground;
        private string? text;
        private Rectangle? bounds;
        private Window? window;
        private bool leftBorder;
        private bool topBorder;
        private bool rightBorder;
        private bool bottomBorder;
        private bool relRight;
        private bool relBottom;
        private bool relWidth;
        private bool relHeight;
        internal void Render(SDLRenderer gfx, IGuiRenderer renderer)
        {
            renderer.RenderGadget(gfx, this, 0, 0);
        }

        public string? Text
        {
            get => text;
            set => text = value;
        }

        public GadgetType GadgetType
        {
            get => gadgetType;
            set => gadgetType = value;
        }

        public SysGadgetType SysGadgetType
        {
            get => sysGadgetType;
            internal set => sysGadgetType = value;
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
        public bool TransparentBackground
        {
            get => transparentBackground;
            set => transparentBackground = value;
        }

        public bool Selected
        {
            get => selected;
            //set => selected = value;    
        }

        public bool NoHighlight
        {
            get => noHighlight;
            set => noHighlight = value;
        }

        public bool LeftBorder
        {
            get => leftBorder;
            internal set => leftBorder = value;
        }

        public bool TopBorder
        {
            get => topBorder;
            internal set => topBorder = value;
        }

        public bool RightBorder
        {
            get => rightBorder;
            internal set => rightBorder = value;
        }

        public bool BottomBorder
        {
            get => bottomBorder;
            internal set => bottomBorder = value;
        }

        public bool RelRight
        {
            get => relRight;
            internal set => relRight = value;
        }

        public bool RelBottom
        {
            get => relBottom;
            internal set => relBottom = value;
        }

        public bool RelWidth
        {
            get => relWidth;
            internal set => relWidth = value;
        }

        public bool RelHeight
        {
            get => relHeight;
            internal set => relHeight = value;
        }
        public bool Enabled => enabled;
        public bool Active => active;
        public bool MouseHover => mouseHover;

        public bool IsBoolGadget => gadgetType == GadgetType.BoolGadget;
        public bool IsPropGadget => gadgetType == GadgetType.PropGadget;
        public bool IsStrGadget => gadgetType == GadgetType.StrGadget;
        public bool IsCustomGadget => gadgetType == GadgetType.CustomGadget;
        public Window? Window => window;

        internal Rectangle GetBounds()
        {
            if (bounds == null)
            {
                bounds = CalculateBounds();
            }
            return bounds.Value;
        }

        internal void InvalidateBounds()
        {
            bounds = null;
        }

        internal void SetWindow(Window window)
        {
            this.window = window;
        }

        private Rectangle CalculateBounds()
        {
            Rectangle pBounds = GetParentBounds();
            int x = pBounds.X;
            int y = pBounds.Y;
            int w = pBounds.Width;
            int h = pBounds.Height;
            int bx = AddRel(relRight, w) + LeftEdge;
            int by = AddRel(relBottom, h) + TopEdge;
            int bw = AddRel(relWidth, w) + Width;
            int bh = AddRel(relHeight, h) + Height;
            bx += x;
            by += y;
            return new Rectangle(bx, by, bw, bh);
        }

        private static int AddRel(bool flag, int value)
        {
            return flag ? value : 0;
        }
        private Rectangle GetParentBounds()
        {
            Rectangle bounds = new Rectangle();
            if (window != null)
            {
                bounds = window.GetBounds();
                if (!leftBorder)
                {
                    bounds.X += window.BorderLeft;
                    bounds.Width -= window.BorderLeft;
                }
                if (!topBorder)
                {
                    bounds.Y += window.BorderTop;
                    bounds.Height -= window.BorderTop;
                }
                if (!rightBorder)
                {
                    bounds.Width -= window.BorderRight;
                }
                if (!bottomBorder)
                {
                    bounds.Height -= window.BorderBottom;
                }
            }
            //if ((activation & GadgetActivation.LeftBorder) == 0)
            //{
            //    bounds.X += window.BorderLeft;
            //    bounds.Width -= window.BorderLeft;
            //}
            //if ((activation & GadgetActivation.TopBorder) == 0)
            //{
            //    bounds.Y += window.BorderTop;
            //    bounds.Height -= window.BorderTop;
            //}
            //if ((activation & GadgetActivation.RightBorder) == 0)
            //{
            //    bounds.Width -= window.BorderRight;
            //}
            //if ((activation & GadgetActivation.BottomBorder) == 0)
            //{
            //    bounds.Height -= window.BorderBottom;
            //}
            //if (requester != null)
            //{
            //    bounds.X += requester.LeftEdge;
            //    bounds.Y += requester.TopEdge;
            //    bounds.Width = requester.Width;
            //    bounds.Height = requester.Height;
            //}
            return bounds;
        }

        internal bool Contains(int x, int y)
        {
            return GetBounds().Contains(x, y);
            //return x >= leftEdge && y >= topEdge && x - leftEdge <= width && y - topEdge <= height;
        }
        internal void SetActive(bool active)
        {
            this.active = active;
        }

        internal void SetMouseHover(bool mouseHover)
        {
            this.mouseHover = mouseHover;
        }

        internal void SetSelected(bool selected)
        {
            this.selected = selected;
        }
    }
}
