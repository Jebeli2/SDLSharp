namespace SDLTest
{
    using SDLSharp;
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
            BackgroundImage = LoadTexture(nameof(Properties.Resources.badlands));
            BackgroundMusic = LoadMusic(nameof(Properties.Resources.jesu_joy_of_mans_desiring));
            //BackgroundImage = LoadTexture("Background", Properties.Resources.badlands);
            //BackgroundMusic = LoadMusic("JesuJoy", Properties.Resources.jesu_joy_of_mans_desiring);
            AddApplet(new LinesApp());
            AddApplet(new RainingBoxesApp());
        }
    }
}
