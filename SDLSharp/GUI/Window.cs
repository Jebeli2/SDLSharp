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
        private Gadget? closeGad;
        private Gadget? maxGad;
        private Gadget? minGad;

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
            superBitMap = true;
            closeable = true;
            dragable = true;
            maximizable = true;
            minimizable = false;
            borderLeft = 4;
            borderRight = 4;
            borderBottom = 4;
            borderTop = 28;
            screen.AddWindow(this);
            InitSysGadgets();
            var gadgets = newWindow.Gadgets;
            if (gadgets != null && gadgets.Count > 0)
            {
                Intuition.AddGList(this, gadgets, -1, gadgets.Count);
            }
        }
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
            renderer.RenderWindow(gfx, this, offsetX, offsetY);
            if (superBitMap)
            {
                foreach (Gadget gadget in gadgets)
                {
                    gadget.Render(gfx, renderer, -leftEdge, -topEdge);
                }
            }
            else
            {
                foreach (Gadget gadget in gadgets)
                {
                    gadget.Render(gfx, renderer, 0, 0);
                }
            }
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
            Rectangle newDim = new Rectangle(leftEdge + dX, topEdge + dY, w, h);
            SetBounds(newDim);
            if (dragging)
            {
                mouseHover = true;
                maximized = false;
            }
            InvalidateBounds();
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
            int gadX = 0;
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
            if (closeable)
            {
                gadX += sysGadgetWidth;
                closeGad ??= new Gadget
                {
                    LeftEdge = -gadX,
                    TopEdge = 0,
                    Width = sysGadgetWidth,
                    Height = sysGadgetHeight,
                    RelRight = true,
                    TransparentBackground = true,
                    Icon = Icons.CROSS,
                    TopBorder = true,
                    RightBorder = true,
                    SysGadgetType = SysGadgetType.WClosing
                };
                AddGadget(closeGad, -1);
            }
            if (maximizable)
            {
                gadX += sysGadgetWidth;
                maxGad ??= new Gadget
                {
                    LeftEdge = -gadX,
                    TopEdge = 0,
                    Width = sysGadgetWidth,
                    Height = sysGadgetHeight,
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
                    RelRight = true,
                    TransparentBackground = true,
                    Icon = Icons.DOCUMENT,
                    TopBorder = true,
                    RightBorder = true,
                    SysGadgetType = SysGadgetType.WMinimizing
                };
                AddGadget(minGad, -1);
            }
        }

        private void MaxGad_GadgetUp(object? sender, EventArgs e)
        {
            if (sender is Gadget gad)
            {
                if (!maximized)
                {
                    gad.Icon = Icons.DOCUMENTS;
                    Maximize();
                }
                else
                {
                    gad.Icon = Icons.DOCUMENT;
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
            maximized = true;
            minimized = false;
            SetBounds(0, 0, screen.Width, screen.Height);
            InvalidateBounds();
        }

        internal void Restore()
        {
            maximized = false;
            minimized = false;
            SetBounds(oldLeftEdge, oldTopEdge, oldWidth, oldHeight);
            InvalidateBounds();
        }

        internal void Minimize()
        {

            minimized = true;
            maximized = false;
            InvalidateBounds();
        }

        internal int AddGadget(Gadget gadget, int position)
        {
            int result = -1;
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
