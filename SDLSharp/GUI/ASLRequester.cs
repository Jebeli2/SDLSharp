using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class ASLRequester
    {

        public FileInfo? FileInfo => GetFileInfo();

        internal Screen? Screen;
        internal Window? Parent;
        internal Window? Window;

        internal NewWindow? NewWindow;

        public Action? OkSelected;
        public Action? CancelSelected;
        internal virtual void Init(string? dir = null)
        {

        }

        internal virtual FileInfo? GetFileInfo() { return null; }
    }
}
