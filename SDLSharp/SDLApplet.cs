namespace SDLSharp
{
    using SDLSharp.Content;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Authentication.ExtendedProtection;
    using System.Text;
    using System.Threading.Tasks;

    public class SDLApplet : IDisposable
    {
        private readonly string name;
        private bool installed;
        private bool enabled;
        private SDLWindow? window;
        private SDLRenderer? renderer;
        private int width;
        private int height;
        private int renderPrio;
        private int inputPrio;
        protected bool noInput;
        protected bool noRender;
        private bool disposedValue;


        public SDLApplet(string name)
        {
            this.name = name;
            enabled = true;
        }

        public string Name => name;
        public int Width => width;
        public int Height => height;

        public int RenderPrio
        {
            get => renderPrio;
            set
            {
                if (renderPrio != value)
                {
                    renderPrio = value;
                    AppletChanged();
                }
            }
        }

        public int InputPrio
        {
            get => inputPrio;
            set
            {
                if (inputPrio != value)
                {
                    inputPrio = value;
                    AppletChanged();
                }
            }
        }
        public bool NoInput => noInput;
        public bool NoRender => noRender;

        public bool Installed
        {
            get => installed;
        }
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    AppletChanged();
                }
            }
        }
        protected void AppletChanged()
        {
            window?.ChangeApplet(this);
        }

        internal void OnInstall(SDLWindow window)
        {
            this.window = window;
            renderer = this.window.Renderer;
            installed = true;
            InternalOnLoad(new SDLWindowLoadEventArgs(renderer));
        }

        internal void OnUninstall(SDLWindow window)
        {
            if (this.window == window)
            {
                installed = false;
                this.window = null;
                renderer = null;
            }
        }

        public SDLWindow? Window => window;

        public IContentManager? ContentManager => window?.ContentManager;

        protected SDLTexture? LoadTexture(string name)
        {
            //return window?.ContentManager.Load<SDLTexture>(name);
            return window?.LoadTexture(name);
        }

        protected SDLSound? LoadSound(string name)
        {
            return window?.LoadSound(name);
        }

        protected SDLMusic? LoadMusic(string name)
        {
            return window?.LoadMusic(name);
        }

        protected virtual void OnWindowLoad(SDLWindowLoadEventArgs e) { }
        protected virtual void OnWindowUpdate(SDLWindowUpdateEventArgs e) { }
        protected virtual void OnWindowPaint(SDLWindowPaintEventArgs e) { }
        protected virtual void OnWindowSizeChanged(int width, int height) { }

        internal void OnWindowResized(SDLWindowSizeEventArgs e)
        {
            width = e.Width;
            height = e.Height;
            OnWindowSizeChanged(width, height);
        }
        internal void OnWindowSizeChanged(SDLWindowSizeEventArgs e)
        {
            width = e.Width;
            height = e.Height;
            OnWindowSizeChanged(width, height);
        }

        internal protected virtual void OnMouseButtonDown(SDLMouseEventArgs e) { }
        internal protected virtual void OnMouseButtonUp(SDLMouseEventArgs e) { }
        internal protected virtual void OnMouseMove(SDLMouseEventArgs e) { }
        internal protected virtual void OnKeyDown(SDLKeyEventArgs e) { }
        internal protected virtual void OnKeyUp(SDLKeyEventArgs e) { }
        internal protected virtual void OnTextInput(SDLTextInputEventArgs e) { }
        protected virtual void OnDispose() { }

        internal void InternalOnUpdate(SDLWindowUpdateEventArgs e)
        {
            OnWindowUpdate(e);
        }
        internal void InternalOnPaint(SDLWindowPaintEventArgs e)
        {
            renderer = e.Renderer;
            width = e.Renderer.Width;
            height = e.Renderer.Height;
            OnWindowPaint(e);
        }
        internal void InternalOnLoad(SDLWindowLoadEventArgs e)
        {
            renderer = e.Renderer;
            width = e.Renderer.Width;
            height = e.Renderer.Height;
            OnWindowLoad(e);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnDispose();
                }

                disposedValue = true;
            }
        }

        ~SDLApplet()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
