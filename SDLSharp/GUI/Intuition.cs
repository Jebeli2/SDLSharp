using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDLSharp.GUI
{
    public static class Intuition
    {
        private static Screen? workbench;
        private static readonly List<Screen> screens = new();
        private static readonly IGuiRenderer guiRenderer = new DefaultGUIRenderer();
        private static int prevMouseX;
        private static int prevMouseY;
        private static int mouseX;
        private static int mouseY;
        private static int diffMouseX;
        private static int diffMouseY;
        private static bool selectMouseDown;
        private static double currentTime;
        private static double lastInputTime;
        private static int timerTick;
        private static int tickIntervall = 100;

        private static Screen? mouseHoverSceen;
        private static Window? mouseHoverWindow;
        private static Gadget? mouseHoverGadget;

        private static Screen? activeScreen;
        private static Window? activeWindow;
        private static Gadget? activeGadget;

        private static Gadget? downGadget;
        private static Gadget? upGadget;
        private static Gadget? selectedGadget;

        private static Queue<Window> activationWindows = new();
        private static Queue<Gadget> activationGadgets = new();

        private static bool moveWindowToFrontOnActivate = true;

        public static bool ShowDebugBounds
        {
            get => guiRenderer.ShowDebugBounds;
            set
            {
                if (guiRenderer.ShowDebugBounds != value)
                {
                    guiRenderer.ShowDebugBounds = value;
                    Invalidate();
                }
            }
        }

        public static void ActivateGadget(Gadget gadget)
        {
            activationGadgets.Enqueue(gadget);
        }
        public static void ActivateWindow(Window window)
        {
            activationWindows.Enqueue(window);
        }

        public static int AddGadget(Window window, Gadget gadget, int position)
        {
            return window.AddGadget(gadget, position);
        }
        public static int AddGList(Window window, IEnumerable<Gadget> gadgets, int position, int numgad, Requester? requester = null)
        {
            int result = -1;
            int count = 0;
            foreach (Gadget gadget in gadgets)
            {
                int insertPos = -1;
                if (requester != null)
                {
                    insertPos = requester.AddGadget(gadget, position);
                }
                else
                {
                    insertPos = window.AddGadget(gadget, position);
                }
                if (result < 0) { result = insertPos; }
                count++;
                if (count >= numgad) { break; }
            }
            return result;
        }
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
                window.Close();
            }
        }

        public static void EndRequest(Requester req, Window window)
        {
            window.EndRequest(req);
        }

        public static void InitRequest(Requester requester, Window window)
        {
            requester.SetWindow(window);
        }

        public static void ModifyProp(Gadget gadget, PropFlags flags, int horizPot, int vertPot, int horizBody, int vertBody)
        {
            gadget.ModifyProp(flags, horizPot, vertPot, horizBody, vertBody);
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
            if (newWindow.Activate) { activationWindows.Enqueue(window); }
            return window;
        }

        public static bool Request(Requester req, Window window)
        {
            if (window.Request(req))
            {
                ClearGadgetsIfWindowMatches(window);
                return true;
            }
            return false;
        }
        public static void WindowToBack(Window window)
        {
            window.ToBack();
        }

        public static void WindowToFront(Window window)
        {
            window.ToFront();
        }

        internal static void Update(double time)
        {
            CheckWindowActivationQueue();
            CheckGadgetActivationQueue();
            CheckTimer(time);

        }
        internal static void PaintDisplay(SDLRenderer renderer)
        {
            foreach (Screen screen in screens)
            {
                screen.Render(renderer, guiRenderer);
            }
        }

        internal static void WindowSizeChanged(int width, int height)
        {
            foreach (Screen screen in screens)
            {
                screen.UpdateScreenSize(width, height);
            }
        }

        internal static bool MouseMoved(int x, int y)
        {
            UpdateMouse(x, y);
            Screen? screen = FindScreen(mouseX, mouseY);
            Window? window = screen?.FindWindow(mouseX, mouseY);
            Gadget? gadget = window?.FindGadget(mouseX, mouseY);
            SetMouseHoverScreen(screen);
            SetMouseHoverWindow(window);
            SetMouseHoverGadget(gadget);
            bool handled = CheckHandled(screen, window, gadget);
            if (CheckWindowDragging(x, y) || CheckWindowSizing(x, y) || CheckGadgetMove(x, y)) { handled = true; lastInputTime = currentTime; }
            return handled;
        }

        internal static bool MouseButtonDown(int x, int y, MouseButton button)
        {
            UpdateMouse(x, y);
            Screen? screen = FindScreen(mouseX, mouseY);
            Window? window = screen?.FindWindow(mouseX, mouseY);
            Gadget? gadget = window?.FindGadget(mouseX, mouseY);
            SetMouseHoverScreen(screen);
            SetMouseHoverWindow(window);
            SetMouseHoverGadget(gadget);
            SetActiveScreen(screen);
            SetActiveWindow(window);
            SetActiveGadget(gadget);
            bool handled = CheckHandled(screen, window, gadget);
            if (button == MouseButton.Left) { selectMouseDown = true; }
            if (CheckGadgetDown(x, y, button)) { handled = true; lastInputTime = currentTime; }
            return handled;
        }

        internal static bool MouseButtonUp(int x, int y, MouseButton button)
        {
            UpdateMouse(x, y);
            Screen? screen = FindScreen(mouseX, mouseY);
            Window? window = screen?.FindWindow(mouseX, mouseY);
            Gadget? gadget = window?.FindGadget(mouseX, mouseY);
            SetMouseHoverScreen(screen);
            SetMouseHoverWindow(window);
            SetMouseHoverGadget(gadget);
            bool handled = CheckHandled(screen, window, gadget);
            if (button == MouseButton.Left) { selectMouseDown = false; }
            if (CheckGadgetUp(x, y, button)) { handled = true; lastInputTime = currentTime; }
            return handled;
        }

        internal static bool KeyDown(SDLKeyEventArgs e)
        {
            bool handled = false;
            if (CheckGadgetKeyDown(e)) { handled = true; lastInputTime = currentTime; }
            return handled;
        }

        internal static bool KeyUp(SDLKeyEventArgs e)
        {
            bool handled = false;
            if (CheckGadgetKeyUp(e)) { handled = true; lastInputTime = currentTime; }
            return handled;
        }

        internal static bool TextInput(SDLTextInputEventArgs e)
        {
            bool handled = false;
            if (CheckGadgetTextInput(e)) { handled = true; lastInputTime = currentTime; }
            return handled;
        }
        private static bool CheckHandled(Screen? screen, Window? window, Gadget? gadget)
        {
            if (screen == null) return false;
            if (gadget != null) return true;
            if (window != null && !window.BackDrop) return true;
            return false;
        }

        private static bool CheckWindowDragging(int x, int y)
        {
            if (activeScreen != null &&
                selectMouseDown &&
                activeGadget != null &&
                activeGadget.SysGadgetType == SysGadgetType.WDragging &&
                activeWindow != null &&
                activeGadget.Window == activeWindow)
            {
                activeWindow.MoveWindow(diffMouseX, diffMouseY, true);
                return true;
            }
            return false;
        }

        private static bool CheckWindowSizing(int x, int y)
        {
            if (activeScreen != null &&
                selectMouseDown &&
                activeGadget != null &&
                activeGadget.SysGadgetType == SysGadgetType.WSizing &&
                activeWindow != null &&
                activeGadget.Window == activeWindow)
            {
                activeWindow.SizeWindow(diffMouseX, diffMouseY, true);
                return true;
            }
            return false;
        }

        private static bool CheckGadgetMove(int x, int y)
        {
            bool result = false;
            if (downGadget == mouseHoverGadget && downGadget != null)
            {
                result |= downGadget.HandleMouseMove(x, y);
            }
            else
            {
                if (downGadget != null)
                {
                    result |= downGadget.HandleMouseMove(x, y);
                }
                else if (mouseHoverGadget != null)
                {
                    result |= mouseHoverGadget.HandleMouseMove(x, y);
                }
            }
            return result;
        }

        private static bool CheckGadgetDown(int x, int y, MouseButton button)
        {
            bool result = false;
            if (button == MouseButton.Left)
            {
                downGadget = mouseHoverGadget;
                if (downGadget != null)
                {
                    SetSelectedGadget(downGadget);
                    result |= downGadget.HandleMouseDown(x, y, false);
                }
            }
            return result;
        }

        private static bool CheckGadgetUp(int x, int y, MouseButton button)
        {
            bool result = false;
            if (button == MouseButton.Left)
            {
                upGadget = mouseHoverGadget;
                if (upGadget != null && upGadget == downGadget)
                {
                    Gadget useGadget = upGadget;
                    result |= useGadget.HandleMouseUp(x, y);
                    if (useGadget.EndGadget && useGadget.Requester != null && useGadget.Window != null)
                    {
                        EndRequest(useGadget.Requester, useGadget.Window);
                    }
                }
                SetSelectedGadget(null);
            }
            return result;
        }

        private static bool CheckGadgetKeyDown(SDLKeyEventArgs e)
        {
            ActionResult result = ActionResult.None;
            if (activeGadget != null)
            {
                Gadget useGadget = activeGadget;
                result |= useGadget.HandleKeyDown(e);
                if (result == ActionResult.NavigateNext)
                {
                    Gadget? next = useGadget.FindNextGadget();
                    if (next != null) { ActivateGadget(next); }
                }
                else if (result == ActionResult.NavigatePrevious)
                {
                    Gadget? next = useGadget.FindPreviousGadget();
                    if (next != null) { ActivateGadget(next); }
                }
                else if (result == ActionResult.GadgetUp)
                {

                }
            }
            return result != ActionResult.None;
        }
        private static bool CheckGadgetKeyUp(SDLKeyEventArgs e)
        {
            ActionResult result = ActionResult.None;
            if (activeGadget != null)
            {
                Gadget useGadget = activeGadget;
                result |= useGadget.HandleKeyUp(e);
                if (result == ActionResult.NavigateNext)
                {
                    Gadget? next = useGadget.FindNextGadget();
                    if (next != null) { ActivateGadget(next); }
                }
                else if (result == ActionResult.NavigatePrevious)
                {
                    Gadget? next = useGadget.FindPreviousGadget();
                    if (next != null) { ActivateGadget(next); }
                }
                else if (result == ActionResult.GadgetUp)
                {

                }
            }
            return result != ActionResult.None;
        }

        private static bool CheckGadgetTextInput(SDLTextInputEventArgs e)
        {
            if (activeGadget != null)
            {
                return activeGadget.HandleTextInput(e);
            }
            return false;
        }

        private static void CheckWindowActivationQueue()
        {
            if (activationWindows.Count > 0)
            {
                Window win = activationWindows.Dequeue();
                SetActiveWindow(win);
            }
        }

        private static void CheckGadgetActivationQueue()
        {
            if (activationGadgets.Count > 0)
            {
                Gadget gad = activationGadgets.Peek();
                if (gad.Window == activeWindow)
                {
                    SetActiveGadget(activationGadgets.Dequeue());
                }
            }
        }

        private static void CheckTimer(double time)
        {
            currentTime = time;
            if (timerTick == 0)
            {
                timerTick = EventHelper.GetExpirationTime(time, tickIntervall);
            }
            if (EventHelper.HasExpired(time, timerTick))
            {
                CheckGadgetTimer(time);
                timerTick = 0;
            }
        }

        private static bool CheckGadgetTimer(double time)
        {
            if (selectedGadget != null && downGadget != null && selectMouseDown)
            {
                if (downGadget.HandleMouseDown(mouseX, mouseY, true))
                {
                    return true;
                }
            }
            return false;
        }

        private static void ClearGadgetsIfWindowMatches(Window window)
        {
            if (activeGadget != null && activeGadget.Window == window)
            {
                SetActiveGadget(null);
            }
            if (selectedGadget != null && selectedGadget.Window == window)
            {
                SetSelectedGadget(null);
            }
            if (mouseHoverGadget != null && mouseHoverGadget.Window == window)
            {
                SetMouseHoverGadget(null);
            }
        }


        private static Screen? FindScreen(int x, int y)
        {
            foreach (Screen screen in screens)
            {
                if (screen.Contains(x, y)) return screen;
            }
            return null;
        }

        private static bool UpdateMouse(int x, int y)
        {
            if (mouseX != x || mouseY != y)
            {
                prevMouseX = mouseX;
                prevMouseY = mouseY;
                mouseX = x;
                mouseY = y;
                diffMouseX = mouseX - prevMouseX;
                diffMouseY = mouseY - prevMouseY;
                lastInputTime = currentTime;
                return true;
            }
            return false;
        }

        private static void SetMouseHoverScreen(Screen? screen)
        {
            if (mouseHoverSceen != screen)
            {
                mouseHoverSceen?.SetMouseHover(false);
                mouseHoverSceen = screen;
                mouseHoverSceen?.SetMouseHover(true);
            }
        }

        private static void SetActiveScreen(Screen? screen)
        {
            if (activeScreen != screen)
            {
                activeScreen?.SetActive(false);
                activeScreen = screen;
                activeScreen?.SetActive(true);
            }
        }

        private static void SetMouseHoverWindow(Window? window)
        {
            if (mouseHoverWindow != window)
            {
                mouseHoverWindow?.SetMouseHover(false);
                mouseHoverWindow = window;
                mouseHoverWindow?.SetMouseHover(true);
            }
        }

        private static void SetActiveWindow(Window? window)
        {
            if (activeWindow != window)
            {
                activeWindow?.SetActive(false);
                activeWindow = window;
                activeWindow?.SetActive(true);
                if (moveWindowToFrontOnActivate && activeWindow != null) { activeWindow.ToFront(); }
            }
        }

        private static void SetMouseHoverGadget(Gadget? gadget)
        {
            if (mouseHoverGadget != gadget)
            {
                mouseHoverGadget?.SetMouseHover(false);
                mouseHoverGadget = gadget;
                mouseHoverGadget?.SetMouseHover(true);
            }
        }

        private static void SetActiveGadget(Gadget? gadget)
        {
            if (activeGadget != gadget)
            {
                activeGadget?.SetActive(false);
                activeGadget = gadget;
                activeGadget?.SetActive(true);
            }
        }
        private static void SetSelectedGadget(Gadget? gadget)
        {
            if (selectedGadget != gadget)
            {
                selectedGadget?.SetSelected(false);
                selectedGadget = gadget;
                selectedGadget?.SetSelected(true);
            }
        }

        private static Screen MakeWorkbench()
        {
            var nwb = new NewScreen { LeftEdge = 0, TopEdge = 0, Width = 1024, Height = 900, DefaultTitle = "Workbench" };
            return OpenScreen(nwb);
        }

        private static void Invalidate()
        {
            foreach (Screen screen in screens) { screen.Invalidate(); }
        }

    }
}
