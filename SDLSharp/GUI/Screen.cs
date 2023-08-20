using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class Screen
    {
        private static int nextScreenId;

        private readonly List<Window> windows = new();

        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private int mouseY;
        private int mouseX;
        private string? title;
        private string? defaultTitle;
        private bool active;
        private bool mouseHover;

        internal Screen(NewScreen newScreen)
        {
            leftEdge = newScreen.LeftEdge;
            topEdge = newScreen.TopEdge;
            width = newScreen.Width;
            height = newScreen.Height;
            defaultTitle = newScreen.DefaultTitle;
            ScreenId = ++nextScreenId;
        }
        public int ScreenId { get; internal set; }

        public int LeftEdge => leftEdge;
        public int TopEdge => topEdge;
        public int Width => width;
        public int Height => height;
        public int MouseX => mouseX;
        public int MouseY => mouseY;
        public bool Active => active;
        public bool MouseHover => mouseHover;

        internal void AddWindow(Window window)
        {
            windows.Add(window);
        }

        internal void RemoveWindow(Window window)
        {
            windows.Remove(window);
        }

        internal void Render(SDLRenderer gfx, IGuiRenderer renderer)
        {
            renderer.RenderScreen(gfx, this, leftEdge, topEdge);
            foreach (Window window in windows) { window.Render(gfx, renderer); }
        }
        internal void UpdateScreenSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            foreach (Window window in windows)
            {
                if (window.IsMaximized)
                {
                    window.SetBounds(0, 0, width, height);
                    window.InvalidateBounds();
                }
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

        internal bool Contains(int x, int y)
        {
            return x >= leftEdge && y >= topEdge && x - leftEdge <= width && y - topEdge <= height;
        }

        internal Window? FindWindow(int x, int y)
        {
            x -= leftEdge;
            y -= topEdge;
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
