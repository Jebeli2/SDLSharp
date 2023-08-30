namespace SDLSharp.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IImageRegion
    {
        IImage Image { get; }
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }
        Rectangle SourceRect { get; }
    }
}
