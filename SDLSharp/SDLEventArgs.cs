namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class SDLWindowPositionEventArgs : EventArgs
    {
        private readonly int x;
        private readonly int y;

        public SDLWindowPositionEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X => x;
        public int Y => y;
    }

    public class SDLWindowSizeEventArgs : EventArgs
    {
        private readonly int width;
        private readonly int height;
        public SDLWindowSizeEventArgs(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public int Width => width;
        public int Height => height;
    }
    public class SDLHandledEventArgs : EventArgs
    {
        private bool handled;

        public bool Handled
        {
            get => handled;
            set => handled = value;
        }

    }

    public class SDLMouseEventArgs : SDLHandledEventArgs
    {
        private readonly int which;
        private readonly int x;
        private readonly int y;
        private readonly MouseButton button;
        private readonly int clicks;
        private readonly KeyButtonState state;
        private readonly int relX;
        private readonly int relY;

        public SDLMouseEventArgs(int which, int x, int y, MouseButton button, int clicks, KeyButtonState state, int relX, int relY)
        {
            this.which = which;
            this.x = x;
            this.y = y;
            this.button = button;
            this.clicks = clicks;
            this.state = state;
            this.relX = relX;
            this.relY = relY;
        }

        public int Which => which;
        public int X => x;
        public int Y => y;
        public MouseButton Button => button;
        public int Clicks => clicks;
        public KeyButtonState State => state;
        public int RelX => relX;
        public int RelY => relY;

    }

    public class SDLMouseWheelEventArgs : SDLHandledEventArgs
    {
        private readonly int which;
        private readonly int x;
        private readonly int y;
        private readonly float preciseX;
        private readonly float preciseY;
        private readonly MouseWheelDirection direction;

        public SDLMouseWheelEventArgs(int which, int x, int y, float preciseX, float preciseY, MouseWheelDirection direction)
        {
            this.which = which;
            this.x = x;
            this.y = y;
            this.preciseX = preciseX;
            this.preciseY = preciseY;
            this.direction = direction;
        }

        public int Which => which;
        public int X => x;
        public int Y => y;
        public float PreciseX => preciseX;
        public float PreciseY => preciseY;
        public MouseWheelDirection Direction => direction;

    }

    public class SDLKeyEventArgs : SDLHandledEventArgs
    {
        private readonly ScanCode scanCode;
        private readonly KeyCode keyCode;
        private readonly KeyMod keyMod;
        private readonly KeyButtonState state;
        private readonly bool repeat;

        public SDLKeyEventArgs(ScanCode scanCode, KeyCode keyCode, KeyMod keyMod, KeyButtonState state, bool repeat)
        {
            this.scanCode = scanCode;
            this.keyCode = keyCode;
            this.keyMod = keyMod;
            this.state = state;
            this.repeat = repeat;
        }

        public ScanCode ScanCode => scanCode;
        public KeyCode KeyCode => keyCode;
        public KeyMod KeyMod => keyMod;
        public KeyButtonState State => state;
        public bool Repeat => repeat;

    }

    public class SDLTextInputEventArgs : SDLHandledEventArgs
    {
        private readonly string text;
        public SDLTextInputEventArgs(string text)
        {
            this.text = text;
        }

        public string Text => text;

    }
    public class SDLWindowUpdateEventArgs : EventArgs
    {
        private double totalTime;
        private double elapsedTime;

        public SDLWindowUpdateEventArgs(double totalTime, double elapsedTime)
        {
            this.totalTime = totalTime;
            this.elapsedTime = elapsedTime;
        }

        public double TotalTime => totalTime;
        public double ElapsedTime => elapsedTime;

        internal void Update(double totalTime, double elapsedTime)
        {
            this.totalTime = totalTime;
            this.elapsedTime = elapsedTime;
        }
    }

    public class SDLWindowPaintEventArgs : EventArgs
    {
        private readonly SDLRenderer renderer;
        private double totalTime;
        private double elapsedTime;
        public SDLWindowPaintEventArgs(SDLRenderer renderer, double totalTime, double elapsedTime)
        {
            this.renderer = renderer;
            this.totalTime = totalTime;
            this.elapsedTime = elapsedTime;
        }
        public SDLRenderer Renderer => renderer;
        public double TotalTime => totalTime;
        public double ElapsedTime => elapsedTime;
        internal void Update(double totalTime, double elapsedTime)
        {
            this.totalTime = totalTime;
            this.elapsedTime = elapsedTime;
        }

    }

    public class SDLWindowLoadEventArgs : EventArgs
    {
        private readonly SDLRenderer renderer;
        public SDLWindowLoadEventArgs(SDLRenderer renderer)
        {
            this.renderer = renderer;
        }
        public SDLRenderer Renderer => renderer;
    }


}
