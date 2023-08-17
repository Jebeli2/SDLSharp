namespace SDLSharp.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum GadgetType
    {
        BoolGadget,
        PropGadget,
        StrGadget,
        CustomGadget
    }

    public enum SysGadgetType
    {
        None,
        WDragging,
        WClosing,
        WMaximizing,
        WMinimizing,
    }
}
