using SDLSharp.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.Applets
{
    public class GUISystem : SDLApplet
    {

        public GUISystem() : base("GUI System")
        {
            RenderPrio = 1000;
            InputPrio = -1000;
        }

        protected override void OnWindowUpdate(SDLWindowUpdateEventArgs e)
        {

        }

        protected override void OnWindowPaint(SDLWindowPaintEventArgs e)
        {
            Intuition.PaintDisplay(e.Renderer);
        }

        protected internal override void OnMouseButtonDown(SDLMouseEventArgs e)
        {
            if (Intuition.MouseButtonDown(e.X, e.Y, e.Button)) { e.Handled = true; }
        }

        protected internal override void OnMouseButtonUp(SDLMouseEventArgs e)
        {
            if (Intuition.MouseButtonUp(e.X, e.Y, e.Button)) { e.Handled = true; }
        }

        protected internal override void OnMouseMove(SDLMouseEventArgs e)
        {
            if (Intuition.MouseMoved(e.X, e.Y)) { e.Handled = true; }
        }


    }
}
