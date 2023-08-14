namespace SDLTest
{
    using SDLSharp;
    using SDLSharp.Applets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class TestSDLWindow : SDLWindow
    {
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
            var lines = GetApplet<LinesApp>();
            boxes.RenderPrio = -500;
            lines.RenderPrio = -750;
            SDLApplication.MaxFramesPerSecond = 120;
        }
    }
}
