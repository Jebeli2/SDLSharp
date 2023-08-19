namespace SDLTest
{
    using SDLSharp;
    using SDLSharp.Applets;
    using SDLSharp.GUI;
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
        private Window? window;
        private Gadget? button1;
        private Gadget? button2;
        private Gadget? button3;

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
            var gui = GetApplet<GUISystem>();
            boxes.RenderPrio = -500;
            music.RenderPrio = -600;
            lines.RenderPrio = -750;

            SDLApplication.MaxFramesPerSecond = 120;
            button1 = new Gadget { LeftEdge = 10, TopEdge = 10, Width = -20, Height = 40, Text = "GUI Test" };
            button2 = new Gadget { LeftEdge = 10, TopEdge = 60, Width = -20, Height = 40, Text = "Blocks" };
            button3 = new Gadget { LeftEdge = 10, TopEdge = 110, Width = -20, Height = 40, Text = "Particles" };


            window = Intuition.OpenWindow(new NewWindow { LeftEdge = 50, TopEdge = 50, Width = 300, Height = 300, Title = "Window", Gadgets = new[] { button1, button2, button3 } });
            //Intuition.AddGadget(window, button1, 0);
            //Intuition.AddGadget(window, button2, 1);
        }

        protected override void OnClose(EventArgs e)
        {
            base.OnClose(e);
            Intuition.CloseWindow(window);
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
