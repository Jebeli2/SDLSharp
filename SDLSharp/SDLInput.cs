namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using static SDL;
    public static class SDLInput
    {
        private static readonly byte[] textBuffer = new byte[64];
        private static IntPtr textMem;

        internal static void Initialize()
        {
            textMem = Marshal.AllocHGlobal(64);
        }

        internal static void Shutdown()
        {
            Marshal.FreeHGlobal(textMem);
        }
        internal static void MessageLoop()
        {
            while (SDL_PollEvent(out var evt) != 0 && !SDLApplication.Quit)
            {
                switch (evt.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        SDLApplication.Exit();
                        break;
                    case SDL_EventType.SDL_WINDOWEVENT:
                        HandleWindowEvent(ref evt.window);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        HandleMouseButtonDownEvent(ref evt.button);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        HandleMouseButtonUpEvent(ref evt.button);
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION:
                        HandleMouseMotionEvent(ref evt.motion);
                        break;
                    case SDL_EventType.SDL_MOUSEWHEEL:
                        HandleMouseWheel(ref evt.wheel);
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        HandleKeyDown(ref evt.key);
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        HandleKeyUp(ref evt.key);
                        break;
                    case SDL_EventType.SDL_TEXTINPUT:
                        HandleTextInput(ref evt.text);
                        break;
                    case SDL_EventType.SDL_DISPLAYEVENT:
                        HandleDisplayEvent(ref evt.display);
                        break;
                }
            }
        }

        private static void HandleWindowEvent([In] ref SDL_WindowEvent evt)
        {
            SDLWindow? window = SDLApplication.GetWindowFromId(evt.windowID);
            if (window != null)
            {
                switch (evt.windowEvent)
                {
                    case SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE: window.RaiseWindowClose(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_DISPLAY_CHANGED: window.RaiseDisplayChanged(evt.data1); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER: window.RaiseWindowEnter(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_EXPOSED: window.RaiseWindowExposed(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED: window.RaiseWindowFocusGained(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST: window.RaiseWindowFocusLost(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_HIDDEN: window.RaiseWindowHidden(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_HIT_TEST: break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_ICCPROF_CHANGED: break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE: window.RaiseWindowLeave(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_MAXIMIZED: window.RaiseWindowMaximized(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED: window.RaiseWindowMinimized(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_MOVED: window.RaiseWindowMoved(evt.data1, evt.data2); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_NONE: break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED: window.RaiseWindowResized(evt.data1, evt.data2); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED: window.RaiseWindowRestored(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_SHOWN: window.RaiseWindowShown(); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED: window.RaiseWindowSizeChanged(evt.data1, evt.data2); break;
                    case SDL_WindowEventID.SDL_WINDOWEVENT_TAKE_FOCUS: break;
                }
            }
        }

        private static void HandleMouseButtonDownEvent([In] ref SDL_MouseButtonEvent evt)
        {
            SDLWindow? window = SDLApplication.GetWindowFromId(evt.windowID);
            window?.RaiseMouseButtonDown(evt.which, (MouseButton)evt.button, (KeyButtonState)evt.state, evt.clicks, evt.x, evt.y);
        }
        private static void HandleMouseButtonUpEvent([In] ref SDL_MouseButtonEvent evt)
        {
            SDLWindow? window = SDLApplication.GetWindowFromId(evt.windowID);
            window?.RaiseMouseButtonUp(evt.which, (MouseButton)evt.button, (KeyButtonState)evt.state, evt.clicks, evt.x, evt.y);
        }

        private static void HandleMouseMotionEvent([In] ref SDL_MouseMotionEvent evt)
        {
            SDLWindow? window = SDLApplication.GetWindowFromId(evt.windowID);
            window?.RaiseMouseMotion(evt.which, evt.x, evt.y, evt.xrel, evt.yrel);
        }

        private static void HandleMouseWheel([In] ref SDL_MouseWheelEvent evt)
        {
            SDLWindow? window = SDLApplication.GetWindowFromId(evt.windowID);
            window?.RaiseMouseWheel(evt.which, evt.x, evt.y, evt.preciseX, evt.preciseY, (MouseWheelDirection)evt.direction);
        }

        private static void HandleKeyDown([In] ref SDL_KeyboardEvent evt)
        {
            SDLWindow? window = SDLApplication.GetWindowFromId(evt.windowID);
            window?.RaiseKeyDown((ScanCode)evt.keysym.scancode, (KeyCode)evt.keysym.sym, (KeyMod)evt.keysym.mod, (KeyButtonState)evt.state, evt.repeat != 0);
        }
        private static void HandleKeyUp([In] ref SDL_KeyboardEvent evt)
        {
            SDLWindow? window = SDLApplication.GetWindowFromId(evt.windowID);
            window?.RaiseKeyUp((ScanCode)evt.keysym.scancode, (KeyCode)evt.keysym.sym, (KeyMod)evt.keysym.mod, (KeyButtonState)evt.state, evt.repeat != 0);
        }

        private static void HandleTextInput([In] ref SDL_TextInputEvent evt)
        {
            SDLWindow? window = SDLApplication.GetWindowFromId(evt.windowID);
            if (window != null)
            {
                Marshal.StructureToPtr(evt, textMem, false);
                Marshal.Copy(textMem, textBuffer, 0, 56);
                int length = 0;
                while (textBuffer[length + 12] != 0 && length < SDL_TEXTINPUTEVENT_TEXT_SIZE)
                {
                    length++;
                }
                if (length > 0)
                {
                    string str = Encoding.UTF8.GetString(textBuffer, 12, length);
                    if (!string.IsNullOrEmpty(str))
                    {
                        window.RaiseTextInput(str);
                    }
                }

            }
        }

        private static void HandleDisplayEvent([In] ref SDL_DisplayEvent evt)
        {

        }
    }
}
