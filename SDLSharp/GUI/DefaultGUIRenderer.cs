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
    using System.Xml;

    internal class DefaultGUIRenderer : IGuiRenderer
    {
        private static readonly string spaceText = " ";
        private bool showDebugBounds;
        private Color debugColor = Color.FromArgb(128, Color.Red);
        public DefaultGUIRenderer()
        {
            TextColor = Color.FromArgb(238, 238, 238);
            DisabledTextColor = MkColor(255, 80);
            SelectedTextColor = Color.FromArgb(255, 255, 255, 255);

            BorderDark = MkColor(29, 255);
            BorderLight = MkColor(92, 255);
            BorderMedium = MkColor(35, 255);

            WindowFillUnFocused = MkColor(43, 230);
            WindowFillFocused = MkColor(45, 230);
            WindowTitleUnFocused = MkColor(220, 160);
            WindowTitleFocused = MkColor(225, 190);
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

            PropGradientTop = MkColor(0, 32);
            PropGradientBot = MkColor(0, 92);
            KnobGradientTop = MkColor(100, 100);
            KnobGradientBot = MkColor(128, 100);
            KnobGradientTopHover = MkColor(220, 100);
            KnobGradientBotHover = MkColor(128, 100);
            StrGradientTop = MkColor(0, 32);
            StrGradientBot = MkColor(0, 92);
        }

        public bool ShowDebugBounds
        {
            get => showDebugBounds;
            set => showDebugBounds = value;
        }

        public Color BorderDark { get; set; }
        public Color BorderLight { get; set; }
        public Color BorderMedium { get; set; }
        public Color TextColor { get; set; }
        public Color DisabledTextColor { get; set; }
        public Color SelectedTextColor { get; set; }

        public Color WindowFillUnFocused { get; set; }
        public Color WindowFillFocused { get; set; }
        public Color WindowTitleUnFocused { get; set; }
        public Color WindowTitleFocused { get; set; }

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

        public Color PropGradientTop { get; set; }
        public Color PropGradientBot { get; set; }
        public Color KnobGradientTop { get; set; }
        public Color KnobGradientBot { get; set; }
        public Color KnobGradientTopHover { get; set; }
        public Color KnobGradientBotHover { get; set; }
        public Color StrGradientTop { get; set; }
        public Color StrGradientBot { get; set; }


        private static Color MkColor(int gray, int alpha)
        {
            return Color.FromArgb(alpha, gray, gray, gray);
        }

        public void RenderGadgetBorder(SDLRenderer renderer, Rectangle bounds, bool active, bool hover, bool selected)
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
            renderer.FillVertGradient(bounds, gradTop, gradBottom);
            DrawBox(renderer, bounds, BorderLight, BorderDark);
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
            else if (gadget.IsPropGadget)
            {
                DrawPropGadget(renderer, gadget, offsetX, offsetY);
            }
            else if (gadget.IsStrGadget)
            {
                DrawStrGadget(renderer, gadget, offsetX, offsetY);
            }
            else if (gadget.IsCustomGadget)
            {
                DrawCustomGadget(renderer, gadget, offsetX, offsetY);
            }
            DrawDebugBounds(renderer, gadget, offsetX, offsetY);
        }
        private void DrawRequester(SDLRenderer renderer, Requester requester, int offsetX, int offsetY)
        {
            if (requester.Window != null)
            {
                Rectangle bounds = requester.GetBounds();
                Rectangle inner = requester.GetInnerBounds();
                bounds.Offset(offsetX, offsetY);
                inner.Offset(offsetX, offsetY);
                bool selected = true;
                bool active = requester.Window.Active;
                bool hover = requester.Window.MouseHover;
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
                renderer.FillVertGradient(bounds, gradTop, gradBottom);
                DrawBox(renderer, bounds, BorderLight, BorderDark);
            }
        }

        private void DrawScreen(SDLRenderer renderer, Screen screen, int offsetX, int offsetY)
        {

        }

        private void DrawWindow(SDLRenderer renderer, Window window, int offsetX, int offsetY)
        {
            if (!window.BackDrop)
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
                if (!window.Borderless)
                {
                    if (window.BorderTop > 2) { renderer.FillVertGradient(bounds.Left, bounds.Top, bounds.Width, window.BorderTop, bt, bb); }
                    if (window.BorderLeft > 2) renderer.FillRect(bounds.Left, inner.Top, window.BorderLeft, inner.Height, bb);
                    if (window.BorderRight > 2) renderer.FillRect(bounds.Right - window.BorderRight - 1, inner.Top, window.BorderRight, inner.Height, bb);
                    if (window.BorderBottom > 2) renderer.FillVertGradient(bounds.Left, bounds.Bottom - window.BorderBottom - 1, bounds.Width, window.BorderBottom, bb, bt);
                    DrawBox(renderer, bounds, BorderLight, BorderDark);
                    if (!string.IsNullOrEmpty(window.Title))
                    {
                        renderer.DrawText(null, window.Title, inner.X, bounds.Y, inner.Width, window.BorderTop, active ? WindowTitleFocused : WindowTitleUnFocused);
                    }
                }
            }
        }

        private void DrawBoolGadget(SDLRenderer gfx, Gadget gadget, int offsetX, int offsetY)
        {
            Rectangle bounds = gadget.GetBounds();
            Rectangle inner = gadget.GetInnerBounds();
            bounds.Offset(offsetX, offsetY);
            inner.Offset(offsetX, offsetY);
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
                if (!gadget.BackgroundColor.IsEmpty)
                {
                    gfx.FillRect(bounds, Color.FromArgb(64, gadget.BackgroundColor));
                }
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
                Size textSize = gfx.MeasureText(null, gadget.Text);
                gfx.DrawIcon(gadget.Icon, inner.X, inner.Y, inner.Width / 2 - textSize.Width / 2 - 10, inner.Height, tc, HorizontalAlignment.Right, VerticalAlignment.Center, offset, offset);
                gfx.DrawText(null, gadget.Text, inner.X, inner.Y, inner.Width, inner.Height, tc, HorizontalAlignment.Center, VerticalAlignment.Center, gadget.TextOffsetX + offset, gadget.TextOffsetY + offset);

            }
            else if (hasIcon)
            {
                gfx.DrawIcon(gadget.Icon, inner.X, inner.Y, inner.Width, inner.Height, tc, HorizontalAlignment.Center, VerticalAlignment.Center, offset, offset);
            }
            else if (hasText)
            {
                gfx.DrawText(null, gadget.Text, inner.X, inner.Y, inner.Width, inner.Height, tc, HorizontalAlignment.Center, VerticalAlignment.Center, offset, offset);
            }
        }

        private void DrawPropGadget(SDLRenderer gfx, Gadget gadget, int offsetX, int offsetY)
        {
            if (gadget.PropInfo != null)
            {
                PropInfo propInfo = gadget.PropInfo;
                Rectangle bounds = gadget.GetBounds();
                Rectangle inner = gadget.GetInnerBounds();
                bool active = gadget.Active;
                bool hover = gadget.MouseHover;
                bool selected = gadget.Selected;
                bool knobHit = propInfo.KnobHit;
                bool knobHover = propInfo.KnobHover;
                Rectangle knob = propInfo.GetKnob(bounds);
                bounds.Offset(offsetX, offsetY);
                inner.Offset(offsetX, offsetY);
                knob.Offset(offsetX, offsetY);
                gfx.FillVertGradient(bounds, PropGradientTop, PropGradientBot);
                if (!propInfo.Borderless)
                {
                    DrawBox(gfx, bounds, BorderLight, BorderDark);
                }
                if ((knobHover && hover) || (knobHover && selected) || (knobHit))
                {
                    gfx.FillVertGradient(knob, KnobGradientTopHover, KnobGradientBotHover);
                }
                else
                {
                    gfx.FillVertGradient(knob, KnobGradientTop, KnobGradientBot);
                }
            }
        }

        private void DrawStrGadget(SDLRenderer gfx, Gadget gadget, int offsetX, int offsetY)
        {
            if (gadget.StringInfo != null)
            {
                StringInfo strInfo = gadget.StringInfo;
                Rectangle bounds = gadget.GetBounds();
                Rectangle inner = gadget.GetInnerBounds();
                bounds.Offset(offsetX, offsetY);
                inner.Offset(offsetX, offsetY);
                gfx.FillVertGradient(bounds, StrGradientTop, StrGradientBot);
                DrawBox(gfx, bounds, BorderDark, BorderLight);
                string buffer = strInfo.Buffer;
                int x = inner.X;
                int y = inner.Y;
                int last = buffer.Length;
                int dispPos = strInfo.DispPos;
                SDLFont? font = SDLApplication.DefaultFont;
                if (font != null)
                {
                    gfx.PushClip(inner);
                    for (int i = dispPos; i < last + 1; i++)
                    {
                        char c = ' ';
                        ReadOnlySpan<char> txt;
                        if (i < last)
                        {
                            c = buffer[i];
                            txt = buffer.AsSpan(i, 1);
                        }
                        else
                        {
                            txt = spaceText;
                        }
                        bool selected = (i >= strInfo.BufferSelStart && i < strInfo.BufferSelEnd);
                        font.GetGlyphMetrics(c, out _, out _, out _, out _, out int advance);
                        if (selected)
                        {
                            gfx.FillRect(x, y, advance, inner.Height, Color.LightBlue);
                            gfx.DrawText(font, txt, x, y, 0, 0, SelectedTextColor, HorizontalAlignment.Left, VerticalAlignment.Top);
                        }
                        else
                        {
                            gfx.DrawText(font, txt, x, y, 0, 0, TextColor, HorizontalAlignment.Left, VerticalAlignment.Top);
                        }
                        if (i == strInfo.BufferPos)
                        {
                            if (gadget.Active)
                            {
                                gfx.DrawLine(x, y, x, y + font.Height, TextColor);
                            }
                        }
                        x += advance;
                    }
                    gfx.PopClip();
                }

            }
        }

        private void DrawCustomGadget(SDLRenderer gfx, Gadget gadget, int offsetX, int offsetY)
        {
            gadget.CustomRenderAction?.Invoke(this, gfx, gadget, offsetX, offsetY);
        }
        private static void DrawBox(SDLRenderer gfx, Rectangle rect, Color shinePen, Color shadowPen)
        {
            gfx.DrawRect(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, shinePen);
            gfx.DrawRect(rect.X, rect.Y, rect.Width - 1, rect.Height - 1, shadowPen);
        }

        private void DrawDebugBounds(SDLRenderer gfx, Gadget gadget, int offsetX, int offsetY)
        {
            if (showDebugBounds)
            {
                Rectangle rect = gadget.GetBounds();
                rect.Offset(offsetX, offsetY);
                gfx.DrawRect(rect, debugColor);
            }
        }
    }
}
