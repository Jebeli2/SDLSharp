using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public interface IGuiRenderer
    {
        void RenderScreen(SDLRenderer renderer, Screen screen, int offsetX = 0, int offsetY = 0);
        void RenderWindow(SDLRenderer renderer, Window window, int offsetX = 0, int offsetY = 0);
        void RenderGadget(SDLRenderer renderer, Gadget gadget, int offsetX = 0, int offsetY = 0);
        void RenderRequester(SDLRenderer renderer, Requester requester, int offsetX = 0, int offsetY = 0);

    }
}
