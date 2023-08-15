namespace SDLTest
{
    using SDLSharp;
    using SDLSharp.Applets;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class TestSDLWindow : SDLWindow
    {
        private Icons icon = Icons.MIN;
        private double lastTime;
        public TestSDLWindow()
            : base("Test SDL")
        {
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ContentManager.AddResourceManager(Properties.Resources.ResourceManager);
            GetApplet<BackgroundImage>().Image = LoadTexture(nameof(Properties.Resources.badlands));
            GetApplet<MusicPlayer>().PlayNow(nameof(Properties.Resources.jesu_joy_of_mans_desiring));
            var boxes = GetApplet<RainingBoxesApp>();
            var music = GetApplet<MusicVisualizer>();
            var lines = GetApplet<LinesApp>();
            boxes.RenderPrio = -500;
            music.RenderPrio = -600;
            lines.RenderPrio = -750;
            SDLApplication.MaxFramesPerSecond = 120;
        }

        private void NextIcon()
        {
            icon++;
            if (icon > Icons.MAX)
            {
                icon = Icons.MIN;
                return;
            }
            while (!Enum.IsDefined(icon))
            {
                icon++;
            }
        }

        protected override void OnUpdate(SDLWindowUpdateEventArgs e)
        {
            base.OnUpdate(e);

            if (e.TotalTime - lastTime > 1000)
            {
                lastTime = e.TotalTime;
                NextIcon();
            }
        }
        protected override void OnPaint(SDLWindowPaintEventArgs e)
        {
            base.OnPaint(e);
            e.Renderer.DrawIcon(icon, Width / 2, Height / 2, Color.White);
            e.Renderer.DrawText(null, icon.ToString(), Width / 2, Height / 2 + 20, Color.White);
        }

        protected override void OnKeyUp(SDLKeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.ScanCode)
            {
                case ScanCode.SCANCODE_F2:
                    DisplayMode = DisplayMode.Windowed;
                    break;
                case ScanCode.SCANCODE_F3:
                    DisplayMode = DisplayMode.Desktop;
                    break;
                case ScanCode.SCANCODE_F4:
                    DisplayMode = DisplayMode.FullSize;
                    break;
                case ScanCode.SCANCODE_F5:
                    DisplayMode = DisplayMode.MultiMonitor;
                    break;
            }
        }

    }
}
