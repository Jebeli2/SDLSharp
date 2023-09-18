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
        WSizing,
    }

    [Flags]
    public enum PropFlags
    {
        FreeHoriz = 0x0002,
        FreeVert = 0x0004,
        PropBorderless = 0x0008,
        KnobHit = 0x0100
    }

    public enum GadgetKind
    {
        None,
        Generic,
        Button,
        Checkbox,
        Integer,
        ListView,
        Mx,
        Number,
        Cycle,
        Palette,
        Scroller,
        Reserved,
        Slider,
        String,
        Text
    }

    [Flags]
    public enum PropFreedom
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2,
    }

    [Flags]
    public enum ActionResult
    {
        None = 0x0000,
        Consumed = 0x0001,
        GadgetUp = 0x0002,
        NavigateNext = 0x0004,
        NavigatePrevious = 0x0008,
    }

    public enum ASLRequestType
    {
        FileRequest = 0,
        FontRequest = 1,
        ScreenModeRequest = 2
    }
    public enum TableSelectMode
    {
        Rows,
        Cells,
        Cols
    }
}
