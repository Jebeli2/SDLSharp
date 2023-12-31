﻿using SDLSharp.GUI;
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
            Intuition.Update(e.TotalTime);
        }

        protected override void OnWindowPaint(SDLWindowPaintEventArgs e)
        {
            Intuition.PaintDisplay(e.Renderer);
        }

        protected override void OnWindowSizeChanged(int width, int height)
        {
            Intuition.WindowSizeChanged(width, height);
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

        protected internal override void OnKeyDown(SDLKeyEventArgs e)
        {
            if (Intuition.KeyDown(e)) { e.Handled = true; }
        }

        protected internal override void OnKeyUp(SDLKeyEventArgs e)
        {
            if (Intuition.KeyUp(e)) { e.Handled = true; }
        }

        protected internal override void OnTextInput(SDLTextInputEventArgs e)
        {
            if (Intuition.TextInput(e)) { e.Handled = true; }
        }

    }
}
