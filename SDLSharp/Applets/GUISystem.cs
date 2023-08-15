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
        private int prevMouseX;
        private int prevMouseY;
        private int mouseX;
        private int mouseY;
        private int diffMouseX;
        private int diffMouseY;

        public GUISystem() : base("GUI System")
        {

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
            UpdateMouse(e.X, e.Y);
        }

        protected internal override void OnMouseButtonUp(SDLMouseEventArgs e)
        {
            UpdateMouse(e.X, e.Y);
        }

        protected internal override void OnMouseMove(SDLMouseEventArgs e)
        {
            UpdateMouse(e.X, e.Y);
        }

        private bool UpdateMouse(int x, int y)
        {
            if (mouseX != x || mouseY != y)
            {
                prevMouseX = mouseX;
                prevMouseY = mouseY;
                mouseX = x;
                mouseY = y;
                diffMouseX = mouseX - prevMouseX;
                diffMouseY = mouseY - prevMouseY;
                //lastInputTime = currentTime;
                return true;
            }
            return false;
        }

    }
}
