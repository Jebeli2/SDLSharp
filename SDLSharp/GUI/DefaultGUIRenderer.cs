using System;
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
            TextColor = Color.FromArgb(238, 238, 238);
            DisabledTextColor = MkColor(255, 80);

            BorderDark = MkColor(29, 255);
            BorderLight = MkColor(92, 255);
            BorderMedium = MkColor(35, 255);

            WindowFillUnFocused = MkColor(43, 230);
            WindowFillFocused = MkColor(45, 230);
            WindowHeaderGradientTop = MkColor(74, 255);
            WindowHeaderGradientBot = MkColor(48, 255);
            WindowHeaderGradientTopActive = Color.FromArgb(200, 62 + 10, 92 + 10, 154 + 10);
            WindowHeaderGradientBotActive = Color.FromArgb(130, 62, 92, 154);

            ButtonGradientTopFocused = MkColor(64, 255);
            ButtonGradientBotFocused = MkColor(48, 255);
            ButtonGradientTopUnFocused = MkColor(74, 255);
            ButtonGradientBotUnFocused = MkColor(58, 255);
            ButtonGradientTopPushed = MkColor(41, 255);
            ButtonGradientBotPushed = MkColor(28, 255);
            ButtonGradientTopHover = MkColor(84, 255);
            ButtonGradientBotHover = MkColor(68, 255);

        }
        public Color BorderDark { get; set; }
        public Color BorderLight { get; set; }
        public Color BorderMedium { get; set; }
        public Color TextColor { get; set; }
        public Color DisabledTextColor { get; set; }

        public Color WindowFillUnFocused { get; set; }
        public Color WindowFillFocused { get; set; }

        public Color WindowHeaderGradientTop { get; set; }
        public Color WindowHeaderGradientBot { get; set; }
        public Color WindowHeaderGradientTopActive { get; set; }
        public Color WindowHeaderGradientBotActive { get; set; }
        public Color ButtonGradientTopFocused { get; set; }
        public Color ButtonGradientBotFocused { get; set; }
        public Color ButtonGradientTopUnFocused { get; set; }
        public Color ButtonGradientBotUnFocused { get; set; }
        public Color ButtonGradientTopPushed { get; set; }
        public Color ButtonGradientBotPushed { get; set; }
        public Color ButtonGradientTopHover { get; set; }
        public Color ButtonGradientBotHover { get; set; }

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
            if (gadget.IsBoolGadget)
            {
                DrawBoolGadget(renderer, gadget, offsetX, offsetY);
            }

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
            bounds.Offset(offsetX, offsetY);
            inner.Offset(offsetX, offsetY);
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

        private void DrawBoolGadget(SDLRenderer gfx, Gadget gadget, int offsetX, int offsetY)
        {
            Rectangle bounds = gadget.GetBounds();
            bounds.Offset(offsetX, offsetY);
            bool active = gadget.Active;
            bool hover = gadget.MouseHover;
            bool selected = gadget.Selected;
            if (gadget.NoHighlight)
            {
                active = false;
                hover = false;
                selected = false;
            }
            if (!gadget.TransparentBackground)
            {
                Color gradTop = ButtonGradientTopUnFocused;
                Color gradBottom = ButtonGradientBotUnFocused;
                if (selected)
                {
                    gradTop = ButtonGradientTopPushed;
                    gradBottom = ButtonGradientBotPushed;
                }
                else if (active)
                {
                    gradTop = ButtonGradientTopFocused;
                    gradBottom = ButtonGradientBotFocused;
                }
                else if (hover)
                {
                    gradTop = ButtonGradientTopHover;
                    gradBottom = ButtonGradientBotHover;
                }
                gfx.FillVertGradient(bounds, gradTop, gradBottom);
            }
            if (gadget.IsBorderGadget)
            {
                if (hover)
                {
                    DrawBox(gfx, bounds, BorderLight, BorderDark);
                }
            }
            else if (!gadget.NoHighlight)
            {
                DrawBox(gfx, bounds, BorderLight, BorderDark);
            }
            int offset = selected ? 1 : 0;
            bool hasIcon = gadget.Icon != Icons.NONE;
            bool hasText = !string.IsNullOrEmpty(gadget.Text);
            Color tc = gadget.Enabled ? TextColor : DisabledTextColor;
            if (hasIcon && hasText)
            {

            }
            else if (hasIcon)
            {
                gfx.DrawIcon(gadget.Icon, bounds.X, bounds.Y, bounds.Width, bounds.Height, tc, HorizontalAlignment.Center, VerticalAlignment.Center, offset, offset);
            }
            else if (hasText)
            {
                gfx.DrawText(null, gadget.Text, bounds.X, bounds.Y, bounds.Width, bounds.Height, tc, HorizontalAlignment.Center, VerticalAlignment.Center, offset, offset);
            }
        }

        private static void DrawBox(SDLRenderer gfx, Rectangle rect, Color shinePen, Color shadowPen)
        {
            gfx.DrawRect(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, shinePen);
            gfx.DrawRect(rect.X, rect.Y, rect.Width - 1, rect.Height - 1, shadowPen);
        }

    }
}
