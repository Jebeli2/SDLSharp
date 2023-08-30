namespace SDLSharp
{
    using System;
    using Content;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SDLObject : Resource, IDisposable
    {
        protected readonly IntPtr handle;

        protected SDLObject(IntPtr handle, string name, ContentFlags flags)
            : base(name, flags)
        {
            this.handle = handle;
        }

        public IntPtr Handle => handle;


    }
}
