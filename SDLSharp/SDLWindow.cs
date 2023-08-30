namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using static SDLSharp.SDL;

    public class SDLWindow : IDisposable
    {
        public const int DEFAULT_WINDOW_WIDTH = 1024;
        public const int DEFAULT_WINDOW_HEIGHT = 900;

        private bool disposedValue;
        private readonly SDLRenderer renderer;
        private readonly SDLContentManager contentManager;
        private IntPtr handle;
        private int windowId;
        private string? title;
        private int x;
        private int y;
        private int width;
        private int height;
        private bool visible;
        private bool resizeable;
        private bool alwaysOnTop;
        private bool borderless;
        private bool skipTaskbar;
        private DisplayMode displayMode;
        private bool mouseGrab;
        private bool keyboardGrab;
        private string driver;
        private bool showFPS;
        private float fpsPosX;
        private float fpsPosY;
        private int updateCount;
        private bool needAppletUpdate;
        private int oldX;
        private int oldY;
        private int oldWidth;
        private int oldHeight;

        private readonly List<SDLApplet> applets = new();
        private readonly List<SDLApplet> paintApplets = new();
        private readonly List<SDLApplet> inputApplets = new();
        private readonly List<SDLApplet> otherApplets = new();


        public SDLWindow(string? title = null)
        {
            this.title = title;
            //driver = "direct3d12";
            //driver = "direct3d11";
            driver = "opengl";
            //driver = "direct3d";
            visible = true;
            resizeable = true;
            alwaysOnTop = false;
            borderless = false;
            skipTaskbar = false;
            displayMode = DisplayMode.Windowed;
            mouseGrab = false;
            keyboardGrab = false;
            showFPS = true;
            fpsPosX = 10;
            fpsPosY = 10;
            x = SDL_WINDOWPOS_UNDEFINED;
            y = SDL_WINDOWPOS_UNDEFINED;
            width = DEFAULT_WINDOW_WIDTH;
            height = DEFAULT_WINDOW_HEIGHT;
            windowId = -1;
            handle = IntPtr.Zero;
            contentManager = new SDLContentManager(this);
            renderer = new SDLRenderer(this);
        }

        ~SDLWindow()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                ClearApplets();
                renderer.Dispose();
                if (handle != IntPtr.Zero)
                {
                    SDL_DestroyWindow(handle);
                    handle = IntPtr.Zero;
                    SDLLog.Info(LogCategory.VIDEO, $"SDLWindow {windowId} destroyed");
                }
                disposedValue = true;
            }
        }

        public IntPtr Handle => handle;
        public int WindowId => windowId;
        public bool HandleCreated => handle != IntPtr.Zero;
        public SDLRenderer Renderer => renderer;
        public SDLContentManager ContentManager => contentManager;
        public int X
        {
            get => x;
            set
            {
                if (x != value)
                {
                    SetPosition(value, y);
                }
            }
        }

        public int Y
        {
            get => y;
            set
            {
                if (y != value)
                {
                    SetPosition(x, value);
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
                    SetSize(value, height);
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
                    SetSize(width, value);
                }
            }
        }
        public string Driver
        {
            get => driver;
            set
            {
                if (driver != value)
                {
                    driver = value;
                }
            }
        }

        public DisplayMode DisplayMode
        {
            get => displayMode;
            set
            {
                if (displayMode != value)
                {
                    SetDisplayMode(value);
                }
            }
        }
        public bool ShowFPS
        {
            get => showFPS;
            set
            {
                if (showFPS != value)
                {
                    showFPS = value;
                }
            }
        }

        public float FPSPosX
        {
            get => fpsPosX;
            set
            {
                if (fpsPosX != value)
                {
                    fpsPosX = value;
                }
            }
        }

        public float FPSPosY
        {
            get => fpsPosY;
            set
            {
                if (fpsPosY != value)
                {
                    fpsPosY = value;
                }
            }
        }
        public bool MouseGrab
        {
            get => mouseGrab;
            set
            {
                if (mouseGrab != value)
                {
                    mouseGrab = value;
                    if (HandleCreated)
                    {
                        SDL_SetWindowMouseGrab(handle, mouseGrab);
                    }
                }
            }
        }

        public bool KeyboardGrab
        {
            get => keyboardGrab;
            set
            {
                if (keyboardGrab != value)
                {
                    keyboardGrab = value;
                    if (HandleCreated)
                    {
                        SDL_SetWindowKeyboardGrab(handle, keyboardGrab);
                    }
                }
            }
        }

        public string? Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    if (HandleCreated)
                    {
                        if (title != null)
                        {
                            SDL_SetWindowTitle(handle, title);
                        }
                        else
                        {
                            SDL_SetWindowTitle(handle, IntPtr.Zero);
                        }
                    }
                }
            }
        }


        public bool Visible
        {
            get => visible;
            set
            {
                if (visible != value)
                {
                    visible = value;
                    if (HandleCreated)
                    {
                        if (visible)
                        {
                            Show();
                        }
                        else
                        {
                            Hide();
                        }
                    }
                }
            }
        }
        public bool Resizeable
        {
            get => resizeable;
            set
            {
                if (resizeable != value)
                {
                    resizeable = value;
                    if (HandleCreated)
                    {
                        SDL_SetWindowResizable(handle, resizeable);
                    }
                }
            }
        }

        public bool AlwaysOnTop
        {
            get => alwaysOnTop;
            set
            {
                if (alwaysOnTop != value)
                {
                    alwaysOnTop = value;
                    if (HandleCreated)
                    {
                        SDL_SetWindowAlwaysOnTop(handle, alwaysOnTop);
                    }
                }
            }
        }

        public bool Borderless
        {
            get => borderless;
            set
            {
                if (borderless != value)
                {
                    borderless = value;
                    if (HandleCreated)
                    {
                        SDL_SetWindowBordered(handle, !borderless);
                    }
                }
            }
        }

        public bool SkipTaskbar
        {
            get => skipTaskbar;
            set
            {
                if (skipTaskbar != value)
                {
                    skipTaskbar = value;
                    if (HandleCreated)
                    {
                        SDLLog.Warn(LogCategory.VIDEO, "SkipTaskbar can only be set before the window handle is created");
                    }
                }
            }
        }

        public T GetApplet<T>() where T : SDLApplet, new()
        {
            foreach (SDLApplet applet in applets)
            {
                if (applet is T t) { return t; }
            }
            T newT = new();
            AddApplet(newT);
            return newT;
        }
        public void AddApplet(SDLApplet applet)
        {
            if (!applets.Contains(applet))
            {
                applets.Add(applet);
                InitApplet(applet);
                CacheAppletSortOrder();
            }
        }
        public void RemoveApplet(SDLApplet applet)
        {
            if (applets.Contains(applet))
            {
                applets.Remove(applet);
                FinishApplet(applet);
                CacheAppletSortOrder();
            }
        }

        public void ChangeApplet(SDLApplet applet)
        {
            if (applets.Contains(applet))
            {
                CacheAppletSortOrder();
            }
        }

        private void InitApplet(SDLApplet applet)
        {
            if (HandleCreated && !applet.Installed)
            {
                applet.OnInstall(this);
            }
        }

        private void FinishApplet(SDLApplet applet)
        {
            if (applet.Installed)
            {
                applet.OnUninstall(this);
            }
        }

        private void BeginUpdate()
        {
            needAppletUpdate = false;
            updateCount++;
        }

        private void EndUpdate()
        {
            updateCount--;
            if (updateCount == 0 && needAppletUpdate) { CacheAppletSortOrder(); }
        }

        private void CacheAppletSortOrder()
        {
            if (updateCount == 0)
            {
                paintApplets.Clear();
                paintApplets.AddRange(applets.Where(x => x.Enabled && !x.NoRender).OrderBy(x => x.RenderPrio));
                inputApplets.Clear();
                inputApplets.AddRange(applets.Where(x => x.Enabled && !x.NoInput).OrderBy(x => x.InputPrio));
                otherApplets.Clear();
                otherApplets.AddRange(applets.Where(x => x.Enabled));
                needAppletUpdate = false;
            }
            else
            {
                needAppletUpdate = true;
            }
        }

        private void ClearApplets()
        {
            foreach (SDLApplet applet in applets)
            {
                applet.OnUninstall(this);
                applet.Dispose();
            }
            applets.Clear();
            CacheAppletSortOrder();
        }

        public void SetPosition(int x, int y)
        {
            ChangePosition(x, y);
            if (HandleCreated)
            {
                SDL_SetWindowPosition(handle, x, y);
            }
        }

        public void SetSize(int width, int height)
        {
            ChangeSize(width, height);
            if (HandleCreated)
            {
                SDL_SetWindowSize(handle, width, height);
            }
        }

        public void SetBounds(int x, int y, int width, int height)
        {
            ChangePosition(x, y);
            ChangeSize(x, y);
            if (HandleCreated)
            {
                SDL_SetWindowPosition(handle, x, y);
                SDL_SetWindowSize(handle, width, height);
            }
        }

        public void SetDisplayMode(DisplayMode mode)
        {
            if (HandleCreated)
            {
                SDLLog.Info(LogCategory.VIDEO, $"Entering Display Mode {mode}");
                switch (mode)
                {
                    case DisplayMode.Windowed:
                        GoWindowed();
                        break;
                    case DisplayMode.Desktop:
                        GoDesktopFullScreen();
                        break;
                    case DisplayMode.FullSize:
                        GoFullSizeFullScreen();
                        break;
                    case DisplayMode.MultiMonitor:
                        GoMultiMonitorFullScreen();
                        break;
                }
            }
            displayMode = mode;
        }

        private void GoWindowed()
        {
            if (displayMode == DisplayMode.Desktop)
            {
                if (SDL_SetWindowFullscreen(handle, 0) == 0)
                {
                    //DetectWindowResized();
                }
            }
            //else
            //{
            SDL_SetWindowPosition(handle, oldX, oldY);
            SDL_SetWindowSize(handle, oldWidth, oldHeight);
            SDL_SetWindowBordered(handle, !borderless);
            SDL_SetWindowResizable(handle, resizeable);
            if (title != null) { SDL_SetWindowTitle(handle, title); }
            SDL_SetWindowAlwaysOnTop(handle, alwaysOnTop);
            DetectWindowResized();

            //}
        }

        private void GoDesktopFullScreen()
        {
            if (displayMode == DisplayMode.Windowed)
            {
                SDL_GetWindowPosition(handle, out oldX, out oldY);
                SDL_GetWindowSize(handle, out oldWidth, out oldHeight);
            }
            if (SDL_SetWindowFullscreen(Handle, (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP) == 0)
            {
            }
        }

        private void GoMultiMonitorFullScreen()
        {
            if (displayMode == DisplayMode.Windowed)
            {
                SDL_GetWindowPosition(handle, out oldX, out oldY);
                SDL_GetWindowSize(handle, out oldWidth, out oldHeight);
            }
            if (SDL_GetDisplayBounds(0, out Rectangle bounds) == 0)
            {
                int numDisplays = SDL_GetNumVideoDisplays();
                for (int index = 1; index < numDisplays; index++)
                {
                    if (SDL_GetDisplayBounds(index, out Rectangle otherBounds) == 0)
                    {
                        if (otherBounds.Height == bounds.Height)
                        {
                            bounds = Rectangle.Union(bounds, otherBounds);

                        }
                        else
                        {
                            break;
                        }
                    }
                }
                SDL_SetWindowBordered(handle, false);
                SDL_SetWindowResizable(handle, false);
                SDL_SetWindowTitle(handle, IntPtr.Zero);
                SDL_SetWindowAlwaysOnTop(handle, true);
                SDL_SetWindowPosition(handle, bounds.X, bounds.Y);
                SDL_SetWindowSize(handle, bounds.Width, bounds.Height);
            }
        }
        private void GoFullSizeFullScreen()
        {
            if (displayMode == DisplayMode.Windowed)
            {
                SDL_GetWindowPosition(handle, out oldX, out oldY);
                SDL_GetWindowSize(handle, out oldWidth, out oldHeight);
            }
            int index = SDL_GetWindowDisplayIndex(handle);
            if (SDL_GetDisplayBounds(index, out Rectangle bounds) == 0)
            {
                SDL_SetWindowBordered(handle, false);
                SDL_SetWindowResizable(handle, false);
                SDL_SetWindowTitle(handle, IntPtr.Zero);
                SDL_SetWindowAlwaysOnTop(handle, true);
                SDL_SetWindowPosition(handle, bounds.X, bounds.Y);
                SDL_SetWindowSize(handle, bounds.Width, bounds.Height);
            }
        }
        private void DetectWindowResized()
        {
            SDL_GetWindowSize(handle, out int w, out int h);
            if (w != width || h != height)
            {
                RaiseWindowResized(w, h);
            }
        }

        public void Show()
        {
            visible = true;
            if (HandleCreated)
            {
                SDL_ShowWindow(handle);
            }
            else
            {
                CreateHandle();
            }
        }

        public void Hide()
        {
            visible = false;
            if (HandleCreated)
            {
                SDL_HideWindow(handle);
            }
        }

        public SDLFont? LoadFont(string name, int size)
        {
            byte[]? data = contentManager.FindContent(name);
            if (data != null)
            {
                return SDLFont.LoadFont(data, name, size);
            }
            return null;
        }

        public SDLTexture? LoadTexture(string name)
        {
            byte[]? data = contentManager.FindContent(name);
            return renderer.LoadTexture(name, data);
        }

        public SDLMusic? LoadMusic(string name)
        {
            byte[]? data = contentManager.FindContent(name);
            return SDLAudio.LoadMusic(name, data);
        }
        public SDLSound? LoadSound(string name)
        {
            byte[]? data = contentManager.FindContent(name);
            return SDLAudio.LoadSound(name, data);
        }

        internal void Update(double totalTime, double elapsedTime)
        {
            SDLWindowUpdateEventArgs e = new(totalTime, elapsedTime);
            foreach (SDLApplet applet in otherApplets) { applet.InternalOnUpdate(e); }
            OnUpdate(e);
        }
        internal void Paint(double totalTime, double elapsedTime)
        {
            renderer.BeginPaint();
            SDLWindowPaintEventArgs e = new(renderer, totalTime, elapsedTime);
            foreach (SDLApplet applet in paintApplets) { applet.InternalOnPaint(e); }
            OnPaint(e);
            if (showFPS) { renderer.DrawText(null, SDLApplication.FPSText, fpsPosX, fpsPosY, 0, 0, Color.White, HorizontalAlignment.Left, VerticalAlignment.Top); }
            renderer.EndPaint();
        }

        private void ChangePosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        private void ChangeSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            renderer.WindowResized(width, height);
            //mouseScaleX = (float)backBufferWidth / width;
            //mouseScaleY = (float)backBufferHeight / height;
        }


        private void CreateHandle()
        {
            SDL_WindowFlags flags = SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI;
            if (!visible) { flags |= SDL_WindowFlags.SDL_WINDOW_HIDDEN; }
            if (resizeable) { flags |= SDL_WindowFlags.SDL_WINDOW_RESIZABLE; }
            if (alwaysOnTop) { flags |= SDL_WindowFlags.SDL_WINDOW_ALWAYS_ON_TOP; }
            if (borderless) { flags |= SDL_WindowFlags.SDL_WINDOW_BORDERLESS; }
            if (skipTaskbar) { flags |= SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR; }
            if (mouseGrab) { flags |= SDL_WindowFlags.SDL_WINDOW_MOUSE_GRABBED; }
            if (keyboardGrab) { flags |= SDL_WindowFlags.SDL_WINDOW_KEYBOARD_GRABBED; }
            if (title != null)
            {
                handle = SDL_CreateWindow(title, x, y, width, height, flags);
            }
            else
            {
                handle = SDL_CreateWindow(IntPtr.Zero, x, y, width, height, flags);
            }
            if (handle != IntPtr.Zero)
            {
                windowId = SDL_GetWindowID(handle);
                SDLLog.Info(LogCategory.VIDEO, $"SDLWindow {windowId} created");
                renderer.CreateHandle();
                BeginUpdate();
                OnHandleCreated(EventArgs.Empty);
                EndUpdate();
            }
            else
            {
                SDLLog.Critical(LogCategory.VIDEO, $"Could not create SDLWindow: {GetError()}");
            }
        }
        internal void RaiseWindowClose()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Close");
            OnClose(EventArgs.Empty);
        }
        internal void RaiseDisplayChanged(int display)
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Display Changed To {display}");

        }

        internal void RaiseWindowEnter()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Enter");
            OnEnter(EventArgs.Empty);
        }

        internal void RaiseWindowLeave()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Leave");
            OnLeave(EventArgs.Empty);
        }
        internal void RaiseWindowFocusGained()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Focus Gained");
            OnFocusGained(EventArgs.Empty);
        }
        internal void RaiseWindowFocusLost()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Focus Lost");
            OnFocusLost(EventArgs.Empty);
        }
        internal void RaiseWindowShown()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Shown");
            OnShown(EventArgs.Empty);
        }
        internal void RaiseWindowHidden()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Hidden");
            OnHidden(EventArgs.Empty);
        }
        internal void RaiseWindowExposed()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Exposed");
            OnExposed(EventArgs.Empty);
        }
        internal void RaiseWindowMaximized()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Maximized");
            OnMaximized(EventArgs.Empty);
        }
        internal void RaiseWindowMinimized()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Minimized");
            OnMinimized(EventArgs.Empty);
        }
        internal void RaiseWindowRestored()
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Restored");
            OnRestored(EventArgs.Empty);
        }

        internal void RaiseWindowMoved(int x, int y)
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Moved {x} {y}");
            ChangePosition(x, y);
            SDLWindowPositionEventArgs e = new(x, y);
            OnMoved(e);
        }

        internal void RaiseWindowResized(int width, int height)
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Resized {width} {height}");
            ChangeSize(width, height);
            SDLWindowSizeEventArgs e = new(width, height);
            foreach (SDLApplet applet in applets) { applet.OnWindowResized(e); }
            OnResized(e);
        }
        internal void RaiseWindowSizeChanged(int width, int height)
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Size Changed {width} {height}");
            ChangeSize(width, height);
            SDLWindowSizeEventArgs e = new(width, height);
            foreach (SDLApplet applet in applets) { applet.OnWindowSizeChanged(e); }
            OnSizeChanged(e);
        }

        internal void RaiseMouseButtonDown(int which, MouseButton button, KeyButtonState state, int clicks, int x, int y)
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Mouse {which} {button} {state} {x} {y} ({clicks} times)");
            SDLMouseEventArgs e = new(which, x, y, button, clicks, state, 0, 0);
            foreach (SDLApplet applet in inputApplets) { applet.OnMouseButtonDown(e); if (e.Handled) break; }
            OnMouseButtonDown(e);

        }
        internal void RaiseMouseButtonUp(int which, MouseButton button, KeyButtonState state, int clicks, int x, int y)
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Mouse {which} {button} {state} {x} {y} ({clicks} times)");
            SDLMouseEventArgs e = new(which, x, y, button, clicks, state, 0, 0);
            foreach (SDLApplet applet in inputApplets) { applet.OnMouseButtonUp(e); if (e.Handled) break; }
            OnMouseButtonUp(e);
        }
        internal void RaiseMouseMotion(int which, int x, int y, int xrel, int yrel)
        {
            //SDLLog.Verbose(LogCategory.INPUT, $"Window {windowId} Mouse {which} Moved {x} {y} ({xrel} {yrel})");
            SDLMouseEventArgs e = new(which, x, y, MouseButton.None, 0, KeyButtonState.Invalid, xrel, yrel);
            foreach (SDLApplet applet in inputApplets) { applet.OnMouseMove(e); if (e.Handled) break; }
            OnMouseMoved(e);
        }
        internal void RaiseMouseWheel(int which, int x, int y, float preciseX, float preciseY, MouseWheelDirection direction)
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Mouse {which} Wheel {x} {y} ({preciseX} {preciseY}) {direction}");
            SDLMouseWheelEventArgs e = new(which, x, y, preciseX, preciseY, direction);
            OnMouseWheel(e);
        }
        internal void RaiseKeyDown(ScanCode scanCode, KeyCode keyCode, KeyMod keyMod, KeyButtonState state, bool repeat)
        {
            if (repeat)
            {
                SDLLog.Verbose(LogCategory.INPUT, $"Window {windowId} {scanCode} {keyCode} {keyMod} {state} (repeat)");
            }
            else
            {
                SDLLog.Verbose(LogCategory.INPUT, $"Window {windowId} {scanCode} {keyCode} {keyMod} {state}");
            }
            SDLKeyEventArgs e = new(scanCode, keyCode, keyMod, state, repeat);
            foreach (SDLApplet applet in inputApplets) { applet.OnKeyDown(e); if (e.Handled) break; }
            OnKeyDown(e);
        }
        internal void RaiseKeyUp(ScanCode scanCode, KeyCode keyCode, KeyMod keyMod, KeyButtonState state, bool repeat)
        {
            if (repeat)
            {
                SDLLog.Verbose(LogCategory.INPUT, $"Window {windowId} {scanCode} {keyCode} {keyMod} {state} (repeat)");
            }
            else
            {
                SDLLog.Verbose(LogCategory.INPUT, $"Window {windowId} {scanCode} {keyCode} {keyMod} {state}");
            }
            SDLKeyEventArgs e = new(scanCode, keyCode, keyMod, state, repeat);
            foreach (SDLApplet applet in inputApplets) { applet.OnKeyUp(e); if (e.Handled) break; }
            OnKeyUp(e);
        }
        internal void RaiseTextInput(string text)
        {
            SDLLog.Debug(LogCategory.INPUT, $"Window {windowId} Text Input '{text}'");
            SDLTextInputEventArgs e = new(text);
            foreach (SDLApplet applet in inputApplets) { applet.OnTextInput(e); if (e.Handled) break; }
            OnTextInput(e);
        }

        protected virtual void OnHandleCreated(EventArgs e)
        {

        }

        protected virtual void OnUpdate(SDLWindowUpdateEventArgs e)
        {

        }
        protected virtual void OnPaint(SDLWindowPaintEventArgs e)
        {

        }
        protected virtual void OnClose(EventArgs e)
        {

        }

        protected virtual void OnEnter(EventArgs e)
        {

        }

        protected virtual void OnLeave(EventArgs e)
        {

        }

        protected virtual void OnFocusGained(EventArgs e)
        {

        }

        protected virtual void OnFocusLost(EventArgs e)
        {

        }

        protected virtual void OnShown(EventArgs e)
        {

        }

        protected virtual void OnHidden(EventArgs e)
        {

        }

        protected virtual void OnExposed(EventArgs e)
        {

        }

        protected virtual void OnMaximized(EventArgs e)
        {

        }

        protected virtual void OnMinimized(EventArgs e)
        {

        }

        protected virtual void OnRestored(EventArgs e)
        {

        }

        protected virtual void OnMoved(SDLWindowPositionEventArgs e)
        {

        }

        protected virtual void OnResized(SDLWindowSizeEventArgs e)
        {

        }

        protected virtual void OnSizeChanged(SDLWindowSizeEventArgs e)
        {

        }

        protected virtual void OnMouseButtonDown(SDLMouseEventArgs e)
        {

        }
        protected virtual void OnMouseButtonUp(SDLMouseEventArgs e)
        {

        }
        protected virtual void OnMouseMoved(SDLMouseEventArgs e)
        {

        }

        protected virtual void OnMouseWheel(SDLMouseWheelEventArgs e)
        {

        }

        protected virtual void OnKeyDown(SDLKeyEventArgs e)
        {

        }
        protected virtual void OnKeyUp(SDLKeyEventArgs e)
        {

        }

        protected virtual void OnTextInput(SDLTextInputEventArgs e)
        {

        }
    }
}
