namespace SDLSharp.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Flags]
    public enum ContentFlags
    {
        None = 0,
        Image = 1,
        Font = 2,
        Music = 4,
        Sound = 8,
        File = 16,
        Data = 32,
        Resource = 64,
        Created = 128,
        Internal = 256
    }
}
