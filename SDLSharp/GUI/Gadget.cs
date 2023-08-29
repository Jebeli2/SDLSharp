using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class Gadget : IntuiBox
    {
        private static int nextGadgetId;
        private bool enabled = true;
        private bool active;
        private bool mouseHover;
        private bool selected;
        private bool noHighlight;
        private bool toggleSelect;
        private bool endGadget;
        private GadgetType gadgetType;
        private SysGadgetType sysGadgetType;
        private bool transparentBackground;
        private Color bgColor = Color.Empty;
        private string? text;
        private Icons icon;
        private Rectangle? bounds;
        private Window? window;
        private Requester? requester;
        private PropInfo? propInfo;
        private StringInfo? stringInfo;
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
        private bool longInt;
        private bool tabCycle;
        private int textOffsetX;
        private int textOffsetY;

        public Gadget() : base()
        {
            GadgetId = ++nextGadgetId;
            immediate = true;
            relVerify = true;
            tabCycle = true;
            SetBorders(1, 1, 1, 1);
        }

        internal Gadget(GadgetKind kind) : base()
        {
            GadgetId = ++nextGadgetId;
            immediate = true;
            relVerify = true;
            tabCycle = true;
            gadInfo = new GadToolsInfo(this);
            gadInfo.Kind = kind;
        }

        public event EventHandler<EventArgs>? GadgetDown;

        public event EventHandler<EventArgs>? GadgetUp;

        internal Action<IGuiRenderer, SDLRenderer, Gadget, int, int>? CustomRenderAction;

        public override void Render(SDLRenderer gfx, IGuiRenderer gui)
        {
            if (window != null && window.SuperBitmap)
            {
                gui.RenderGadget(gfx, this, -window.LeftEdge, -window.TopEdge);
            }
            else
            {
                gui.RenderGadget(gfx, this, 0, 0);
            }
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
                        case GadgetType.StrGadget:
                            stringInfo = new StringInfo(this);
                            SetBorders(2, 2, 2, 2);
                            break;
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
            if (TopEdge < 0)
            {
                relBottom = true;
            }
            if (LeftEdge < 0) { relRight = true; }
            if (Width <= 0) { relWidth = true; }
            if (Height <= 0) { relHeight = true; }
        }

        public bool IsBorderGadget => leftBorder || topBorder || rightBorder || bottomBorder;
        public bool IsSysGadget => sysGadgetType != SysGadgetType.None;
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

        public bool LongInt
        {
            get => longInt;
            internal set => longInt = value;
        }

        public bool TabCycle
        {
            get => tabCycle;
            internal set => tabCycle = value;
        }
        public bool Enabled => enabled;
        public bool Disbled
        {
            get => !enabled;
            set => enabled = !value;
        }
        public bool EndGadget
        {
            get => endGadget;
            set => endGadget = value;
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
        public bool IsIntegerGadget => gadgetType == GadgetType.StrGadget && longInt;
        public Window? Window => window;
        public Requester? Requester => requester;
        internal PropInfo? PropInfo => propInfo;
        internal StringInfo? StringInfo => stringInfo;
        internal GadToolsInfo? GadInfo => gadInfo;

        public override Rectangle GetBounds()
        {
            bounds ??= CalculateBounds();
            return bounds.Value;
        }

        protected override void OnInvalidate()
        {
            propInfo?.Invalidate();
            stringInfo?.Invalidate();
            gadInfo?.Invalidate();
            window?.InvalidateFromGadget();
        }

        protected internal override void InvalidateBounds()
        {
            bounds = null;
            base.InvalidateBounds();
        }

        internal void SetWindow(Window window)
        {
            this.window = window;
            InvalidateBounds();
        }

        internal void SetRequester(Requester requester)
        {
            this.requester = requester;
            window = this.requester.Window;
            InvalidateBounds();
        }

        internal Gadget? FindNextGadget()
        {
            return window?.FindNextGadget(this);
        }

        internal Gadget? FindPreviousGadget()
        {
            return window?.FindPreviousGadget(this);
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
            if (requester != null)
            {
                bounds.X += requester.LeftEdge;
                bounds.Y += requester.TopEdge;
                bounds.Width = requester.Width;
                bounds.Height = requester.Height;
            }
            return bounds;
        }
        internal void SetActive(bool active)
        {
            if (this.active != active)
            {
                SDLLog.Verbose(LogCategory.APPLICATION, $"{this} {(active ? "activated" : "deactivated")}");
                this.active = active;
                window?.InvalidateFromGadget();
            }
        }

        internal void SetMouseHover(bool mouseHover)
        {
            if (this.mouseHover != mouseHover)
            {
                this.mouseHover = mouseHover;
                window?.InvalidateFromGadget();
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
                window?.InvalidateFromGadget();
            }
            else if (this.selected != selected)
            {
                this.selected = selected;
                if (!this.selected) { propInfo?.HandleDeselection(); }
                SDLLog.Verbose(LogCategory.APPLICATION, $"{this} {(this.selected ? "selected" : "deselected")}");
                window?.InvalidateFromGadget();
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
                window?.InvalidateFromGadget();
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
            else if (stringInfo != null)
            {
                Rectangle bounds = GetBounds();
                result |= stringInfo.HandleMouseDown(bounds, x, y, isTimerRepeat);
            }
            if (immediate) { RaiseGadgetDown(); result |= true; }
            if (result) { window?.InvalidateFromGadget(); }
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
            else if (stringInfo != null)
            {
                Rectangle bounds = GetBounds();
                result |= stringInfo.HandleMouseUp(bounds, x, y);
            }
            if (RelVerify) { RaiseGadgetUp(); result |= true; }
            if (result) { window?.InvalidateFromGadget(); }
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
            else if (stringInfo != null)
            {
                Rectangle bounds = GetBounds();
                result |= stringInfo.HandleMouseMove(bounds, x, y);
            }
            if (result) { window?.InvalidateFromGadget(); }
            return result;
        }

        internal ActionResult HandleKeyDown(SDLKeyEventArgs e)
        {
            ActionResult result = CheckNavKeys(e);
            if (result == ActionResult.None)
            {
                result |= stringInfo?.HandleKeyDown(e) ?? ActionResult.None;
                if (result != ActionResult.None) { window?.InvalidateFromGadget(); }
            }
            return result;
        }
        internal ActionResult HandleKeyUp(SDLKeyEventArgs e)
        {
            ActionResult result = CheckNavKeys(e);
            if (result == ActionResult.None)
            {
                result |= stringInfo?.HandleKeyUp(e) ?? ActionResult.None;
                if (result != ActionResult.None) { window?.InvalidateFromGadget(); }
            }
            return result;
        }

        internal bool HandleTextInput(SDLTextInputEventArgs e)
        {
            bool result = false;
            result |= stringInfo?.HandleTextInput(e) ?? false;
            if (result) { window?.InvalidateFromGadget(); }
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

        private ActionResult CheckNavKeys(SDLKeyEventArgs e)
        {
            if (e.ScanCode == ScanCode.SCANCODE_TAB && e.State == KeyButtonState.Pressed)
            {
                if ((e.KeyMod & KeyMod.SHIFT) != 0)
                {
                    return ActionResult.NavigatePrevious;
                }
                else
                {
                    return ActionResult.NavigateNext;
                }
            }
            else if (e.ScanCode == ScanCode.SCANCODE_RETURN && e.State == KeyButtonState.Pressed)
            {
                RaiseGadgetUp();
                return ActionResult.GadgetUp;
            }
            return ActionResult.None;
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
