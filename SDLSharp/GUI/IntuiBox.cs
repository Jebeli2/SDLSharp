namespace SDLSharp.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IntuiBox
    {
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private int borderTop;
        private int borderLeft;
        private int borderRight;
        private int borderBottom;
        private int minWidth;
        private int minHeight;
        private int maxWidth;
        private int maxHeight;
        protected SDLFont? font;

        protected IntuiBox()
        {

        }


        public int LeftEdge
        {
            get => leftEdge;
            set
            {
                if (leftEdge != value)
                {
                    SetDimensions(value, topEdge, width, height);
                }
            }
        }

        public int TopEdge
        {
            get => topEdge;
            set
            {
                if (topEdge != value)
                {
                    SetDimensions(leftEdge, value, width, height);
                }
            }
        }
        public int Width
        {
            get => width;
            set
            {
                if (width != value)
                {
                    SetDimensions(leftEdge, topEdge, value, height);
                }
            }
        }

        public int Height
        {
            get => height;
            set
            {
                if (height != value)
                {
                    SetDimensions(leftEdge, topEdge, width, value);
                }
            }
        }

        public int BorderLeft
        {
            get => borderLeft;
            set
            {
                if (borderLeft != value)
                {
                    SetBorders(value, borderTop, borderRight, borderBottom);
                }
            }
        }
        public int BorderTop
        {
            get => borderTop;
            set
            {
                if (borderTop != value)
                {
                    SetBorders(borderLeft, value, borderRight, borderBottom);
                }
            }
        }
        public int BorderRight
        {
            get => borderRight;
            set
            {
                if (borderRight != value)
                {
                    SetBorders(borderLeft, borderTop, value, borderBottom);
                }
            }
        }
        public int BorderBottom
        {
            get => borderBottom;
            set
            {
                if (borderBottom != value)
                {
                    SetBorders(borderLeft, borderTop, borderRight, value);
                }
            }
        }

        public int MinWidth
        {
            get => minWidth;
            set
            {
                if (minWidth != value)
                {
                    SetMinSize(value, minHeight);
                }
            }
        }

        public int MinHeight
        {
            get => minHeight;
            set
            {
                if (minHeight != value)
                {
                    SetMinSize(minWidth, value);
                }
            }
        }

        public int MaxWidth
        {
            get => maxWidth;
            set
            {
                if (maxWidth != value)
                {
                    SetMaxSize(value, maxHeight);
                }
            }
        }

        public int MaxHeight
        {
            get => maxHeight;
            set
            {
                if (maxHeight != value)
                {
                    SetMaxSize(maxWidth, value);
                }
            }
        }

        public SDLFont? Font
        {
            get { return GetFont(); }
            set
            {
                if (font != value)
                {
                    font = value;
                    Invalidate();
                }
            }
        }

        protected virtual SDLFont? GetFont()
        {
            return font;
        }
        public virtual Rectangle GetBounds()
        {
            return new Rectangle(leftEdge, topEdge, width, height);
        }

        public Rectangle GetInnerBounds()
        {
            Rectangle rect = GetBounds();
            rect.X += borderLeft;
            rect.Y += borderTop;
            rect.Width -= (borderLeft + borderRight);
            rect.Height -= (borderTop + borderBottom);
            return rect;
        }
        public void Invalidate()
        {
            OnInvalidate();
        }
        protected internal virtual void InvalidateBounds()
        {
            OnInvalidate();
        }
        protected virtual void OnInvalidate()
        {

        }
        public virtual bool Contains(int x, int y)
        {
            return GetBounds().Contains(x, y);
        }

        public virtual void Render(SDLRenderer gfx, IGuiRenderer gui)
        {

        }
        protected virtual void SetDimensions(int x, int y, int w, int h)
        {
            leftEdge = x;
            topEdge = y;
            width = w;
            height = h;
            InvalidateBounds();
        }

        protected virtual void SetSize(int w, int h)
        {
            width = w;
            height = h;
            InvalidateBounds();
        }

        protected virtual void SetBorders(int left, int top, int right, int bottom)
        {
            borderLeft = left;
            borderTop = top;
            borderRight = right;
            borderBottom = bottom;
        }

        public virtual void SetMinSize(int minWidth, int minHeight)
        {
            this.minWidth = minWidth;
            this.minHeight = minHeight;
        }

        public virtual void SetMaxSize(int maxWidth, int maxHeight)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
        }

        protected Rectangle CheckDimensions(int sw, int sh, int x, int y, int w, int h)
        {
            if (maxWidth > 0 && w > maxWidth) { w = maxWidth; }
            if (maxHeight > 0 && h > maxHeight) { h = maxHeight; }
            if (w < minWidth) { w = minWidth; }
            if (h < minHeight) { h = minHeight; }
            if (x < 0) { x = 0; }
            if (y < 0) { y = 0; }
            if (x + w > sw) { x -= (x + w) - sw; }
            if (y + h > sh) { y -= (y + h) - sh; }
            return new Rectangle(x, y, w, h);
        }

    }
}
