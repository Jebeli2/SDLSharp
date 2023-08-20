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
        private static int nextGadgetId;
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private bool enabled = true;
        private bool active;
        private bool mouseHover;
        private bool selected;
        private bool noHighlight;
        private bool toggleSelect;
        private GadgetType gadgetType;
        private SysGadgetType sysGadgetType;
        private bool transparentBackground;
        private Color bgColor = Color.Empty;
        private string? text;
        private Icons icon;
        private Rectangle? bounds;
        private Window? window;
        private PropInfo? propInfo;
        private GadToolsInfo? gadInfo;
        private bool leftBorder;
        private bool topBorder;
        private bool rightBorder;
        private bool bottomBorder;
        private bool relRight;
        private bool relBottom;
        private bool relWidth;
        private bool relHeight;
        private bool immediate;
        private bool relVerify;
        private int textOffsetX;
        private int textOffsetY;

        public Gadget()
        {
            GadgetId = ++nextGadgetId;
            immediate = true;
            relVerify = true;
        }

        internal Gadget(GadgetKind kind)
        {
            GadgetId = ++nextGadgetId;
            immediate = true;
            relVerify = true;
            gadInfo = new GadToolsInfo(this);
            gadInfo.Kind = kind;
        }

        public event EventHandler<EventArgs>? GadgetDown;

        public event EventHandler<EventArgs>? GadgetUp;

        internal void Render(SDLRenderer gfx, IGuiRenderer renderer, int offsetX, int offsetY)
        {
            renderer.RenderGadget(gfx, this, offsetX, offsetY);
        }
        public int GadgetId { get; internal set; }

        public string? Text
        {
            get => text;
            set => text = value;
        }

        public Icons Icon
        {
            get => icon;
            set => icon = value;
        }

        public GadgetType GadgetType
        {
            get => gadgetType;
            set
            {
                if (gadgetType != value)
                {
                    gadgetType = value;
                    switch (gadgetType)
                    {
                        case GadgetType.PropGadget:
                            propInfo = new PropInfo(this);
                            break;
                        case GadgetType.BoolGadget:
                            propInfo = null;
                            break;
                    }
                }
            }
        }

        public SysGadgetType SysGadgetType
        {
            get => sysGadgetType;
            internal set => sysGadgetType = value;
        }

        public int LeftEdge
        {
            get => leftEdge;
            set
            {
                if (leftEdge != value)
                {
                    leftEdge = value;
                    //CheckAutoFlags();
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
                    topEdge = value;
                    //CheckAutoFlags();
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
                    width = value;
                    //CheckAutoFlags();
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
                    height = value;
                    //CheckAutoFlags();
                }
            }
        }
        public bool TransparentBackground
        {
            get => transparentBackground;
            set => transparentBackground = value;
        }

        public Color BackgroundColor
        {
            get => bgColor;
            set
            {
                if (bgColor != value)
                {
                    bgColor = value;
                }
            }
        }
        public bool Selected
        {
            get => selected;
            set => selected = value;
        }

        public bool NoHighlight
        {
            get => noHighlight;
            set => noHighlight = value;
        }

        public bool ToggleSelect
        {
            get => toggleSelect;
            set => toggleSelect = value;
        }

        internal void CheckAutoFlags()
        {
            if (topEdge < 0) { relBottom = true; }
            if (leftEdge < 0) { relRight = true; }
            if (width <= 0) { relWidth = true; }
            if (height <= 0) { relHeight = true; }
        }

        public bool IsBorderGadget => leftBorder || topBorder || rightBorder || bottomBorder;
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
        public bool Immediat
        {
            get => immediate;
            internal set => immediate = value;
        }

        public bool RelVerify
        {
            get => relVerify;
            internal set => relVerify = value;
        }
        public bool Enabled => enabled;
        public bool Disbled
        {
            get => !enabled;
            set => enabled = !value;
        }
        public bool Active => active;
        public bool MouseHover => mouseHover;

        public int TextOffsetX
        {
            get => textOffsetX;
            set
            {
                if (textOffsetX != value)
                {
                    textOffsetX = value;

                }
            }
        }
        public int TextOffsetY
        {
            get => textOffsetY;
            set
            {
                if (textOffsetY != value)
                {
                    textOffsetY = value;

                }
            }
        }

        public bool IsBoolGadget => gadgetType == GadgetType.BoolGadget;
        public bool IsPropGadget => gadgetType == GadgetType.PropGadget && propInfo != null;
        public bool IsStrGadget => gadgetType == GadgetType.StrGadget;
        public bool IsCustomGadget => gadgetType == GadgetType.CustomGadget;
        public Window? Window => window;

        internal PropInfo? PropInfo => propInfo;
        internal GadToolsInfo? GadInfo => gadInfo;
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
            propInfo?.Invalidate();
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
            if (this.active != active)
            {
                SDLLog.Verbose(LogCategory.APPLICATION, $"{this} {(active ? "activated" : "deactivated")}");
                this.active = active;
                window?.Invalidate();
            }
        }

        internal void SetMouseHover(bool mouseHover)
        {
            if (this.mouseHover != mouseHover)
            {
                this.mouseHover = mouseHover;
                window?.Invalidate();
            }
        }

        internal void SetSelected(bool selected)
        {
            if (toggleSelect)
            {
                if (selected)
                {
                    this.selected = !this.selected;
                    SDLLog.Verbose(LogCategory.APPLICATION, $"{this} toggle {(this.selected ? "selected" : "deselected")}");
                }
                window?.Invalidate();
            }
            else if (this.selected != selected)
            {
                this.selected = selected;
                if (!this.selected) { propInfo?.HandleDeselection(); }
                SDLLog.Verbose(LogCategory.APPLICATION, $"{this} {(this.selected ? "selected" : "deselected")}");
                window?.Invalidate();
            }
        }


        internal void ModifyProp(PropFlags flags, int horizPot, int vertPot, int horizBody, int vertBody)
        {
            if (propInfo != null)
            {
                propInfo.Flags = flags;
                propInfo.HorizPot = horizPot;
                propInfo.VertPot = vertPot;
                propInfo.HorizBody = horizBody;
                propInfo.VertBody = vertBody;
                window?.Invalidate();
            }
        }

        internal bool HandleMouseDown(int x, int y, bool isTimerRepeat = false)
        {
            bool result = false;
            if (propInfo != null)
            {
                Rectangle bounds = GetBounds();
                result |= propInfo.HandleMouseDown(bounds, x, y, isTimerRepeat);
            }
            if (immediate) { RaiseGadgetDown(); result |= true; }
            if (result) { window?.Invalidate(); }
            return result;
        }
        internal bool HandleMouseUp(int x, int y)
        {
            bool result = false;
            if (propInfo != null)
            {
                Rectangle bounds = GetBounds();
                result |= propInfo.HandleMouseUp(bounds, x, y);
            }
            if (RelVerify) { RaiseGadgetUp(); result |= true; }
            if (result) { window?.Invalidate(); }
            return result;
        }

        internal bool HandleMouseMove(int x, int y)
        {
            bool result = false;
            if (propInfo != null)
            {
                Rectangle bounds = GetBounds();
                result |= propInfo.HandleMouseMove(bounds, x, y);
            }
            if (result) { window?.Invalidate(); }
            return result;
        }

        internal void RaiseGadgetDown()
        {
            EventHelper.Raise(this, GadgetDown, EventArgs.Empty);
        }
        internal void RaiseGadgetUp()
        {
            EventHelper.Raise(this, GadgetUp, EventArgs.Empty);
        }


        private string GetGadgetName()
        {
            StringBuilder sb = new();
            sb.Append("_GUI_Gadget_");
            sb.Append(GadgetId);
            sb.Append('_');
            return sb.ToString();
        }
        public override string ToString()
        {
            return GetGadgetName();
        }

    }
}
