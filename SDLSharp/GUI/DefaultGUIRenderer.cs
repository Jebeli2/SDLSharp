﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{

    using SDLSharp;
    using System.Reflection;

    internal class DefaultGUIRenderer : IGuiRenderer
    {
        public DefaultGUIRenderer()
        {
            BorderDark = MkColor(29, 255);
            BorderLight = MkColor(92, 255);
            BorderMedium = MkColor(35, 255);

            WindowFillUnFocused = MkColor(43, 230);
            WindowFillFocused = MkColor(45, 230);
            WindowHeaderGradientTop = MkColor(74, 255);
            WindowHeaderGradientBot = MkColor(48, 255);
            WindowHeaderGradientTopActive = Color.FromArgb(200, 62 + 10, 92 + 10, 154 + 10);
            WindowHeaderGradientBotActive = Color.FromArgb(130, 62, 92, 154);

        }
        public Color BorderDark { get; set; }
        public Color BorderLight { get; set; }
        public Color BorderMedium { get; set; }
        public Color WindowFillUnFocused { get; set; }
        public Color WindowFillFocused { get; set; }

        public Color WindowHeaderGradientTop { get; set; }
        public Color WindowHeaderGradientBot { get; set; }
        public Color WindowHeaderGradientTopActive { get; set; }
        public Color WindowHeaderGradientBotActive { get; set; }

        private static Color MkColor(int gray, int alpha)
        {
            return Color.FromArgb(alpha, gray, gray, gray);
        }


        public void RenderGadget(SDLRenderer renderer, Gadget gadget, int offsetX = 0, int offsetY = 0)
        {
            DrawGadget(renderer, gadget, offsetX, offsetY);
        }

        public void RenderRequester(SDLRenderer renderer, Requester requester, int offsetX = 0, int offsetY = 0)
        {
            DrawRequester(renderer, requester, offsetX, offsetY);
        }

        public void RenderScreen(SDLRenderer renderer, Screen screen, int offsetX = 0, int offsetY = 0)
        {
            DrawScreen(renderer, screen, offsetX, offsetY);
        }

        public void RenderWindow(SDLRenderer renderer, Window window, int offsetX = 0, int offsetY = 0)
        {
            DrawWindow(renderer, window, offsetX, offsetY);
        }

        private void DrawGadget(SDLRenderer renderer, Gadget gadget, int offsetX, int offsetY)
        {

        }
        private void DrawRequester(SDLRenderer renderer, Requester requester, int offsetX, int offsetY)
        {

        }

        private void DrawScreen(SDLRenderer renderer, Screen screen, int offsetX, int offsetY)
        {

        }

        private void DrawWindow(SDLRenderer renderer, Window window, int offsetX, int offsetY)
        {
            Rectangle bounds = window.GetBounds();
            Rectangle inner = window.GetInnerBounds();
            bool active = window.Active;
            Color bg = WindowFillUnFocused;
            Color bt = WindowHeaderGradientTop;
            Color bb = WindowHeaderGradientBot;
            if (active)
            {
                bg = WindowFillFocused;
                bt = WindowHeaderGradientTopActive;
                bb = WindowHeaderGradientBotActive;
            }
            renderer.FillRect(bounds, bg);

            if (window.BorderTop > 2) { renderer.FillVertGradient(bounds.Left, bounds.Top, bounds.Width, window.BorderTop, bt, bb); }
            if (window.BorderLeft > 2) renderer.FillRect(bounds.Left, inner.Top, window.BorderLeft, inner.Height, bb);
            if (window.BorderRight > 2) renderer.FillRect(bounds.Right - window.BorderRight - 1, inner.Top, window.BorderRight, inner.Height, bb);
            if (window.BorderBottom > 2) renderer.FillVertGradient(bounds.Left, bounds.Bottom - window.BorderBottom - 1, bounds.Width, window.BorderBottom, bb, bt);
            DrawBox(renderer, bounds, BorderLight, BorderDark);

        }

        private static void DrawBox(SDLRenderer gfx, Rectangle rect, Color shinePen, Color shadowPen)
        {
            gfx.DrawRect(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, shinePen);
            gfx.DrawRect(rect.X, rect.Y, rect.Width - 1, rect.Height - 1, shadowPen);
        }

    }
}
