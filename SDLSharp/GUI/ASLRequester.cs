using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class ASLRequester
    {
        internal Screen? Screen;
        internal Window? Parent;
        internal Window? Window;

        internal NewWindow? NewWindow;


        internal virtual void Init(string? dir = null)
        {

        }

    }
}
