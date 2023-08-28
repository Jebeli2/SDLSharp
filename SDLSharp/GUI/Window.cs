namespace SDLSharp.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    public class Window
    {
        private static int nextWindowId;

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
        private readonly List<Requester> requests = new();
        private bool inRequest;
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
        private bool sizeBRight;
        private int sysGadgetWidth = 32;
        private int sysGadgetHeight = 28;

        private Gadget? dragBar;
        private Gadget? closeGad;
        private Gadget? maxGad;
        private Gadget? minGad;
        private Gadget? sizeGad;

        private SDLTexture? bitmap;
        private bool superBitMap;
        private bool valid;
        private bool maximized;
        private bool minimized;

        private int oldLeftEdge;
        private int oldTopEdge;
        private int oldWidth;
        private int oldHeight;
        internal Window(NewWindow newWindow, Screen wbScreen)
        {
            WindowId = ++nextWindowId;
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
            superBitMap = newWindow.SuperBitmap;
            closeable = newWindow.Closing;
            dragable = newWindow.Dragging;
            resizable = newWindow.Sizing;
            borderless = newWindow.Borderless;
            backDrop = newWindow.BackDrop;
            maximizable = newWindow.Maximizing;
            minimizable = newWindow.Minimizing;
            if (!borderless)
            {
                borderLeft = 4;
                borderRight = HasRightBar ? sysGadgetWidth : 4;
                borderBottom = HasBottomBar ? sysGadgetHeight : 4;
                borderTop = HasTitleBar ? 28 : 4;
            }
            screen.AddWindow(this);
            InitSysGadgets();
            var gadgets = newWindow.Gadgets;
            if (gadgets != null && gadgets.Count > 0)
            {
                Intuition.AddGList(this, gadgets, -1, gadgets.Count);
            }
        }
        public event EventHandler<EventArgs>? WindowClose;

        public int WindowId { get; internal set; }
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

        public bool IsMaximized => maximized;
        public bool IsMinimized => minimized;
        public bool IsRestored => !maximized && !minimized;
        public bool BackDrop
        {
            get => backDrop;
            set => backDrop = value;
        }

        public bool HasBottomBar
        {
            get => !borderless && resizable && !sizeBRight;
        }

        public bool HasRightBar
        {
            get => !borderless && resizable && sizeBRight;
        }
        public bool HasTitleBar
        {
            get => !borderless && (!string.IsNullOrEmpty(Title) || dragable);
        }

        public bool SizeBBottom
        {
            get => !sizeBRight;
            internal set => sizeBRight = !value;
        }
        public bool SizeBRight
        {
            get => sizeBRight;
            internal set => sizeBRight = value;
        }

        internal bool Request(Requester req)
        {
            if (req.Window == this)
            {
                requests.Add(req);
                inRequest = true;
                Invalidate();
                return true;
            }
            return false;
        }

        internal void EndRequest(Requester req)
        {
            if (req.Window == this && requests.Remove(req))
            {
                if (requests.Count == 0)
                {
                    inRequest = false;
                }
                Invalidate();
            }
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
            if (this.active != active)
            {
                SDLLog.Verbose(LogCategory.APPLICATION, $"{this} {(active ? "activated" : "deactivated")}");
                this.active = active;
                valid = false;
            }
        }

        internal void SetMouseHover(bool mouseHover)
        {
            if (this.mouseHover != mouseHover)
            {
                this.mouseHover = mouseHover;
                valid = false;
            }
        }
        internal bool Contains(int x, int y)
        {
            return x >= leftEdge && y >= topEdge && x - leftEdge <= width && y - topEdge <= height;
        }
        internal void RaiseWindowClose()
        {
            EventHelper.Raise(this, WindowClose, EventArgs.Empty);
        }

        internal Gadget? FindNextGadget(Gadget gadget)
        {
            int index = gadgets.IndexOf(gadget);
            if (index >= 0)
            {
                while (true)
                {
                    index++;
                    if (index >= gadgets.Count) { index = 0; }
                    Gadget gad = gadgets[index];
                    if (gad == gadget) return null;
                    if (gad.Enabled && gad.TabCycle) return gad;
                }
            }
            return null;
        }
        internal Gadget? FindPreviousGadget(Gadget gadget)
        {
            int index = gadgets.IndexOf(gadget);
            if (index >= 0)
            {
                while (true)
                {
                    index--;
                    if (index < 0) { index = gadgets.Count - 1; }
                    Gadget gad = gadgets[index];
                    if (gad == gadget) return null;
                    if (gad.Enabled && gad.TabCycle) return gad;
                }
            }
            return null;
        }

        internal void Render(SDLRenderer gfx, IGuiRenderer renderer)
        {
            if (superBitMap)
            {
                if (!valid)
                {
                    CheckBitmap(gfx);
                    gfx.PushTarget(bitmap);
                    gfx.ClearScreen(Color.FromArgb(0, 0, 0, 0));
                    RenderWindow(gfx, renderer, -leftEdge, -topEdge);
                    gfx.PopTarget();
                    valid = true;
                }
                if (valid)
                {
                    if (bitmap != null)
                    {
                        Rectangle dst = GetBounds();
                        Rectangle src = new Rectangle(0, 0, dst.Width, dst.Height);
                        gfx.DrawTexture(bitmap, src, dst);
                    }
                }
            }
            else
            {
                RenderWindow(gfx, renderer, 0, 0);
            }
        }

        private void RenderWindow(SDLRenderer gfx, IGuiRenderer renderer, int offsetX, int offsetY)
        {
            int renderOffsetX = superBitMap ? -leftEdge : 0;
            int renderOffsetY = superBitMap ? -topEdge : 0;
            renderer.RenderWindow(gfx, this, offsetX, offsetY);
            foreach (var gad in gadgets.Where(g => g.IsBorderGadget))
            {
                gad.Render(gfx, renderer, renderOffsetX, renderOffsetY);
            }
            Rectangle inner = GetInnerBounds();
            inner.Offset(offsetX, offsetY);
            gfx.PushClip(inner);
            foreach (var gad in gadgets.Where(g => !g.IsBorderGadget))
            {
                gad.Render(gfx, renderer, renderOffsetX, renderOffsetY);
            }
            gfx.PopClip();
        }

        internal void MoveWindow(int dX, int dY, bool dragging = false)
        {
            int w = width;
            int h = height;
            if (maximized)
            {
                w = oldWidth;
                h = oldHeight;
            }
            Rectangle newDim = CheckDimensions(leftEdge + dX, topEdge + dY, w, h);
            SetBounds(newDim);
            if (dragging)
            {
                mouseHover = true;
                SetRestored();
            }
            InvalidateBounds();
        }

        internal void SizeWindow(int dX, int dY, bool sizing = false)
        {
            int w = width + dX;
            int h = height + dY;
            Rectangle newDim = CheckDimensions(leftEdge, topEdge, w, h);
            SetBounds(newDim);
            if (sizing)
            {
                mouseHover = true;
                SetRestored();
            }
            InvalidateBounds();
        }

        private Rectangle CheckDimensions(int x, int y, int w, int h)
        {
            int sw = screen.Width;
            int sh = screen.Height;
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

        internal void InvalidateBounds()
        {
            Invalidate();
            foreach (Gadget gadget in gadgets)
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
                    TabCycle = false,
                    RelWidth = true,
                    TransparentBackground = true,
                    TopBorder = true,
                    RightBorder = true,
                    LeftBorder = true,
                    SysGadgetType = SysGadgetType.WDragging
                };
                AddGadget(dragBar, 0);
            }
            if (closeable)
            {
                closeGad ??= new Gadget
                {
                    LeftEdge = 0,
                    TopEdge = 0,
                    Width = sysGadgetWidth,
                    Height = sysGadgetHeight,
                    TabCycle = false,
                    TransparentBackground = true,
                    Icon = Icons.CROSS,
                    TopBorder = true,
                    RightBorder = true,
                    SysGadgetType = SysGadgetType.WClosing
                };
                closeGad.GadgetUp += CloseGad_GadgetUp;
                AddGadget(closeGad, -1);
            }
            int gadX = 0;
            if (maximizable)
            {
                gadX += sysGadgetWidth;
                maxGad ??= new Gadget
                {
                    LeftEdge = -gadX,
                    TopEdge = 0,
                    Width = sysGadgetWidth,
                    Height = sysGadgetHeight,
                    TabCycle = false,
                    RelRight = true,
                    TransparentBackground = true,
                    Icon = Icons.DOCUMENT,
                    TopBorder = true,
                    RightBorder = true,
                    SysGadgetType = SysGadgetType.WMaximizing
                };
                maxGad.GadgetUp += MaxGad_GadgetUp;
                AddGadget(maxGad, -1);
            }
            if (minimizable)
            {
                gadX += sysGadgetWidth;
                minGad ??= new Gadget
                {
                    LeftEdge = -gadX,
                    TopEdge = 0,
                    Width = sysGadgetWidth,
                    Height = sysGadgetHeight,
                    TabCycle = false,
                    RelRight = true,
                    TransparentBackground = true,
                    Icon = Icons.DOCUMENT,
                    TopBorder = true,
                    RightBorder = true,
                    SysGadgetType = SysGadgetType.WMinimizing
                };
                AddGadget(minGad, -1);
            }
            if (resizable)
            {
                sizeGad = new Gadget
                {
                    LeftEdge = -sysGadgetWidth,
                    TopEdge = -sysGadgetHeight,
                    Width = sysGadgetWidth,
                    Height = sysGadgetHeight,
                    TabCycle = false,
                    RelRight = true,
                    RelBottom = true,
                    TransparentBackground = true,
                    Icon = Icons.RETWEET,
                    BottomBorder = true,
                    RightBorder = true,
                    SysGadgetType = SysGadgetType.WSizing

                };
                AddGadget(sizeGad, -1);
            }
        }

        private void CloseGad_GadgetUp(object? sender, EventArgs e)
        {
            if (sender is Gadget gad)
            {
                RaiseWindowClose();
            }
        }

        private void MaxGad_GadgetUp(object? sender, EventArgs e)
        {
            if (sender is Gadget gad)
            {
                if (!maximized)
                {
                    Maximize();
                }
                else
                {
                    Restore();
                }
            }
        }

        internal void Maximize()
        {
            if (IsRestored)
            {
                oldLeftEdge = leftEdge;
                oldTopEdge = topEdge;
                oldWidth = width;
                oldHeight = height;
            }
            SetMaximized();
            SetBounds(0, 0, screen.Width, screen.Height);
            InvalidateBounds();
        }

        internal void Restore()
        {
            SetRestored();
            SetBounds(oldLeftEdge, oldTopEdge, oldWidth, oldHeight);
            InvalidateBounds();
        }

        internal void Minimize()
        {
            if (IsRestored)
            {
                oldLeftEdge = leftEdge;
                oldTopEdge = topEdge;
                oldWidth = width;
                oldHeight = height;
            }
            SetMinimized();
            InvalidateBounds();
        }

        private void SetMaximized()
        {
            maximized = true;
            minimized = false;
            if (maxGad != null)
            {
                maxGad.Icon = Icons.DOCUMENTS;
            }
        }

        private void SetRestored()
        {
            maximized = false;
            minimized = false;
            if (maxGad != null)
            {
                maxGad.Icon = Icons.DOCUMENT;
            }
        }
        private void SetMinimized()
        {
            maximized = false;
            minimized = true;
            if (maxGad != null)
            {
                maxGad.Icon = Icons.DOCUMENTS;
            }
        }

        internal int AddGadget(Gadget gadget, int position)
        {
            int result ;
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
            gadget.SetWindow(this);
            if (gadget.SysGadgetType == SysGadgetType.None)
            {
                gadget.CheckAutoFlags();
            }
            gadget.InvalidateBounds();
            return result;
        }

        internal void Invalidate()
        {
            valid = false;
        }
        internal void Close()
        {
            bitmap?.Dispose();
            bitmap = null;
            valid = false;
        }

        internal void ToBack()
        {
            screen.WindowToBack(this);
        }
        internal void ToFront()
        {
            screen.WindowToFront(this);
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

        private void CheckBitmap(SDLRenderer renderer)
        {
            if (bitmap == null || bitmap.Width < width || bitmap.Height < height)
            {
                InitBitmap(renderer);
            }
        }
        private void InitBitmap(SDLRenderer renderer)
        {
            bitmap?.Dispose();
            bitmap = renderer.CreateTexture(GetWindowName(), width, height);
            if (bitmap != null) { bitmap.BlendMode = BlendMode.Blend; }
        }

        private string GetWindowName()
        {
            StringBuilder sb = new();
            sb.Append("_GUI_Window_");
            sb.Append(WindowId);
            sb.Append('_');
            return sb.ToString();
        }

        public override string ToString()
        {
            return GetWindowName();
        }

    }
}
