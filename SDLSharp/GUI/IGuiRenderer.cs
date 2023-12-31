﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public interface IGuiRenderer
    {
        bool ShowDebugBounds { get; set; }
        Color BorderDark { get; }
        Color BorderLight { get; }
        Color BorderMedium { get; }
        Color TextColor { get; }
        Color SelectedTextColor { get; }
        Color ButtonGradientTopFocused { get; }
        Color ButtonGradientBotFocused { get; }
        Color ButtonGradientTopUnFocused { get; }
        Color ButtonGradientBotUnFocused { get; }
        Color ButtonGradientTopPushed { get; }
        Color ButtonGradientBotPushed { get; set; }
        Color ButtonGradientTopHover { get; }
        Color ButtonGradientBotHover { get; set; }
        void RenderGadgetBackground(SDLRenderer renderer, Rectangle bounds, bool active, bool hover, bool selected);
        void RenderGadgetBorder(SDLRenderer renderer, Rectangle rect, bool active, bool hover, bool selected);
        void RenderScreen(SDLRenderer renderer, Screen screen, int offsetX = 0, int offsetY = 0);
        void RenderWindow(SDLRenderer renderer, Window window, int offsetX = 0, int offsetY = 0);
        void RenderGadget(SDLRenderer renderer, Gadget gadget, int offsetX = 0, int offsetY = 0);
        void RenderRequester(SDLRenderer renderer, Requester requester, int offsetX = 0, int offsetY = 0);

    }
}
