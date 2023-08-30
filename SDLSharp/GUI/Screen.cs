using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class Screen : IntuiBox
    {
        private static int nextScreenId;

        private readonly List<Window> windows = new();

        private int mouseY;
        private int mouseX;
        private string? title;
        private string? defaultTitle;
        private bool active;
        private bool mouseHover;

        internal Screen(NewScreen newScreen) : base()
        {
            SetDimensions(newScreen.LeftEdge, newScreen.TopEdge, newScreen.Width, newScreen.Height);
            font = newScreen.Font;
            defaultTitle = newScreen.DefaultTitle;
            ScreenId = ++nextScreenId;
        }
        public int ScreenId { get; internal set; }
        public int MouseX => mouseX;
        public int MouseY => mouseY;
        public bool Active => active;
        public bool MouseHover => mouseHover;

        internal IList<Window> Windows => windows;
        internal void AddWindow(Window window)
        {
            windows.Add(window);
        }

        internal void RemoveWindow(Window window)
        {
            windows.Remove(window);
        }

        internal void MoveScreen(int dX, int dY, bool dragging = false)
        {
            SetDimensions(LeftEdge + dX, TopEdge + dY, Width, Height);
        }

        public override void Render(SDLRenderer gfx, IGuiRenderer gui)
        {
            gui.RenderScreen(gfx, this, LeftEdge, TopEdge);
            foreach (Window win in windows)
            {
                win.Render(gfx, gui);
            }
        }

        internal void UpdateScreenSize(int width, int height)
        {
            SetSize(width, height);
            foreach (Window window in windows)
            {
                if (window.IsMaximized)
                {
                    window.SetBounds(0, 0, width, height);
                    window.InvalidateBounds();
                }
            }
        }
        protected override void OnInvalidate()
        {
            foreach (Window window in windows)
            {
                window.Invalidate();
            }
        }
        internal void SetActive(bool active)
        {
            if (this.active != active)
            {
                SDLLog.Verbose(LogCategory.APPLICATION, $"{this} {(active ? "activated" : "deactivated")}");
                this.active = active;
            }
        }


        internal void SetMouseHover(bool mouseHover)
        {
            this.mouseHover = mouseHover;
        }

        internal Window? FindWindow(int x, int y)
        {
            x -= LeftEdge;
            y -= TopEdge;
            for (int i = windows.Count - 1; i >= 0; i--)
            {
                Window win = windows[i];
                if (win.Contains(x, y))
                {
                    return win;
                }
            }
            return null;
        }

        private string GetScreenName()
        {
            StringBuilder sb = new();
            sb.Append("_GUI_Screen_");
            sb.Append(ScreenId);
            sb.Append('_');
            return sb.ToString();
        }
        public override string ToString()
        {
            return GetScreenName();
        }
        internal void WindowToBack(Window window)
        {
            int index = windows.IndexOf(window);
            if (index > 0)
            {
                windows.RemoveAt(index);
                windows.Insert(0, window);
            }
        }

        internal void WindowToFront(Window window)
        {
            int index = windows.IndexOf(window);
            if (index >= 0 && index < windows.Count - 1)
            {
                windows.RemoveAt(index);
                windows.Add(window);
            }
        }
    }
}
