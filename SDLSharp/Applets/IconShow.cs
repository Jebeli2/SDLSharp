namespace SDLSharp.Applets
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IconShow : SDLApplet
    {
        private Icons icon = Icons.MIN;
        private double lastTime;

        public IconShow()
            : base("Icon Show")
        {
            noInput = true;
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

        protected override void OnWindowUpdate(SDLWindowUpdateEventArgs e)
        {
            if (e.TotalTime - lastTime > 1000)
            {
                lastTime = e.TotalTime;
                NextIcon();
            }
        }

        protected override void OnWindowPaint(SDLWindowPaintEventArgs e)
        {
            e.Renderer.DrawIcon(icon, Width / 2, Height / 2, Color.White);
            e.Renderer.DrawText(null, icon.ToString(), Width / 2, Height / 2 + 20, Color.White);
        }
    }
}
