using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public static class Intuition
    {
        private static Screen? workbench;
        private static readonly List<Screen> screens = new();
        private static readonly IGuiRenderer guiRenderer = new DefaultGUIRenderer();

        public static void CloseScreen(Screen? screen)
        {
            if (screen != null && screens.Contains(screen))
            {
                screens.Remove(screen);
                if (workbench == screen) { workbench = null; }
            }
        }
        public static void CloseWindow(Window? window)
        {
            if (window != null)
            {
                Screen screen = window.Screen;
                screen.RemoveWindow(window);
            }
        }

        public static Screen OpenScreen(NewScreen newScreen)
        {
            Screen screen = new Screen(newScreen);
            screens.Add(screen);
            return screen;
        }
        public static Window OpenWindow(NewWindow newWindow)
        {
            workbench ??= MakeWorkbench();
            Window window = new Window(newWindow, workbench);

            return window;
        }

        public static void PaintDisplay(SDLRenderer renderer)
        {
            foreach (Screen screen in screens)
            {
                screen.Render(renderer, guiRenderer);
            }
        }

        private static Screen MakeWorkbench()
        {
            var nwb = new NewScreen { LeftEdge = 0, TopEdge = 0, Width = 1024, Height = 900, DefaultTitle = "Workbench" };
            return OpenScreen(nwb);
        }


    }
}
