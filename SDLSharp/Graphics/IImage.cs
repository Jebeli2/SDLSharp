namespace SDLSharp.Graphics
{
    using SDLSharp.Content;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IImage : IResource
    {
        int Width { get; }
        int Height { get; }
        byte AlphaMod { get; set; }
        Color ColorMod { get; set; }
        TextureFilter TextureFilter { get; set; }

    }
}
