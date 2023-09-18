namespace SDLSharp.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class GadToolsInfo
    {
        private readonly Gadget gadget;
        public GadToolsInfo(Gadget gadget)
        {
            this.gadget = gadget;
        }
        internal GadgetKind Kind { get; set; }
        internal int SliderMin { get; set; }
        internal int SliderMax { get; set; }
        internal int SliderLevel { get; set; }

        internal int ScrollerTotal { get; set; }
        internal int ScrollerVisible { get; set; }
        internal int ScrollerTop { get; set; }

        internal bool CheckboxChecked { get; set; }
        internal bool Scaled { get; set; }
        internal Action<int>? ValueChangedAction { get; set; }
        internal Action<bool>? CheckedStateChangedAction { get; set; }
        internal string? Format { get; set; }

        internal ListViewInfo? ListViewInfo { get; set; }

        internal void Invalidate()
        {
            ListViewInfo?.Invalidate();
        }

        internal bool HandleMouseMove(Rectangle bounds, int x, int y)
        {
            if (ListViewInfo != null)
            {
                return ListViewInfo.HandleMouseMove(bounds, x, y);
            }
            return false;
        }

        internal bool HandleMouseDown(Rectangle bounds, int x, int y, bool isTimerRepeat = false)
        {
            if (ListViewInfo != null)
            {
                return ListViewInfo.HandleMouseDown(bounds, x, y, isTimerRepeat);
            }
            return false;
        }
        internal bool HandleMouseUp(Rectangle bounds, int x, int y)
        {
            if (ListViewInfo != null)
            {
                return ListViewInfo.HandleMouseUp(bounds, x, y);
            }
            return false;
        }

    }
}
